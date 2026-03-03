using FluentValidation;

namespace ClinicManagement.Application.Featuers.Payments.Command.InitializePayment
{
    public class InitializePaymentCommandValidator : AbstractValidator<InitializePaymentCommand>
    {
        public InitializePaymentCommandValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("InvoiceId is required.");

            RuleFor(x => x.Method)
                .IsInEnum().WithMessage("Invalid payment method.");
        }
    }




}
