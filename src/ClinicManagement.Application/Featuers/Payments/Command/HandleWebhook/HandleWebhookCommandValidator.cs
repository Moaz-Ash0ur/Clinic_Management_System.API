using FluentValidation;

namespace ClinicManagement.Application.Featuers.Payments.Command.HandleWebhook
{
    public class HandleWebhookCommandValidator : AbstractValidator<HandleWebhookCommand>
    {
        public HandleWebhookCommandValidator()
        {
            RuleFor(x => x.Payload)
                .NotEmpty().WithMessage("Webhook payload is required.");

            RuleFor(x => x.ReceivedHmac)
                .NotEmpty().WithMessage("Received HMAC signature is required.");
        }
    }
}
