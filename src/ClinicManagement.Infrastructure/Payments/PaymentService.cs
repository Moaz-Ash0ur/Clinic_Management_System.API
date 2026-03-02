using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Invoices;
using ClinicManagement.Domain.Payments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace ClinicManagement.Infrastructure.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPaymobClient _paymob;
        private readonly IConfiguration _config;
        private readonly ILogger<PaymentService> _logger;
        private readonly IHmacValidator _hmacValidator;


        public PaymentService(IUnitOfWork uow, IPaymobClient paymob, IHmacValidator hmacValidator, IConfiguration config, ILogger<PaymentService> logger)
        {
            _uow = uow;
            _paymob = paymob;
            _config = config;
            _logger = logger;
            _hmacValidator = hmacValidator;
        }


        public async Task<Result<string>> InitializePaymentAsync(Guid invoiceId, PaymentMethod method)
        {
            _logger.LogInformation("Initializing payment for InvoiceId {InvoiceId} with method {PaymentMethod}", invoiceId, method);

            try
            {
                var invoiceRepo = _uow.GetRepository<Invoice>();
                var paymentRepo = _uow.GetRepository<Payment>();


                var invoice = await invoiceRepo.GetByIdAsync(invoiceId);
                if (invoice == null)
                {
                    _logger.LogWarning("Invoice {InvoiceId} not found", invoiceId);
                    return Error.Failure("Invoice not found");
                }

                if (invoice.Status == InvoiceStatus.Paid)
                {
                    _logger.LogWarning("Invoice {InvoiceId} already paid", invoiceId);
                    return Error.Failure("Invoice already paid");
                }

                var paymentMethodIdResult = GetPaymentIntegrationId(method.ToString());
                if (!paymentMethodIdResult.IsSuccess)
                {
                    _logger.LogWarning("Payment method {PaymentMethod} not configured", method);
                    return paymentMethodIdResult.TopError;
                }

                var specialReference = Guid.NewGuid().ToString();
                var paymentResult = Payment.Create(Guid.NewGuid(), invoice.Id, specialReference, invoice.TotalAmount, method);

                if (paymentResult.IsError)
                {
                    _logger.LogWarning("Failed to create Payment entity for InvoiceId {InvoiceId}", invoiceId);
                    return paymentResult.TopError;
                }

                var payment = paymentResult.Value!;
                await paymentRepo.AddAsync(payment);
                await _uow.SaveChangesAsync();

                _logger.LogInformation("Payment entity created with SpecialReference {SpecialReference}", specialReference);

                var createPaymentIntentResult = await _paymob.CreatePaymentIntentAsync(invoiceId, paymentMethodIdResult.Value, specialReference);
                if (!createPaymentIntentResult.IsSuccess)
                {
                    _logger.LogError("Failed to create Paymob payment intent for InvoiceId {InvoiceId}", invoiceId);
                    return createPaymentIntentResult.TopError;
                }

                var CheckoutUrl = createPaymentIntentResult.Value;
                _logger.LogInformation("Paymob payment intent created successfully for InvoiceId {InvoiceId}", invoiceId);

                return CheckoutUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while initializing payment for InvoiceId {InvoiceId}", invoiceId);
                return Error.Failure("Exception occurred while initializing payment");
            }
        }


        public async Task<bool> HandleServerCallbackAsync(JsonElement payload, string receivedHmac)
        {
            try
            {
                if (!payload.TryGetProperty("obj", out var obj))
                {
                    _logger.LogWarning("Missing 'obj' in payload.");
                    return false;
                }

                string[] fields = new[]
                {
                   "amount_cents","created_at", "currency", "error_occured",
                   "has_parent_transaction","id","integration_id",
                   "is_3d_secure","is_auth", "is_capture","is_refunded", "is_standalone_payment",
                   "is_voided", "order.id", "owner",
                   "pending","source_data.pan","source_data.sub_type",
                   "source_data.type","success"
                };


                var concatenated = new StringBuilder();

                foreach (var field in fields)
                {
                    string[] parts = field.Split('.');
                    JsonElement current = obj;
                    bool found = true;

                    foreach (var part in parts)
                    {
                        if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(part, out var next))
                            current = next;
                        else
                        {
                            found = false;
                            break;
                        }
                    }

                    if (!found || current.ValueKind == JsonValueKind.Null)
                        concatenated.Append("");
                    else if (current.ValueKind == JsonValueKind.True || current.ValueKind == JsonValueKind.False)
                        concatenated.Append(current.GetBoolean() ? "true" : "false");
                    else
                        concatenated.Append(current.ToString());
                }

                if (!_hmacValidator.IsValid(concatenated.ToString(), receivedHmac))
                {
                    _logger.LogWarning("Invalid HMAC detected.");
                    return false;
                }


                string merchantOrderId = null;

                if (obj.TryGetProperty("order", out var order) &&
                    order.TryGetProperty("merchant_order_id", out var merchantOrderIdElement) &&
                    merchantOrderIdElement.ValueKind != JsonValueKind.Null)
                {
                    merchantOrderId = merchantOrderIdElement.ToString();
                }

                bool isSuccess = obj.TryGetProperty("success", out var successElement) && successElement.GetBoolean();


                if (!string.IsNullOrEmpty(merchantOrderId))
                {
                    var result = await UpdatePaymentStatus(merchantOrderId, isSuccess);
                    if (!result.IsSuccess)
                    {
                        _logger.LogWarning("Failed to update payment status: {Message}", result.Errors);
                        return false;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing server callback");
                return false;
            }

        }



        private Result<int> GetPaymentIntegrationId(string paymentMethod)
        {
            try
            {
                var integrationID = paymentMethod?.ToLower() switch
                {
                    "card" => _config["Paymob:CardIntegrationID"],
                    "wallet" => _config["Paymob:WalletIntegrationID"],
                    _ => null
                };

                if (string.IsNullOrEmpty(integrationID))
                    return Error.Failure($"Integration ID not configured for method {paymentMethod}");

                return Convert.ToInt32(integrationID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Integration ID for payment method {PaymentMethod}", paymentMethod);
                return Error.Failure("Exception occurred while getting payment integration ID");
            }
        }

        private async Task<Result<Success>> UpdatePaymentStatus(string specialReference, bool isSuccess)
        {
            var paymentRepo = _uow.GetRepository<Payment>();

            var payment = await paymentRepo.FindAsync(p => p.TransactionReference == specialReference);

            if (payment == null)
                return Error.Validation($"Payment Wiht Transaction ID {specialReference} Not Found !");

            var invoiceRepo = _uow.GetRepository<Invoice>();

            var invoice = await invoiceRepo.GetByIdAsync(payment.InvoiceId);

            if (invoice == null) return Error.Validation($"Invoice Wiht Id {payment.InvoiceId} Not Found!");


            if (isSuccess)
            {
                payment.MarkAsSuccess(specialReference);
                invoice.MarkAsPaid();
            }
            else
            {
                payment.MarkAsFailed();
                invoice.MarkAsFaild();
            }

            await _uow.SaveChangesAsync();
            return Result.Success;
        }


    }


}
