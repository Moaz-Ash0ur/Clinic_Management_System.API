using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Application.Common.Interfaces;

public interface IUserManagementService
{
    Task<Result<Success>> UpdateUser(
        string userId,
        string firstName,
        string lastName,
        string email);

    Task<Result<Success>> DeleteUser(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<Result<AppUserDto>> GetUserByIdAsync(string userId);

    Task<string?> GetUserNameAsync(string userId);

    Task<Result<List<AppUserDto>>> GetAllUsers();

    Task<bool> IsUserExist(string email);
}
