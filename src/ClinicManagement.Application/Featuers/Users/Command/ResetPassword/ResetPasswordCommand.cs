using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Command.NewFolder
{
    public record ResetPasswordCommand(
       string Email,
       string Token,
       string NewPassword
   ) : IRequest<Result<Success>>;

}
