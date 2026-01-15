using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Sessions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.Sessions.Queries.GetSessionByDoctorId
{

    public sealed class GetSessionsByDoctorIdQueryHandler : IRequestHandler<GetSessionsByDoctorIdQuery, Result<List<SessionDto>>>
    {
        private readonly IRepository<Session> _sessionRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSessionsByDoctorIdQueryHandler> _logger;

        public GetSessionsByDoctorIdQueryHandler(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<GetSessionsByDoctorIdQueryHandler> logger)
        {
            _sessionRepo = uow.GetRepository<Session>();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<SessionDto>>> Handle(GetSessionsByDoctorIdQuery query,CancellationToken ct)
        {         
            var sessions = await _sessionRepo
                .GetQueryable()
                .AsNoTracking()
                .Include(s => s.appointment)
                .Where(s =>
                    s.appointment != null &&
                    s.appointment.DoctorId == query.DoctorId &&
                    (
                        s.appointment.Status != AppointmentStatus.Confirmed                       
                    ))
                .ToListAsync(ct);

            if (!sessions.Any())
            {
                _logger.LogWarning(
                    "No sessions found for DoctorId: {DoctorId}",
                    query.DoctorId);

                return ApplicationErrors.SessionNotFound;
            }



            return _mapper.Map<List<SessionDto>>(sessions);
        }
    }

}

