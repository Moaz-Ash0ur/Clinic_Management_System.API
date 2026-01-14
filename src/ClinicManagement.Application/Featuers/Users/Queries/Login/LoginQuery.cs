using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClinicManagement.Application.Common.Interfaces.ITokenProvider;

namespace ClinicManagement.Application.Featuers.Users.Queries.Login
{
    public record LoginQuery(string Email,string Password) : IRequest<Result<AuthResponse>>;
}
