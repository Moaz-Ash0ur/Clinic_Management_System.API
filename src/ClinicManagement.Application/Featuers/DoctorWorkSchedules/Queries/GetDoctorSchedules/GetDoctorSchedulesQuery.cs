using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Queries.NewFolder1
{
    public sealed record GetDoctorSchedulesQuery(Guid DoctorId) : IRequest<Result<List<DoctorScheduleDto>>>;





}
