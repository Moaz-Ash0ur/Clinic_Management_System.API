using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Settings
{
    public class AppSettings
    {
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }
        public int LocalCacheExpirationInMins { get; set; }
        public int DistributedCacheExpirationMins { get; set; }
        public int DefaultPageNumber { get; set; }
        public int DefaultPageSize { get; set; }
        public int BookingCancellationThresholdMinutes { get; set; }
        public int AutoMissedSessionCheckFrequencyMinutes { get; set; }
        public string CorsPolicyName { get; set; } = default!;
        public string[] AllowedOrigins { get; set; } = default!;
    }
}
