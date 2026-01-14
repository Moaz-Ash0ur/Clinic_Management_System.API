using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Doctors.Command.UpdateDoctor
{
    public record UpdateDoctorCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string Email) : IRequest<Result<Success>>;





}
