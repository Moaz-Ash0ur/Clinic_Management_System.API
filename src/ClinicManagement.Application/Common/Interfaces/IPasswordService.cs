using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Application.Common.Interfaces;

public interface IPasswordService
{
    Task<Result<Success>> ChangePasswordAsync(
        string userId,
        string currentPassword,
        string newPassword);

    Task<Result<Success>> SendResetPasswordLinkAsync(string email);

    Task<Result<Success>> ResetPasswordAsync(
        string email,
        string token,
        string newPassword);
}
