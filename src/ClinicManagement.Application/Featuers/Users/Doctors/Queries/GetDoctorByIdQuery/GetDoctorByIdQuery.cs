using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Doctors.Queries.GetDoctorByIdQuery
{
    public record GetDoctorByIdQuery(Guid DcotorId) : IRequest<Result<DoctorDto>>;



}
