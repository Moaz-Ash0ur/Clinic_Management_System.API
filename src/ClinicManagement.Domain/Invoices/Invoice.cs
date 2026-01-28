using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Invoices
{

    public sealed class Invoice : AuditableEntity
    {
        public Guid SessionId { get; private set; }
        public Session? Session { get; private set; }
        public Guid PatientId { get; private set; }
        public decimal TotalAmount { get; private set; }

        public bool IsPaid { get; set; } = false;

        private Invoice() { }

        private Invoice(Guid id, Guid sessionId, Guid patientId, decimal totalAmount) : base(id)
        {
            SessionId = sessionId;
            TotalAmount = totalAmount;
            PatientId = patientId;
        }

        public static Result<Invoice> Create(Guid id, Guid sessionId, Guid PatientId, decimal TotalAmount)
        {
            if (id == Guid.Empty)
                return InvoiceErrors.EmptyInvoiceId;

            if (sessionId == Guid.Empty)
                return InvoiceErrors.SessionRequired;

            return new Invoice(id, sessionId, PatientId, TotalAmount);
        }


        public Result<Success> MarkAsPaid()
        {
            if (IsPaid != false)
                return InvoiceErrors.InvalidInvoiceState;


            IsPaid = true;
            return Result.Success;
        }
    }
}
