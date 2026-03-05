using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Invoices;
using ClinicManagement.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Payments
{

    public class PaymobClient : IPaymobClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<PaymobClient> _logger;
        private readonly IUnitOfWork _uow;

        private string? _cachedToken;
        private DateTime _tokenExpiry;

        public PaymobClient(HttpClient httpClient, IConfiguration config, ILogger<PaymobClient> logger, IUnitOfWork uow)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
            _uow = uow;
        }

        public async Task<Result<string>> GetAuthTokenAsync()
        {
            try
            {

                if (!string.IsNullOrEmpty(_cachedToken) && _tokenExpiry > DateTime.UtcNow)
                {
                    return _cachedToken;
                }

                var apiKey = _config["Paymob:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                    return Error.Failure("API Key not configured");

                var payload = new { api_key = apiKey };
                var request = new HttpRequestMessage(HttpMethod.Post, "https://accept.paymob.com/api/auth/tokens")
                {
                    Content = JsonContent.Create(payload)
                };

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get Paymob Auth Token. StatusCode: {StatusCode}", response.StatusCode);
                    return Error.Failure("Failed to get auth token");
                }

                var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var token = json.RootElement.GetProperty("token").GetString();

                if (string.IsNullOrEmpty(token))
                    return Error.Failure("Auth token not returned from Paymob");

                _cachedToken = token;
                _tokenExpiry = DateTime.UtcNow.AddMinutes(10);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while getting Paymob Auth Token");
                return Error.Failure("Exception occurred while getting auth token");
            }
        }

        public async Task<Result<string>> CreatePaymentIntentAsync(Guid invoiceId, int paymentMethodId, string specialReference)
        {
            _logger.LogInformation("Creating Paymob intent for InvoiceId {InvoiceId} with PaymentMethodId {PaymentMethodId}", invoiceId, paymentMethodId);


            var invoiceRepo = _uow.GetRepository<Invoice>();
            var patientRepo = _uow.GetRepository<Patient>();

            var invoice = await invoiceRepo.GetByIdAsync(invoiceId);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice {InvoiceId} not found", invoiceId);
                return Error.Failure("Invoice not found");
            }

            var patient = await patientRepo.GetQueryable()
                .AsNoTracking()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == invoice.PatientId);

            if (patient == null || patient.User == null)
            {
                _logger.LogWarning("Patient not found for InvoiceId {InvoiceId}", invoiceId);
                return ApplicationErrors.PatientNotFound;
            }

            try
            {
                if (invoice.TotalAmount <= 0)
                    return Error.Failure("Payment amount must be greater than zero");

                var secretToken = _config["Paymob:SecretKey"];
                if (string.IsNullOrEmpty(secretToken))
                    return Error.Failure("SecretKey not configured");

                var amountCents = (int)(invoice.TotalAmount * 100);

                var payload = new
                {
                    amount = amountCents,
                    currency = "EGP",
                    payment_methods = new[] { paymentMethodId },
                    items = new[]
                    {
                new
                {
                    name = $"Invoice [{invoice.Id}]",
                    amount = amountCents,
                    description = "Clinic Payment For Session With Doctor",
                    quantity = 1
                }
            },
                    billing_data = new
                    {
                        first_name = patient.User.FirstName,
                        last_name = patient.User.LastName,
                        email = patient.User.Email,
                        phone_number = "+2010xxxxxxxx",
                        apartment = "N/A",
                        building = "N/A",
                        floor = "N/A",
                        street = "N/A",
                        state = "N/A",
                        city = "Cairo",
                        country = "EG"
                    },
                    extras = new { invoiceId = invoice.Id },
                    special_reference = specialReference,
                    expiration = 3600,
                    notification_url = "https://yourdomain/api/v1/payments/webhook",
                    redirection_url = "https://yourdomain/index.html#/"
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://accept.paymob.com/v1/intention");
                request.Headers.Authorization = new AuthenticationHeaderValue("Token", secretToken);
                request.Content = JsonContent.Create(payload);

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Paymob intent failed. StatusCode: {StatusCode}, Response: {Response}", response.StatusCode, responseContent);
                    return Error.Failure("Failed to create payment intent");
                }

                var resultJson = JsonDocument.Parse(responseContent);
                var clientSecret = resultJson.RootElement.GetProperty("client_secret").GetString();

                if (string.IsNullOrEmpty(clientSecret))
                    return Error.Failure("Client secret not returned from Paymob");

                var publicKey = _config["Paymob:PublicKey"];
                if (string.IsNullOrEmpty(publicKey))
                    return Error.Failure("PublicKey not configured");

                var checkoutUrl = $"https://accept.paymob.com/unifiedcheckout/?publicKey={publicKey}&clientSecret={clientSecret}";

                _logger.LogInformation("Paymob intent created successfully for InvoiceId {InvoiceId}", invoiceId);

                return checkoutUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while creating Payment Intent for InvoiceId {InvoiceId}", invoiceId);
                return Error.Failure("Exception occurred while creating payment intent");
            }
        }





    }




}
