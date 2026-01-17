using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.MedicalRecords.Command.CreateMedicalRecord;
using ClinicManagement.Application.Featuers.MedicalRecords.Dtos;
using ClinicManagement.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Queries.GetMedicalRecordById
{
    public sealed record GetMedicalRecordByIdQuery(Guid Id) : ICachedQuery<Result<MedicalRecordDto>>
    {
        public string CacheKey => $"medical-record:{Id}";
        public string[] Tags => new[] { "medical-records" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }


}
