namespace ClinicManagement.Application.Featuers.Payments.Command.InitializePayment
{
    public class PaymentRedirectDto
    {
        public Guid InvoiceId { get; set; }
        public string RedirectUrl { get; set; }
    }




}
