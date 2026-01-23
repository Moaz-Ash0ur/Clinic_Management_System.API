using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ClinicManagement.Application.Common.Interfaces;

public interface ISmsService
{
    Task<bool> SendAsync(string phoneNumber,string message, CancellationToken cancellationToken = default);
}



