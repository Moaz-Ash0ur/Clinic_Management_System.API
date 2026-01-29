using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Application.Common.Interfaces;

public interface IEmailConfirmationService
{
    Task<Result<Success>> ConfirmEmailAsync(string email, string token);
}
