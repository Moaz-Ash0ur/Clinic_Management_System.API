using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Payments.Command.HandleWebhook
{

    public record HandleWebhookCommand(JsonElement Payload, string ReceivedHmac)
        : IRequest<Result<Success>>;
}
