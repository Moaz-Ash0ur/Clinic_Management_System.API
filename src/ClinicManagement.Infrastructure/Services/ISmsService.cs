using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ClinicManagement.Infrastructure.Services
{
    public class TwilioSmsService : ISmsService
    {
        private readonly TwilioSettings _settings;

        public TwilioSmsService(IOptions<TwilioSettings> options)
        {
            _settings = options.Value;
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
        }

        public async Task<bool> SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                var sms = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(_settings.FromNumber),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );

                return sms.Status != MessageResource.StatusEnum.Failed;
            }
            catch
            {
                return false;
            }
        }

    }




}
