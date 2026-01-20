
using ClinicManagement.Domain.Prescriptions;

namespace ClinicManagement.Application.Common.Interfaces;

public interface IPrescriptionPdfGenerator
{
    byte[] Generate(Prescription prescription);
}








