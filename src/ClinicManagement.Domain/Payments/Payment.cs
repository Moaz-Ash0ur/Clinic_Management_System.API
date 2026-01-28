using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Invoices;
using ClinicManagement.Domain.Patients;
using ClinicManagement.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Payments
{
    public sealed class Payment : AuditableEntity
    {
        public Guid InvoiceId { get; private set; }
        public Invoice? Invoice { get; private set; }

        public decimal Amount { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }

        public string? TransactionReference { get; private set; }

        private Payment() { } 

        private Payment(Guid id, Guid invoiceId, decimal amount, PaymentMethod method)
            : base(id)
        {
            InvoiceId = invoiceId;
            Amount = amount;
            Method = method;
            Status = PaymentStatus.Pending;
        }

        public static Result<Payment> Create(
            Guid id,
            Guid invoiceId,
            decimal amount,
            PaymentMethod method)
        {
            if (id == Guid.Empty)
                return PaymentErrors.EmptyPaymentId;

            if (invoiceId == Guid.Empty)
                return PaymentErrors.InvoiceRequired;

            if (amount <= 0)
                return PaymentErrors.InvalidAmount;

            return new Payment(id, invoiceId, amount, method);
        }

        public Result<Success> MarkAsSuccess(string transactionRef)
        {
            if (Status != PaymentStatus.Pending)
                return PaymentErrors.InvalidPaymentState;

            Status = PaymentStatus.Paid;
            TransactionReference = transactionRef;

            return Result.Success;
        }

        public Result<Success> MarkAsFailed()
        {
            if (Status != PaymentStatus.Pending)
                return PaymentErrors.InvalidPaymentState;

            Status = PaymentStatus.Failed;
            return Result.Success;
        }
    }

}
