using Asp.Versioning;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Users.Command.ConfirmEmail;
using ClinicManagement.Application.Featuers.Users.Command.ForgotPassword;
using ClinicManagement.Application.Featuers.Users.Command.Login;
using ClinicManagement.Application.Featuers.Users.Command.Logout;
using ClinicManagement.Application.Featuers.Users.Command.NewFolder;
using ClinicManagement.Application.Featuers.Users.Command.NewFolder___Copy;
using ClinicManagement.Application.Featuers.Users.Command.RefreshTokens;
using ClinicManagement.Application.Featuers.Users.Command.RevokeRefreshToken;
using ClinicManagement.Application.Featuers.Users.Doctors.Command.RegisterDoctor;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Application.Featuers.Users.Patients.Command.RegisterPatient;
using ClinicManagement.Contracts.Requests.Doctors;
using ClinicManagement.Contracts.Requests.Patients;
using ClinicManagement.Contracts.Requests.Users;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ForgotPasswordRequest = ClinicManagement.Contracts.Requests.Users.ForgotPasswordRequest;
using LoginRequest = ClinicManagement.Contracts.Requests.Users.LoginRequest;
using ResetPasswordRequest = ClinicManagement.Contracts.Requests.Users.ResetPasswordRequest;

namespace ClinicManagement.API.Controllers
{
    [Route("api/v{version:apiVersion}/Auth")]
    [ApiVersion("1.0")]
    public class AuthController : ApiController
    {

        private readonly ISender _sender;
        private readonly IUser _currentUser;
        private string IpAddress => HttpContext.Connection.RemoteIpAddress!.ToString();

        private const string RefreshTokenCookieName = "refreshToken";


        public AuthController(ISender sender, IUser currentUser)
        {
            _sender = sender;
            _currentUser = currentUser;
        }


        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Login for doctor or patient.")]
        [EndpointDescription("Authenticates a doctor or patient and returns a JWT token.")]
        [EndpointName("Login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login( [FromBody ] LoginRequest request, CancellationToken ct)
        {

            var loginQuery = new LoginCommand(request.Email, request.Password, IpAddress);

  
            var result = await _sender.Send(loginQuery, ct);

            if (result.IsSuccess)
                AddRefreshTokenToCookie(result.Value.RefreshToken!, result.Value.ExpiresOnUtc);


            return result.Match(response => Ok(response),Problem);
        }



        [HttpPost("register-doctor")]
        [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Registers a new doctor.")]
        [EndpointDescription("Adds a new doctor to the system and returns doctor details.")]
        [EndpointName("RegisterDoctor")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorRequest request, CancellationToken ct)
        {
            var command = new RegisterDoctorCommand(
                request.specialization,
                request.FirstName,
                request.LastName,
                request.Email,
                request.password             
            );

            var result = await _sender.Send(command, ct);

            return result.Match(response => Ok(response),  Problem);
        }


        [HttpPost("register-patient")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Registers a new patient.")]
        [EndpointDescription("Adds a new patient to the system and returns patient details.")]
        [EndpointName("RegisterPatient")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientRequest request, CancellationToken ct)
        {
            var command = new RegisterPatientCommand(
                request.DateOfBirth,
                request.Gender,
                request.FirstName,
                request.LastName,
                request.Email,
                request.password         
            );

            var result = await _sender.Send(command, ct);

            return result.Match(response => Ok(response),Problem);
        }



        [HttpPost("token/refresh")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Refresh access token")]
        [EndpointDescription("Generate a new access token using refresh token stored in HTTP-only cookie")]
        [EndpointName("RefreshToken")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RefreshToken(CancellationToken ct)
        {
            var refreshToken = Request.Cookies[RefreshTokenCookieName];

            var command = new RefreshTokenCommand(
                refreshToken!,
                IpAddress!
            );

            var result = await _sender.Send(command, ct);

            if (result.IsSuccess)           
                AddRefreshTokenToCookie(result.Value.RefreshToken!, result.Value.ExpiresOnUtc);
            

            return result.Match(response => Ok(response), Problem);

        }



        [HttpPost("token/revoke")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Revoke refresh token")]
        [EndpointDescription("Revoke the refresh token for the currently authenticated user")]
        [EndpointName("RevokeToken")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RevokeToken(CancellationToken ct)
        {
            var refreshToken = Request.Cookies[RefreshTokenCookieName];

            var command = new RevokeRefreshTokenCommand(
                refreshToken!,
                IpAddress!
            );

            var result = await _sender.Send(command, ct);

            return result.Match(_ => Ok(), Problem);
        }


        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("User logout")]
        [EndpointDescription("Logout the currently authenticated user and revoke the current refresh token")]
        [EndpointName("Logout")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> LogoutAsync(CancellationToken ct)
        {         
            var command = new LogoutCommand(_currentUser.Id!,IpAddress!);

            var result = await _sender.Send(command, ct);

            if (result.IsSuccess)
                RemoveRefreshTokenFromCookie();

            return result.Match(_ => Ok(), Problem);
        }



        [HttpPost("change-password")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Change user password")]
        [EndpointDescription("Allows authenticated user to change their password")]
        [EndpointName("ChangePassword")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken ct)
        {
            var command = new ChangePasswordCommand(
                _currentUser.Id!,
                request.CurrentPassword,
                request.NewPassword
            );

            var result = await _sender.Send(command, ct);

            return result.Match(_ => Ok(), Problem);
        }



        [HttpPost("confirm-email")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Confirm email address")]
        [EndpointDescription("Confirms user email using token")]
        [EndpointName("ConfirmEmail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken ct)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var command = new ConfirmEmailCommand(
                request.Email,
                request.Token,
                ipAddress!
            );

            var result = await _sender.Send(command, ct);

            return result.Match(_ => Ok(), Problem);
        }



        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Forgot password")]
        [EndpointDescription("Sends reset password link to email")]
        [EndpointName("ForgotPassword")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
        {
            var command = new ForgotPasswordCommand(request.Email);

            var result = await _sender.Send(command, ct);

            return result.Match(_ => Ok(), Problem);
        }


        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Reset password")]
        [EndpointDescription("Resets user password using token")]
        [EndpointName("ResetPassword")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
        {
            var command = new ResetPasswordCommand(
                request.Email,
                request.Token,
                request.NewPassword
            );

            var result = await _sender.Send(command, ct);

            return result.Match(_ => Ok(), Problem);
        }


        private void AddRefreshTokenToCookie(string refreshToken, DateTime expires)
        {
            Response.Cookies.Append(RefreshTokenCookieName, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expires
            });
        }

        private void RemoveRefreshTokenFromCookie()
        {
            Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }




    }







}
