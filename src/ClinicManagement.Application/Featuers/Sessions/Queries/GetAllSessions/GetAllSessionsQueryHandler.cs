using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Sessions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.Sessions.Queries.GetAllSessions
{
    public sealed class GetAllSessionsQueryHandler : IRequestHandler<GetAllSessionsQuery, Result<List<SessionDto>>>
    {
        private readonly IRepository<Session> _sessionRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllSessionsQueryHandler> _logger;

        public GetAllSessionsQueryHandler(IUnitOfWork uow,IMapper mapper,ILogger<GetAllSessionsQueryHandler> logger)
        {
            _sessionRepo = uow.GetRepository<Session>();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<SessionDto>>> Handle(GetAllSessionsQuery query,CancellationToken ct)
        {

            var sessions = await _sessionRepo
                .GetQueryable()
                .AsNoTracking()
                .ToListAsync(ct);

            if (!sessions.Any())
            {
                _logger.LogWarning("No sessions found");
                return ApplicationErrors.SessionNotFound;
            }


            return _mapper.Map<List<SessionDto>>(sessions);
        }
    }

}
