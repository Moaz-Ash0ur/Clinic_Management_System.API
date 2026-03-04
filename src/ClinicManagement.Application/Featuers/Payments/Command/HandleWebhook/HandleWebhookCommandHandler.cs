using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.Payments.Command.HandleWebhook
{
    public class HandleWebhookCommandHandler : IRequestHandler<HandleWebhookCommand, Result<Success>>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<HandleWebhookCommandHandler> _logger;

        public HandleWebhookCommandHandler(IPaymentService paymentService,ILogger<HandleWebhookCommandHandler> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<Result<Success>> Handle(HandleWebhookCommand command, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Processing webhook...");

                var result = await _paymentService.HandleServerCallbackAsync(
                    command.Payload,
                    command.ReceivedHmac);

                if (!result)
                {
                    _logger.LogWarning("Webhook processing failed: Invalid payload or HMAC");
                    return Error.Failure("Invalid webhook payload or HMAC");
                }

                _logger.LogInformation("Webhook processed successfully");
                return Result.Success;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access: Invalid HMAC signature in webhook");
                return Error.Unauthorized("Unauthorized Invalid HMAC signature");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while handling webhook");
                return Error.Failure("Failed to handle webhook: " + ex.Message);
            }
        }
    }
}
