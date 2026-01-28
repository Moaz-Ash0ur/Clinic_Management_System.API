using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Payments
{
    public static class PaymentErrors
    {
        public static Error EmptyPaymentId =>
            Error.Validation("Payment.EmptyId", "PaymentId is required");

        public static Error InvoiceRequired =>
            Error.Validation("Payment.InvoiceRequired", "Invoice is required");

        public static Error InvalidAmount =>
            Error.Validation("Payment.InvalidAmount", "Invalid payment amount");

        public static Error InvalidPaymentState =>
            Error.Validation("Payment.InvalidState", "Invalid payment state");
    }

}
