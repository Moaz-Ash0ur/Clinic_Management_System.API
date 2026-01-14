using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.DoctorWorkSchedules;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Queries.NewFolder1
{
    public sealed class GetDoctorSchedulesHandler  : IRequestHandler<GetDoctorSchedulesQuery, Result<List<DoctorScheduleDto>>>
    {
        private readonly IRepository<DoctorWorkSchedule> _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetDoctorSchedulesHandler> _logger;

        public GetDoctorSchedulesHandler(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<GetDoctorSchedulesHandler> logger)
        {
            _repo = uow.GetRepository<DoctorWorkSchedule>();
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<DoctorScheduleDto>>> Handle(GetDoctorSchedulesQuery query,CancellationToken ct)
        {
            var schedules = await _repo.GetQueryable()
                .Where(s => s.DoctorId == query.DoctorId)
                .OrderBy(s => s.dayofWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync(ct);

            if(!schedules.Any())
            {
                _logger.LogWarning("Doctor Work Schedules not found. Doctor ID: {DoctorID}", query.DoctorId);
                return ApplicationErrors.ScheduleNotFound;
            }

            return _mapper.Map<List<DoctorScheduleDto>>(schedules);        
        }



    }





}
