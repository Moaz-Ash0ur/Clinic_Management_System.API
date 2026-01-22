using ClinicManagement.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ClinicManagement.Infrastructure.Services
{
    public class TwilioSmsService : ISmsService
    {
        public Task<bool> SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }


    public class TwilioSettings
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string FromNumber { get; set; }
    }

}
