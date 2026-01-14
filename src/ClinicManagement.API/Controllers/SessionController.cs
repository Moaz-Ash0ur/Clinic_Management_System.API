using Asp.Versioning;
using ClinicManagement.Application.Featuers.Sessions.Command.CompleteSession;
using ClinicManagement.Application.Featuers.Sessions.Command.CreateSession;
using ClinicManagement.Application.Featuers.Sessions.Command.StartSession;
using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Application.Featuers.Sessions.Queries.GetAllSessions;
using ClinicManagement.Application.Featuers.Sessions.Queries.GetSessionByDoctorId;
using ClinicManagement.Application.Featuers.Sessions.Queries.GetSessionById;
using ClinicManagement.Contracts.Requests.Session;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.API.Controllers
{
    [Route("api/v{version:apiVersion}/sessions")]
    [ApiVersion("1.0")]
    //[Authorize]
    public class SessionController : ApiController
    {
        private readonly ISender _sender;

        public SessionController(ISender sender)
        {
            _sender = sender;
        }




        [HttpPost("create/{appointmentId:guid}")]
        [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Creates a new session.")]
        [EndpointDescription("Creates a session for the specified appointment.")]
        [EndpointName("CreateSession")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateSession(Guid appointmentId, CancellationToken ct)
        {

            var result = await _sender.Send(new CreateSessionCommand(appointmentId), ct);

            return result.Match(
                response => Ok(response),
                Problem
            );
        }



        [HttpPost("start/{sessionId:guid}")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Starts a session.")]
        [EndpointDescription("Marks the specified session as started.")]
        [EndpointName("StartSession")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> StartSession(Guid sessionId, CancellationToken ct)
        {
            var result = await _sender.Send(new StartSessionCommand(sessionId), ct);

            return result.Match(
                response => Ok("Session Started Successfully"),
                Problem
            );
        }




        [HttpPost("complete/{sessionId:guid}")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Completes a session.")]
        [EndpointDescription("Marks the specified session as completed and optionally adds doctor notes.")]
        [EndpointName("CompleteSession")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CompleteSession(Guid sessionId, [FromBody] CompleteSessionRequest request, CancellationToken ct)
        {

            var result = await _sender.Send(new CompleteSessionCommand(sessionId, request.DoctorNotes), ct);

            return result.Match(
                response => Ok(response),
                Problem
            );
        }
    
    
    
    
    
    
    
    
    
    }







}
