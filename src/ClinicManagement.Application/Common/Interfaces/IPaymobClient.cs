using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Application.Common.Interfaces
{
    public interface IPaymobClient
    {
        Task<Result<string>> GetAuthTokenAsync();
        Task<Result<string>> CreatePaymentIntentAsync(Guid invoiceId, int paymentMethodId, string specialReference);
    }


}
