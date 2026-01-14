namespace ClinicManagement.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendEmailAsync(string to, CancellationToken cancellationToken = default);

    Task SendSmsAsync(string phoneNumber, CancellationToken cancellationToken = default);
}