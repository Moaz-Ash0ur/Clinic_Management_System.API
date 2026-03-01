using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Application.Common.Interfaces
{
    public interface IPaymobClient
    {
        Task<Result<string>> GetAuthTokenAsync();
        Task<Result<PaymentIntentResponse>> CreatePaymentIntentAsync(Guid invoiceId, int paymentMethodId);
    }


}
