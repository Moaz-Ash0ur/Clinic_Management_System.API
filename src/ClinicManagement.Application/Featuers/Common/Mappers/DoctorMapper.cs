using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Doctors;

namespace ClinicManagement.Application.Featuers.Common.Mappers
{
    public static class DoctorMapper
    {
        public static DoctorDto ToDto(this Doctor doctor, AppUserDto user)
        {
            ArgumentNullException.ThrowIfNull(doctor);
            ArgumentNullException.ThrowIfNull(user);

            return new DoctorDto
            {
                Id = doctor.Id,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Email = user.Email!,
                specialization = doctor.Specialization
            };
        }

        public static List<DoctorDto> ToDtos(this IEnumerable<Doctor> doctors, IEnumerable<AppUserDto> users)
        {
            var userDict = users.ToDictionary(u => u.Id);

            return doctors.Where(d => userDict.ContainsKey(d.UserId)).Select(d => d.ToDto(userDict[d.UserId])).ToList();
        }



    }


}
