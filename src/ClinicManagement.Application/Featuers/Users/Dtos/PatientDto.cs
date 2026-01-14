using ClinicManagement.Application.Common.BaseDto;
using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Application.Featuers.Users.Dtos
{
    public class PatientDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
    }

}
