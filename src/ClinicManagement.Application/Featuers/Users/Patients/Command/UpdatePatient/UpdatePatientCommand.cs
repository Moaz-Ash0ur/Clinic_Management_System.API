using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Patients.Command.UpdatePatient
{

    public record UpdatePatientCommand(Guid Id,string FirstName,string LastName,string Email) : IRequest<Result<Success>>;

}
