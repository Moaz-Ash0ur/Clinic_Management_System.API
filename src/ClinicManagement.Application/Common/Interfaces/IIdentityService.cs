

using ClinicManagement.Application.Featuers.Users.Command;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Identity;
using ClinicManagement.Domain.Roles;
using System;
using System.Net;
using System.Security.Cryptography;

namespace ClinicManagement.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<AppUserDto>> LoginAsync(string email, string password);

    Task<Result<AppUserDto>> RegisterDoctorAsync(RegisterCommand cmd);

    Task<Result<AppUserDto>> RegisterPatientAsync(RegisterCommand cmd);

    Task<Result<Success>> UpdateUser(string userId, string firstName, string lastName, string email);

    Task<Result<Success>> DeleteUser(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<Result<AppUserDto>> GetUserByIdAsync(string userId);

    Task<string?> GetUserNameAsync(string userId);

    Task<Result<List<AppUserDto>>?> GetAllUsers();

    Task<bool> IsUserExist(string email);




    //Task<Result<AuthResponse>> LoginGoogleAsync(GoogleSignInVM model, string IpAddress)

}


//    public async Task<Result<AuthResponse>> LoginWithGoogleAsync(GoogleSignInVM model, string ipAddress)
//    {
//        var userResult = await _googleAuthService.GoogleSignInAsync(model);

//        if (!userResult.IsSuccess)
//            return Result<AuthResponse>.Failure(userResult.Message);

//        var user = userResult.Data!;

//        var accessToken = await _jwtService.GenerateToken(user);

//        var refreshToken = new RefreshToken
//        {
//            Token = Guid.NewGuid().ToString(),
//            Created = DateTime.UtcNow,
//            CreatedByIp = ipAddress,
//            Expires = DateTime.UtcNow.AddDays(7),
//            User = user
//        };

//        user.RefreshTokens.Add(refreshToken);
//        await _userManager.UpdateAsync(user);

//        var response = new AuthResponse
//        {
//            AccessToken = accessToken,
//            RefreshToken = refreshToken.Token,
//            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(30)
//        };

//        return Result<AuthResponse>.Success(response, "Login with Google successful");
//    }

