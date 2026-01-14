using Asp.Versioning;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Command.CreateDoctorWorkSchedule;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Command.UpdateDoctorSchedule;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Dtos;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Queries.NewFolder;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Queries.NewFolder1;
using ClinicManagement.Contracts.Requests.DoctorSchedules;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace ClinicManagement.API.Controllers
{
    [Route("api/v{version:apiVersion}/work-schedules")]
    //[Authorize]
    [ApiVersion("1.0")]
    public class DoctorWorkScheduleController : ApiController
    {
        private readonly ISender _sender;

        public DoctorWorkScheduleController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DoctorScheduleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Creates a doctor work schedule.")]
        [EndpointDescription("Creates a new work schedule for a doctor with validation against conflicts.")]
        [EndpointName("CreateDoctorSchedule")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create(
            [FromBody] CreateDoctorScheduleRequest request,
            CancellationToken ct)
        {
            var command = new CreateDoctorWorkScheduleCommand(
                request.DoctorId,
                request.Day,
                request.StartTime,
                request.EndTime);

            var result = await _sender.Send(command, ct);

            return result.Match(
                response => CreatedAtRoute(
                    "GetDoctorScheduleById",
                    new { version = "1.0", scheduleId = response.Id },
                    response),
                Problem);
        }



        [HttpGet("{scheduleId:guid}")]
        [ProducesResponseType(typeof(List<DoctorScheduleDto>), StatusCodes.Status200OK)]
        [EndpointSummary("Gets schedules by Id.")]
        [EndpointDescription("Returns  work schedules by Id.")]
        [EndpointName("GetScheduleById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(Guid scheduleId, CancellationToken ct)
        {
            var result = await _sender.Send(new GetScheduleByIdQuery(scheduleId), ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }



        [HttpGet("doctor/{doctorId:guid}")]
        [ProducesResponseType(typeof(List<DoctorScheduleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves all schedules for a doctor.")]
        [EndpointDescription("Returns all work schedules associated with the specified doctor.")]
        [EndpointName("GetDoctorSchedules")]
        [MapToApiVersion("1.0")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetAllByDoctor(Guid doctorId, CancellationToken ct)
        {
            var result = await _sender.Send(
                new GetDoctorSchedulesQuery(doctorId),
                ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }


        [HttpPut("{scheduleId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [EndpointSummary("Updates a doctor schedule.")]
        [EndpointDescription("Updates doctor work schedule with validation against conflicts.")]
        [EndpointName("UpdateDoctorSchedule")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update(Guid scheduleId,[FromBody] UpdateDoctorScheduleRequest request,CancellationToken ct)
        {
            var command = new UpdateDoctorWorkScheduleCommand(
                scheduleId,
                request.Day,
                request.StartTime,
                request.EndTime);

            var result = await _sender.Send(command, ct);

            return result.Match(
                _ => NoContent(),
                Problem);
        }

    }



}
