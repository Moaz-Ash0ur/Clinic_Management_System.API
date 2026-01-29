using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Command.ForgotPassword
{
    public record ForgotPasswordCommand(
    string Email
) : IRequest<Result<Success>>;

}
