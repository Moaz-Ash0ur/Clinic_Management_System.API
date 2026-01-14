using ClinicManagement.Application.Featuers.Users.Doctors.Queries.GetDoctorByIdQuery;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Patients.Queries.GetPatientByIdQuery
{

    public record GetPatientByIdQuery(Guid PatientId) : IRequest<Result<PatientDto>>;

}
