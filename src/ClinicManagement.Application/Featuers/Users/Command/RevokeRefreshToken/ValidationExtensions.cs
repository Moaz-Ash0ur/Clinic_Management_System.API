using FluentValidation;

namespace ClinicManagement.Application.Featuers.Users.Command.RevokeRefreshToken
{
    //---------------------------------------------------------------


    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidIp<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Must(ip => System.Net.IPAddress.TryParse(ip, out _))
                .WithMessage("IpAddress is not valid.");
        }
    }




}
