using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Patients;
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

    public class ReminderForAppointment : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReminderForAppointment> _logger;
        private readonly TimeProvider _dateTime;
        private readonly AppSettings _appSettings;

        public ReminderForAppointment(ILogger<ReminderForAppointment> logger,
            TimeProvider timeProvider, IOptions<AppSettings> options, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _dateTime = timeProvider;
            _appSettings = options.Value;
            _scopeFactory = scopeFactory;
        }
         
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(3));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Start Job Scheduler Sent SMS For Reminder before one day the Appointment at {Now}", _dateTime.GetUtcNow());

                try
                {
                   await using var scope = _scopeFactory.CreateAsyncScope();
                    var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                    var smsService = scope.ServiceProvider.GetRequiredService<ISmsService>();
                    await using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();


                    var appointments = await appointmentService.GetScheduledAppointment();

                    foreach (var appt in appointments)
                    {
                        var message = @$"
                                      Hello {appt.Patient!.User.FullName},
                                      this is a reminder for your appointment on
                                      {appt.ScheduledAt:dd/MM/yyyy} at {appt.ScheduledAt:hh:mm tt}";

                        var smsSent = await smsService.SendAsync("+201050429118", message);

                        if (smsSent)
                        {
                            appt.SetReminderSend();
                        }
                    }
                    await uow.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error when send sms.");
                }
            }
        }

    }

}
