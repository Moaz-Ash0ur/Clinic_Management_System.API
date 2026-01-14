using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Appointments.Dtos;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Appointments.Queries.GetAppointmentById
{
    public class GetAppointmentByIdHandler : IRequestHandler<GetAppointmentByIdQuery, Result<AppointmentDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Appointment> _repo;
        private readonly IMapper _mapper;

        public GetAppointmentByIdHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Appointment>();
            _mapper = mapper;
        }

        public async Task<Result<AppointmentDto>> Handle(GetAppointmentByIdQuery query, CancellationToken ct)
        {
            var appointment = await _repo.GetByIdAsync(query.AppointmentId);
            if (appointment == null)
                return ApplicationErrors.AppointmentNotFound;

           return _mapper.Map<AppointmentDto>(appointment);
        }



    }




}
