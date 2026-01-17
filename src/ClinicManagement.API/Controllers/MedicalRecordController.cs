using Asp.Versioning;
using ClinicManagement.Application.Featuers.MedicalRecords.Command.CreateMedicalRecord;
using ClinicManagement.Application.Featuers.MedicalRecords.Command.UpdateMedicalRecord;
using ClinicManagement.Application.Featuers.MedicalRecords.Dtos;
using ClinicManagement.Application.Featuers.MedicalRecords.Queries.GetAllMedicalRecord;
using ClinicManagement.Application.Featuers.MedicalRecords.Queries.GetMedicalRecordById;
using ClinicManagement.Contracts.Requests.MedicalRecord;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.API.Controllers
{
    [Route("api/v{version:apiVersion}/medical-records")]
    [ApiVersion("1.0")]
    // [Authorize]
    public sealed class MedicalRecordController : ApiController
    {

        private readonly ISender _sender;

        public MedicalRecordController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost]
        [ProducesResponseType(typeof(MedicalRecordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("Creates a medical record.")]
        [EndpointDescription("Creates a new medical record for a session or patient.")]
        [EndpointName("CreateMedicalRecord")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromBody] CreateMedicalRecordRequest request,CancellationToken ct)
        {
            var command = new CreateMedicalRecordCommand(request.PatientId, request.SessionId, request.Diagnosis, request.Notes);

            var result = await _sender.Send(command, ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }

    

        [HttpPut("{medicalRecordId:guid}")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("Updates a medical record.")]
        [EndpointDescription("Updates an existing medical record.")]
        [EndpointName("UpdateMedicalRecord")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update(Guid medicalRecordId,[FromBody] UpdateMedicalRecordRequest request, CancellationToken ct)
        {
            var command = new UpdateMedicalRecordCommand(medicalRecordId,request.Diagnosis, request.Notes);
       
           var result = await _sender.Send(command, ct);

            return result.Match(
                _ => Ok(),
                Problem);
        }

      
        [HttpGet("{medicalRecordId:guid}")]
        [ProducesResponseType(typeof(MedicalRecordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Retrieves a medical record by id.")]
        [EndpointDescription("Returns medical record details for the specified id.")]
        [EndpointName("GetMedicalRecordById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(Guid medicalRecordId,CancellationToken ct)
        {
            var result = await _sender.Send(
                new GetMedicalRecordByIdQuery(medicalRecordId), ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }

       

        [HttpGet]
        [ProducesResponseType(typeof(List<MedicalRecordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves all medical records.")]
        [EndpointDescription("Returns a list of all medical records in the system.")]
        [EndpointName("GetAllMedicalRecords")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _sender.Send(
                new GetAllMedicalRecordsQuery(), ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }
    }




}
