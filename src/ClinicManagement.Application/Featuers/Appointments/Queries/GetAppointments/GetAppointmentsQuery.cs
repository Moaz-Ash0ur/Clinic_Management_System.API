using ClinicManagement.Application.Featuers.Appointments.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Appointments.Queries.NewFolder
{
    public record GetAppointmentsQuery() : IRequest<Result<List<AppointmentDto>>>;


}
