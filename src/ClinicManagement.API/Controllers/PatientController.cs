using Asp.Versioning;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Application.Featuers.Users.Patients.Command.DeletePatient;
using ClinicManagement.Application.Featuers.Users.Patients.Command.UpdatePatient;
using ClinicManagement.Application.Featuers.Users.Patients.Queries.GetPatientByIdQuery;
using ClinicManagement.Application.Featuers.Users.Patients.Queries.GetPatientsQuery;
using ClinicManagement.Contracts.Requests.Patients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace ClinicManagement.API.Controllers
{
    [Route("api/v{version:apiVersion}/patients")]
    [ApiVersion("1.0")]
    //[Authorize]
    public class PatientController : ApiController
    {
        private readonly ISender _sender;

        public PatientController(ISender sender)
        {
            _sender = sender;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<PatientDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves all patients.")]
        [EndpointDescription("Returns a list of all registered patients.")]
        [EndpointName("GetPatients")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _sender.Send(new GetPatientsQuery(), ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }





        [HttpGet("{patientId:guid}", Name = "GetPatientById")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves a patient by ID.")]
        [EndpointDescription("Returns detailed information about the specified patient if found.")]
        [EndpointName("GetPatientById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(Guid patientId, CancellationToken ct)
        {
            var result = await _sender.Send(
                new GetPatientByIdQuery(patientId),
                ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }





        [HttpPut("{patientId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Updates patient information.")]
        [EndpointDescription("Updates patient personal details.")]
        [EndpointName("UpdatePatient")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update(Guid patientId,[FromBody] UpdatePatientRequest request,CancellationToken ct)
        {
            var command = new UpdatePatientCommand(
                patientId,
                request.FirstName,
                request.LastName,
                request.Email);

            var result = await _sender.Send(command, ct);

            return result.Match(
                _ => NoContent(),
                Problem);
        }



        [HttpDelete("{patientId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Deletes a patient.")]
        [EndpointDescription("Removes a patient from the system.")]
        [EndpointName("DeletePatient")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(Guid patientId, CancellationToken ct)
        {
            var result = await _sender.Send(new DeletePatientCommand(patientId), ct);

            return result.Match(
                _ => NoContent(),
                Problem);
        }




    }







}
