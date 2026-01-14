using ClinicManagement.Application.Featuers.Users.Command;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Doctors.Command.RegisterDoctor
{
    public class RegisterDoctorCommand : RegisterCommand , IRequest<Result<DoctorDto>>
    {
        public string specialization {  get; set; }

        public RegisterDoctorCommand(string specialization, string FirstName, string LastName, 
            string Email, string password) : base(FirstName, LastName, Email, password)
        {
            this.specialization = specialization;
        }





    }
}