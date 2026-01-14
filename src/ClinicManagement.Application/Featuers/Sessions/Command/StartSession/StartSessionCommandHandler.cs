using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Sessions.Command.CreateSession;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Sessions;
using ClinicManagement.Domain.Sessions.Attendances;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.Sessions.Command.StartSession
{
    public class StartSessionCommandHandler : IRequestHandler<StartSessionCommand, Result<Success>>
    {
        private readonly IRepository<Session> _sessionRepo;
        private readonly IRepository<Attendance> _attendanceRepo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<StartSessionCommandHandler> _logger;

        public StartSessionCommandHandler(IUnitOfWork uow, ILogger<StartSessionCommandHandler> logger)
        {
            _uow = uow;
            _sessionRepo = uow.GetRepository<Session>();
            _attendanceRepo = uow.GetRepository<Attendance>();
            _logger = logger;
        }

        public async Task<Result<Success>> Handle(StartSessionCommand command,CancellationToken ct)
        {
            var session = await _sessionRepo.GetByIdAsync(command.SessionId);
            if (session is null)
                return ApplicationErrors.SessionNotFound;

            var attendance = await _attendanceRepo.FindAsync(a => a.SessionId == session.Id);

            if (attendance is null)
                return ApplicationErrors.AttendanceNotFound;

            await _uow.BeginTransactionAsync();

            try
            {
                // Start session
                var sessionResult = session.Start(DateTime.UtcNow);
                if (sessionResult.IsError)
                {
                    return sessionResult.Errors;
                }

                var attendanceResult = attendance.MarkPresent();
                if (attendanceResult.IsError)
                {
                    return attendanceResult.Errors;
                }

                _sessionRepo.Update(session);
                _attendanceRepo.Update(attendance);

                await _uow.CommitAsync();
                await _uow.SaveChangesAsync();

                return Result.Success;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }


    //-----------------------------------------------------







}
