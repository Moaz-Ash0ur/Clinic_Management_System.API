using AutoMapper;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Appointments.Dtos;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Appointments.Queries.NewFolder
{
    public class GetAppointmentsQueryHandler : IRequestHandler<GetAppointmentsQuery, Result<List<AppointmentDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Appointment> _repo;
        private readonly IMapper _mapper;

        public GetAppointmentsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Appointment>();
            _mapper = mapper;
        }

        public async Task<Result<List<AppointmentDto>>> Handle(GetAppointmentsQuery query, CancellationToken ct)
        {
            var appointments = await _repo.GetAllAsync();
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }




    }


}
