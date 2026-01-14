using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Sessions;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.BackgroundJobs
{

    public class AutoMissedSessionJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AutoMissedSessionJob> _logger;
        private readonly TimeProvider _dateTime;
        private readonly AppSettings _appSettings;

        public AutoMissedSessionJob(ILogger<AutoMissedSessionJob> logger,
            TimeProvider timeProvider, IOptions<AppSettings> options, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _dateTime = timeProvider;
            _appSettings = options.Value;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(_appSettings.AutoMissedSessionCheckFrequencyMinutes));//each 10m reCheck

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
               // _logger.LogInformation("Checking overdue sessions at {Now}", _dateTime.GetUtcNow());

                try
                {

                }
                catch (Exception ex)
                {
                  //  _logger.LogError(ex, "Error marking sessions as missed.");
                }
            }
        }
    }

}
