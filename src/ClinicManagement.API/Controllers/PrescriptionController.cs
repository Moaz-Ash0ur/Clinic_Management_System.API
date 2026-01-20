using Asp.Versioning;
using ClinicManagement.Application.Featuers.Prescriptions.Command.CreatePrescription;
using ClinicManagement.Application.Featuers.Prescriptions.Command.UpdatePrescription;
using ClinicManagement.Application.Featuers.Prescriptions.Dto;
using ClinicManagement.Application.Featuers.Prescriptions.Queries.GetAllPrescriptions;
using ClinicManagement.Application.Featuers.Prescriptions.Queries.GetPrescriptionById;
using ClinicManagement.Application.Featuers.Prescriptions.Queries.GetPrescriptionPdf;
using ClinicManagement.Contracts.Requests.NewFolder;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.API.Controllers
{
    [Route("api/v{version:apiVersion}/prescriptions")]
    [ApiVersion("1.0")]
    // [Authorize]
    public sealed class PrescriptionController : ApiController
    {
        private readonly ISender _sender;

        public PrescriptionController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost]
        [ProducesResponseType(typeof(PrescriptionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("Creates a prescription.")]
        [EndpointDescription("Creates a new prescription for a confirmed session.")]
        [EndpointName("CreatePrescription")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionRequest request, CancellationToken ct)
        {
            var command = new CreatePrescriptionCommand(
                request.PatientId,
                request.SessionId,
                request.MedicationName,
                request.Dosage,
                request.Description);

            var result = await _sender.Send(command, ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }



        [HttpPut("{prescriptionId:guid}")]
        [ProducesResponseType(typeof(Success), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("Updates a prescription.")]
        [EndpointDescription("Updates an existing prescription.")]
        [EndpointName("UpdatePrescription")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update( Guid prescriptionId,[FromBody] UpdatePrescriptionRequest request,CancellationToken ct)
        {
            var command = new UpdatePrescriptionCommand(
                prescriptionId,
                request.MedicationName,
                request.Dosage,
                request.Description);

            var result = await _sender.Send(command, ct);

            return result.Match(
                _ => Ok(),
                Problem);
        }


        [HttpGet("{prescriptionId:guid}")]
        [ProducesResponseType(typeof(PrescriptionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Retrieves a prescription by id.")]
        [EndpointDescription("Returns prescription details for the specified id.")]
        [EndpointName("GetPrescriptionById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(Guid prescriptionId,CancellationToken ct)
        {
            var result = await _sender.Send(
                new GetPrescriptionByIdQuery(prescriptionId),
                ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }



        [HttpGet]
        [ProducesResponseType(typeof(List<PrescriptionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves all prescriptions.")]
        [EndpointDescription("Returns a list of all prescriptions.")]
        [EndpointName("GetAllPrescriptions")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _sender.Send(
                new GetAllPrescriptionsQuery(),
                ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }


        [HttpGet("{prescriptionId:guid}/pdf")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Downloads the Prescription as a PDF file.")]
        [EndpointDescription("Returns the Prescription PDF file for the specified Prescription ID")]
        [EndpointName("GetPrescriptionPdf")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetInvoicePdf(Guid prescriptionId, CancellationToken ct)
        {
            var result = await _sender.Send(new GetPrescriptionPdfQuery(prescriptionId), ct);

            return result.Match(
              response => File(response.Content!, "application/pdf", response.FileName),
              Problem);
        }




    }



}
