using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Command.ConfirmEmail
{
    public record ConfirmEmailCommand(
      string Email,
      string Token,
      string IpAddress
  ) : IRequest<Result<Success>>;


    public sealed class ConfirmEmailCommandHandler
        : IRequestHandler<ConfirmEmailCommand, Result<Success>>
    {
        private readonly IIdentityService _identityService;

        public ConfirmEmailCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<Success>> Handle(
            ConfirmEmailCommand cmd,
            CancellationToken cancellationToken)
        {
            return await _identityService.ConfirmEmailAsync(
                cmd.Email,
                cmd.Token);
        }
    }



    public sealed class ConfirmEmailCommandValidator
    : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");

            RuleFor(x => x.IpAddress)
                .NotEmpty().WithMessage("IP Address is required");
        }
    }





}
