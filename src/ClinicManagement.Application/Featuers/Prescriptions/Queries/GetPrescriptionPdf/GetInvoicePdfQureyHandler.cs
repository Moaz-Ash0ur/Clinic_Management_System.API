using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Prescriptions.Dto;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Prescriptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Prescriptions.Queries.GetPrescriptionPdf
{

    public sealed record GetPrescriptionPdfQuery(Guid PrescriptionId) : IRequest<Result<PrescriptionPdfDto>>;


    public sealed class GetPrescriptionPdfQueryHandler : IRequestHandler<GetPrescriptionPdfQuery, Result<PrescriptionPdfDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPrescriptionPdfGenerator _pdfGenerator;
        private readonly ILogger<GetPrescriptionPdfQueryHandler> _logger;

        public GetPrescriptionPdfQueryHandler(IUnitOfWork uow,IPrescriptionPdfGenerator pdfGenerator,ILogger<GetPrescriptionPdfQueryHandler> logger)
        {
            _pdfGenerator = pdfGenerator;
            _logger = logger;
            _uow = uow;
        }

        public async Task<Result<PrescriptionPdfDto>> Handle(GetPrescriptionPdfQuery query,CancellationToken ct)
        {
            var prescription = await _uow.GetRepository<Prescription>()
                .GetQueryable()
                .AsNoTracking()
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(p => p.Id == query.PrescriptionId, ct);

            if (prescription is null)
            {
                _logger.LogWarning("Prescription not found. Id: {Id}", query.PrescriptionId);
                return Error.NotFound("Prescription not found.");
            }

            try
            {
                var pdfBytes = _pdfGenerator.Generate(prescription);

                return new PrescriptionPdfDto
                {
                    Content = pdfBytes,
                    FileName = $"prescription-{prescription.Id}.pdf"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate prescription PDF.");
                return Error.Failure("Failed to generate prescription PDF.");
            }
        }
    }




}
