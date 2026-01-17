using ClinicManagement.Application.Common.BaseDto;
using ClinicManagement.Application.Featuers.MedicalRecords.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Command.CreateMedicalRecord
{
   
    public sealed record CreateMedicalRecordCommand(Guid PatientId,Guid SessionId,string Diagnosis,string? Notes) : IRequest<Result<MedicalRecordDto>>;




}
