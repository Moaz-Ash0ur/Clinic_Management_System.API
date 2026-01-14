using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Users.Command;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.Patients;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace ClinicManagement.Application.Featuers.Users.Doctors.Command.RegisterDoctor
{
    public class RegisterDoctorCommandHandler : IRequestHandler<RegisterDoctorCommand,Result<DoctorDto>>
    {

        private readonly IRepository<Doctor> _DocRepo;
        private readonly ILogger<RegisterDoctorCommandHandler> _logger;
        private readonly HybridCache _cache;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityService _identityService;


        public RegisterDoctorCommandHandler(ILogger<RegisterDoctorCommandHandler> logger, HybridCache cache,
            IMapper mapper, IRepository<Doctor> docRepo, IUnitOfWork uow, IIdentityService identityService)
        {
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
            _uow = uow;
            _DocRepo = _uow.GetRepository<Doctor>();
            _identityService = identityService;
        }



        public async Task<Result<DoctorDto>> Handle(RegisterDoctorCommand command, CancellationToken ct)
        {

            var email = command.Email.Trim().ToLower();

            if (await _identityService.IsUserExist(email))
                return ApplicationErrors.DoctorAlreadyExists;

            var registerCmd = new RegisterCommand(
                command.FirstName,
                command.LastName,
                email,
                command.password);
       
            try
            {
                await _uow.BeginTransactionAsync();

                var registerResult = await _identityService.RegisterDoctorAsync(registerCmd);

                if (registerResult.IsError)
                    throw new ApplicationException(registerResult.TopError.ToString());


                var createDoctorResult = Doctor.Create(
                    Guid.NewGuid(),
                    registerResult.Value.Id,
                    command.specialization);

                if (createDoctorResult.IsError)
                    throw new ApplicationException(createDoctorResult.TopError.ToString());

               var doctor = createDoctorResult.Value;

                await _DocRepo.AddAsync(doctor);

                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();

                _logger.LogInformation(
                    "Doctor created successfully. Id: {DoctorId}, UserId: {UserId}",
                    doctor.Id,
                    registerResult.Value.Id);


                return new DoctorDto
                {
                    Id = doctor.Id,
                    FirstName = registerResult.Value.FirstName,
                    LastName = registerResult.Value.LastName,
                    Email = registerResult.Value.Email,
                    specialization = doctor.Specialization
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating doctor");

                await _uow.RollbackAsync();
                throw;
            }



        }







    }
}
