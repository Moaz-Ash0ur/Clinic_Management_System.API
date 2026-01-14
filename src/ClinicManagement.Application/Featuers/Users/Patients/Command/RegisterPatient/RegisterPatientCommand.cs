using ClinicManagement.Application.Featuers.Users.Command;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Patients.Command.RegisterPatient
{

    public class RegisterPatientCommand : RegisterCommand, IRequest<Result<PatientDto>>
    {
          public DateTime DateOfBirth { get;  set; }
          public Gender Gender { get;  set; }

        public RegisterPatientCommand(DateTime dateOfBirth, Gender gender ,string firstName, string lastName, string email, string password) : base(firstName, lastName, email, password)
        {
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }







       




    }

}
