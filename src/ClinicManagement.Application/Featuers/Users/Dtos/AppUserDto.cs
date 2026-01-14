using ClinicManagement.Domain.Roles;

namespace ClinicManagement.Application.Featuers.Users.Dtos
{
    public class AppUserDto
    {
        public string Id { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public Role role { get; init; }
    }

}
