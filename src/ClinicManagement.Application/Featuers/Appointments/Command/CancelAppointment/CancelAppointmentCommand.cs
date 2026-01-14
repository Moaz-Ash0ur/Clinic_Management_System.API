using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Appointments.Command.DeleteAppointment
{

    public record CancelAppointmentCommand(Guid AppointmentId) : IRequest<Result<Success>>;








    

}
