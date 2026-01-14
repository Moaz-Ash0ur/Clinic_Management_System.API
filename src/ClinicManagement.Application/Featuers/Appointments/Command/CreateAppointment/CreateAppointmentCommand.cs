using ClinicManagement.Application.Featuers.Appointments.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Appointments.Command.CreateAppointment
{


    public sealed record CreateAppointmentCommand(
     Guid DoctorId,
     Guid PatientId,
     DateTime ScheduledAt) : IRequest<Result<AppointmentDto>>;






}
