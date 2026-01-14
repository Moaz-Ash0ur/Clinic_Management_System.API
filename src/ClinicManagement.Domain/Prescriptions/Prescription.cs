using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Prescriptions

{
    public sealed class Prescription : AuditableEntity
    {
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string? Notes { get; set; }
        public Guid SessionId { get; private set; }
        public Session? Session { get; private set; }

        private Prescription() { }

        private Prescription(Guid id, Guid sessionId, Session session)
            : base(id)
        {
            SessionId = sessionId;
            Session = session;
        }

        public static Result<Prescription> Create(Guid id, Guid sessionId, Session session)
        {
            return new Prescription(id, sessionId, session);
        }
    }

}
