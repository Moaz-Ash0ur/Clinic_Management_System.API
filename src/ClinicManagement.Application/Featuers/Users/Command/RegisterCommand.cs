using ClinicManagement.Domain.Roles;

namespace ClinicManagement.Application.Featuers.Users.Command
{
    public class RegisterCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string password { get; set; }

        public RegisterCommand(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
           this.password = password;
        }

      


    }
}