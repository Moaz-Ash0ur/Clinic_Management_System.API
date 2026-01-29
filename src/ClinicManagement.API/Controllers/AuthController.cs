using Asp.Versioning;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Users.Command.ConfirmEmail;
using ClinicManagement.Application.Featuers.Users.Command.ForgotPassword;
using ClinicManagement.Application.Featuers.Users.Command.NewFolder;
using ClinicManagement.Application.Featuers.Users.Command.NewFolder___Copy;
using ClinicManagement.Application.Featuers.Users.Doctors.Command.RegisterDoctor;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Application.Featuers.Users.Patients.Command.RegisterPatient;
using ClinicManagement.Application.Featuers.Users.Queries.Login;
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
using ResetPasswordRequest = ClinicManagement.Contracts.Requests.Users.ResetPasswordRequest;

namespace ClinicManagement.API.Controllers
{
    [Route("api/v{version:apiVersion}/Auht")]
    [ApiVersion("1.0")]
    public class AuthController : ApiController
    {

        private readonly ISender _sender;
        private readonly IUser _currentUser;
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
        public async Task<IActionResult> Login(LoginQuery loginQuery, CancellationToken ct)
        {
            var result = await _sender.Send(loginQuery, ct);

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



       

    }







}
