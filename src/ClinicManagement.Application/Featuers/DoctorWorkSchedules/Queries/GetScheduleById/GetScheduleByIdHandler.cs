using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.DoctorWorkSchedules;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Queries.NewFolder
{
    public sealed class GetScheduleByIdHandler : IRequestHandler<GetScheduleByIdQuery, Result<DoctorScheduleDto>>
    {
        private readonly IRepository<DoctorWorkSchedule> _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetScheduleByIdHandler> _logger;

        public GetScheduleByIdHandler(IUnitOfWork uow,IMapper mapper,ILogger<GetScheduleByIdHandler> logger)
        {
            _repo = uow.GetRepository<DoctorWorkSchedule>();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<DoctorScheduleDto>> Handle(GetScheduleByIdQuery query,CancellationToken ct)
        {
            
            var schedule = await _repo.GetByIdAsync(query.Id);

            if (schedule is null)
            {
               _logger.LogWarning("DoctorWorkSchedule not found. Id: {ScheduleId}", query.Id);
               return ApplicationErrors.ScheduleNotFound;           
            }

            _logger.LogInformation("DoctorWorkSchedule retrieved successfully. Id: {ScheduleId}",query.Id);

            return _mapper.Map<DoctorScheduleDto>(schedule);

        }
    }




}
