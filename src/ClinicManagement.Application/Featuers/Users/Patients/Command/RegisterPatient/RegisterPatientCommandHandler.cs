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

namespace ClinicManagement.Application.Featuers.Users.Patients.Command.RegisterPatient
{
    public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, Result<PatientDto>>
    {

        private readonly IRepository<Patient> _patientRepo;
        private readonly ILogger<RegisterPatientCommandHandler> _logger;
        private readonly HybridCache _cache;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityService _identityService;


        public RegisterPatientCommandHandler(ILogger<RegisterPatientCommandHandler> logger, HybridCache cache,
            IMapper mapper, IRepository<Doctor> patientRepo, IUnitOfWork uow, IIdentityService identityService)
        {
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
            _uow = uow;
            _patientRepo = _uow.GetRepository<Patient>();
            _identityService = identityService;
        }


        public async Task<Result<PatientDto>> Handle(RegisterPatientCommand command,CancellationToken ct) 
        {
            var email = command.Email.Trim().ToLower();

            if (await _identityService.IsUserExist(email))
                return ApplicationErrors.PatientAlreadyExists;

            var registerCmd = new RegisterCommand(
                command.FirstName,
                command.LastName,
                email,
                command.password);

            Patient patient;


            try
            {
                await _uow.BeginTransactionAsync();

                var registerResult = await _identityService.RegisterPatientAsync(registerCmd);

                if (registerResult.IsError)
                throw new ApplicationException(registerResult.TopError.ToString());


                var createPatientResult = Patient.Create(
                    Guid.NewGuid(),
                    registerResult.Value.Id,
                    command.DateOfBirth,
                    command.Gender);

                if (createPatientResult.IsError)
                    throw new ApplicationException(createPatientResult.TopError.ToString());

                patient = createPatientResult.Value;

                await _patientRepo.AddAsync(patient);

                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();

                _logger.LogInformation(
                    "Patient created successfully. Id: {PatientId}, UserId: {UserId}",
                    patient.Id,
                    registerResult.Value.Id);


              //  return _mapper.Map<Patient, PatientDto>(patient);


                return new PatientDto
                {
                    Id = patient.Id,
                    FirstName = registerResult.Value.FirstName,
                    LastName = registerResult.Value.LastName,
                    Email = registerResult.Value.Email,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating patient");

                await _uow.RollbackAsync();
                throw;
            }
            
        }







    }

}
