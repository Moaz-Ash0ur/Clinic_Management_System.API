using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Contracts.Requests.Session
{
    public class CompleteSessionRequest
    {
        public Guid sessionId { get; set; }
        public string? DoctorNotes { get; set; }
    }

}
