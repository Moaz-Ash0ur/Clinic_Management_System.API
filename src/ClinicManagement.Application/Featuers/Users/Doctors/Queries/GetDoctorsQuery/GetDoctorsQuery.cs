using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Application.Featuers.Users.Patients.Queries.GetPatientsQuery;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Doctors.Queries.GetDoctorsQuery
{
    public record GetDoctorsQuery() : IRequest<Result<List<DoctorDto>>>;



}
