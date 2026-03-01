using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Invoices;
using ClinicManagement.Domain.Patients;
using ClinicManagement.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using static System.Net.WebRequestMethods;

namespace ClinicManagement.Application.Common.Interfaces
{

    public interface IPaymentService
    {
        Task<Result<string>> InitializePaymentAsync(Guid invoiceId, PaymentMethod method);
        Task<bool> HandleServerCallbackAsync(JsonElement payload, string receivedHmac);
    }

}
