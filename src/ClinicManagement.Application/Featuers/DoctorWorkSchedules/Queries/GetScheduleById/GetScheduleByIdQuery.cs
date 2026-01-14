using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Command.CreateDoctorWorkSchedule;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Queries.NewFolder
{
    public sealed record GetScheduleByIdQuery(Guid Id) : IRequest<Result<DoctorScheduleDto>>;




}
