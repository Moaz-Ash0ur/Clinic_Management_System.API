using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Common.Mappers
{
    public static class PatientMapper
    {
        public static PatientDto ToDto(this Patient patient, AppUserDto user)
        {
            ArgumentNullException.ThrowIfNull(patient);
            ArgumentNullException.ThrowIfNull(user);

            return new PatientDto
            {
                Id = patient.Id,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Email = user.Email!,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender
            };
        }

        public static List<PatientDto> ToDtos(this IEnumerable<Patient> patients, IEnumerable<AppUserDto> users)
        {
            var userDict = users.ToDictionary(u => u.Id);

            return patients.Where(p => userDict.ContainsKey(p.UserId)).Select(p => p.ToDto(userDict[p.UserId])).ToList();
        }



    }


}
