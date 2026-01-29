using ClinicManagement.Application.Featuers.Users.Command.NewFolder;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Command.NewFolder___Copy
{

    public record ChangePasswordCommand(string UserId,string CurrentPassword,string NewPassword) : IRequest<Result<Success>>;


}
