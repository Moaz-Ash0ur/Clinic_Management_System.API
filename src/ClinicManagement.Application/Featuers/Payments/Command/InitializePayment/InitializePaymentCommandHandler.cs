using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.Payments.Command.InitializePayment
{
    public class InitializePaymentCommandHandler : IRequestHandler<InitializePaymentCommand, Result<PaymentRedirectDto>>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<InitializePaymentCommandHandler> _logger;

        public InitializePaymentCommandHandler(IPaymentService paymentService,ILogger<InitializePaymentCommandHandler> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<Result<PaymentRedirectDto>> Handle(InitializePaymentCommand command,CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Initializing payment for InvoiceId: {InvoiceId}", command.InvoiceId);


                var createPaymentResult = await _paymentService
                    .InitializePaymentAsync(command.InvoiceId,command.Method);

                if (createPaymentResult.IsError)
                {
                    _logger.LogWarning("Payment initialization failed for InvoiceId: {InvoiceId}. Errors: {Errors}",command.InvoiceId,
                                       string.Join(", ", createPaymentResult.Errors));
                    return createPaymentResult.Errors;
                }

                var dto = new PaymentRedirectDto
                {
                    InvoiceId = command.InvoiceId,
                    RedirectUrl = createPaymentResult.Value
                };

                _logger.LogInformation("Payment successfully initialized for InvoiceId: {InvoiceId}, RedirectUrl: {RedirectUrl}",
                                       command.InvoiceId,
                                       dto.RedirectUrl);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while initializing payment for InvoiceId: {InvoiceId}", command.InvoiceId);
                return Error.Failure($"Failed to initialize payment: {ex.Message}");
            }
        }
    }




}
