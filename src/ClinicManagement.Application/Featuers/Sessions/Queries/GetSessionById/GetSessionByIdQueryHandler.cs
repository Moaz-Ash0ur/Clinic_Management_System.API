using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Sessions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.Sessions.Queries.GetSessionById
{
    public sealed class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, Result<SessionDto>>
    {
        private readonly IRepository<Session> _sessionRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSessionByIdQueryHandler> _logger;

        public GetSessionByIdQueryHandler(IUnitOfWork uow,IMapper mapper, ILogger<GetSessionByIdQueryHandler> logger)
        {
            _sessionRepo = uow.GetRepository<Session>();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<SessionDto>> Handle(GetSessionByIdQuery query,CancellationToken ct)
        {
           var session = await _sessionRepo.GetByIdAsync(query.SessionId);

            if (session is null)
            {
                _logger.LogWarning(
                    "Session not found. SessionId: {SessionId}",
                    query.SessionId);

                return ApplicationErrors.SessionNotFound;
            }

        
            return _mapper.Map<SessionDto>(session);
        }
    }

}
