using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.MedicalRecords.Command.CreateMedicalRecord;
using ClinicManagement.Application.Featuers.MedicalRecords.Dtos;
using ClinicManagement.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Queries.GetAllMedicalRecord
{
  
    public sealed record GetAllMedicalRecordsQuery(int PageNumber = 1,int PageSize = 10): ICachedQuery<Result<IReadOnlyList<MedicalRecordDto>>>
    {
        public string CacheKey => $"medical-records:page:{PageNumber}:size:{PageSize}";

        public string[] Tags => new[] { "medical-records" };

        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }


}
