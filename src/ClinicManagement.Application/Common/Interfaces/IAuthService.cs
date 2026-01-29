

using ClinicManagement.Application.Featuers.Users.Command;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Roles;

namespace ClinicManagement.Application.Common.Interfaces;

public interface IAuthService
{
    Task<Result<AppUserDto>> RegisterAsync(RegisterCommand cmd, Role role);

    Task<Result<AppUserDto>> LoginAsync(string email, string password);

    Task<Result<AppUserDto>> RegisterDoctorAsync(RegisterCommand cmd);

    Task<Result<AppUserDto>> RegisterPatientAsync(RegisterCommand cmd);
}
