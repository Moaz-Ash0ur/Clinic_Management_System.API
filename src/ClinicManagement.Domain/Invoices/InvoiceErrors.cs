using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Invoices
{
    public static class InvoiceErrors
    {
        public static Error EmptyInvoiceId =>
            Error.Validation("Invoice.EmptyId", "InvoiceId is required");

        public static Error SessionRequired =>
            Error.Validation("Invoice.SessionRequired", "Session is required");

        public static Error InvalidInvoiceState =>
            Error.Validation("Invoice.InvalidState", "Invalid invoice state");

        public static Error InvalidPrice =>
            Error.Validation("Invoice.InvalidPrice", "Price must be greater than zero");
    }
}
