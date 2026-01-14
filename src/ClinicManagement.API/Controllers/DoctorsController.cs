using Asp.Versioning;
using ClinicManagement.Application.Featuers.Users.Doctors.Command.DeleteDoctor;
using ClinicManagement.Application.Featuers.Users.Doctors.Command.UpdateDoctor;
using ClinicManagement.Application.Featuers.Users.Doctors.Queries.GetDoctorByIdQuery;
using ClinicManagement.Application.Featuers.Users.Doctors.Queries.GetDoctorsQuery;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Contracts.Requests.Doctors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClinicManagement.API.Controllers
{

    [Route("api/v{version:apiVersion}/doctors")]
    [ApiVersion("1.0")]
   // [Authorize]
    public class DoctorController : ApiController
    {
        private readonly ISender _sender;

        public DoctorController(ISender sender)
        {
            _sender = sender;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<DoctorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves all doctors.")]
        [EndpointDescription("Returns a list of all registered doctors.")]
        [EndpointName("GetDoctors")]
        [MapToApiVersion("1.0")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _sender.Send(new GetDoctorsQuery(), ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }



        [HttpGet("{doctorId:guid}", Name = "GetDoctorById")]
        [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves a doctor by ID.")]
        [EndpointDescription("Returns doctor details if found.")]
        [EndpointName("GetDoctorById")]
        [MapToApiVersion("1.0")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetById(Guid doctorId, CancellationToken ct)
        {
            var result = await _sender.Send(new GetDoctorByIdQuery(doctorId), ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }



        [HttpPut("{doctorId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Updates doctor information.")]
        [EndpointDescription("Updates doctor profile and specialization.")]
        [EndpointName("UpdateDoctor")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update(Guid doctorId,[FromBody] UpdateDoctorRequest request,CancellationToken ct)
        {
            var command = new UpdateDoctorCommand(
                doctorId,
                request.FirstName,
                request.LastName
                ,request.Email);

            var result = await _sender.Send(command, ct);

            return result.Match(
                _ => NoContent(),
                Problem);
        }



        [HttpDelete("{doctorId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Deletes a doctor.")]
        [EndpointDescription("Removes a doctor from the system.")]
        [EndpointName("DeleteDoctor")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(Guid doctorId, CancellationToken ct)
        {
            var result = await _sender.Send(new DeleteDoctorCommand(doctorId), ct);

            return result.Match(
                _ => NoContent(),
                Problem);
        }



    }

}
