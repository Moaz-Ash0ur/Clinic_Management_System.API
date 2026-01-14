using Asp.Versioning;
using ClinicManagement.Application.Featuers.Appointments.Command.CreateAppointment;
using ClinicManagement.Application.Featuers.Appointments.Command.DeleteAppointment;
using ClinicManagement.Application.Featuers.Appointments.Command.UpdateAppointment;
using ClinicManagement.Application.Featuers.Appointments.Dtos;
using ClinicManagement.Application.Featuers.Appointments.Queries.GetAppointmentById;
using ClinicManagement.Application.Featuers.Appointments.Queries.NewFolder;
using ClinicManagement.Contracts.Requests.Appointments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.API.Controllers
{
    [Route("api/v{version:apiVersion}/appointments")]
   // [Authorize]
    [ApiVersion("1.0")]
    public class AppointmentController : ApiController
    {
        private readonly ISender _sender;

        public AppointmentController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Creates a new appointment.")]
        [EndpointDescription("Schedules a new appointment for a doctor and patient.")]
        [EndpointName("CreateAppointment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request, CancellationToken ct)
        {
            var command = new CreateAppointmentCommand(
                request.DoctorId,
                request.PatientId,
                request.ScheduledAt
            );

            var result = await _sender.Send(command, ct);

            return result.Match(
                response => CreatedAtRoute(
                    routeName: "GetAppointmentById",
                    routeValues: new { version = "1.0", appointmentId = response.Id },
                    value: response),
                Problem
            );
        }


        [HttpGet("{appointmentId}", Name = "GetAppointmentById")]
        [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Get appointment details by ID.")]
        [EndpointDescription("Returns the details of a specific appointment.")]
        [EndpointName("GetAppointmentById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAppointmentById(Guid appointmentId, CancellationToken ct)
        {
            var query = new GetAppointmentByIdQuery(appointmentId);

            var result = await _sender.Send(query, ct);

            return result.Match(
                response => Ok(response),
                Problem
            );
        }


        [HttpGet(Name = "GetAppointments")]
        [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Get all appointments.")]
        [EndpointDescription("Returns All appointments.")]
        [EndpointName("GetAppointments")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var query = new GetAppointmentsQuery();

            var result = await _sender.Send(query, ct);

            return result.Match(
                response => Ok(response),
                Problem
            );
        }



        [HttpPut("{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Updates an existing appointment.")]
        [EndpointDescription("Updates details such as date/time for an existing appointment.")]
        [EndpointName("UpdateAppointment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateAppointment(Guid appointmentId, [FromBody] UpdateAppointmentRequest request, CancellationToken ct)
        {
            var command = new RescheduleAppointmentCommand(
                appointmentId,
                request.ScheduledAt
            );

            var result = await _sender.Send(command, ct);

            return result.Match(
                _ => NoContent(),
                Problem
            );
        }

        [HttpPut("Cancel/{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Cancel an existing appointment.")]
        [EndpointDescription("Cancel appointment before start a session.")]
        [EndpointName("CancelAppointment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CancelAppointment(Guid appointmentId, CancellationToken ct)
        {
            var command = new CancelAppointmentCommand(appointmentId);

            var result = await _sender.Send(command, ct);

            return result.Match(
                _ => NoContent(),
                Problem
            );
        }







    }

}
