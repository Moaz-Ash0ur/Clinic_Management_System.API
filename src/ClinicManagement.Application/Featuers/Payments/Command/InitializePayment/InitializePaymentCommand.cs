using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Payments.Command.InitializePayment
{
    public record InitializePaymentCommand(Guid InvoiceId, PaymentMethod Method)
        : IRequest<Result<PaymentRedirectDto>>;




}
