using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Identity;
using ClinicManagement.Domain.Patients;
using ClinicManagement.Domain.Roles;
using ClinicManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Data {

    public class ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        AppDbContext context, UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        private readonly ILogger<ApplicationDbContextInitialiser> _logger = logger;
        private readonly AppDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            //Roles
            foreach (var roleName in Enum.GetNames(typeof(Role)))
            {
                if (!_roleManager.Roles.Any(r => r.Name == roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Admin
            var adminEmail = "admin@gmail.com";
            var adminUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "admin",
                LastName = "admin",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            if (!_userManager.Users.Any(u => u.Email == adminEmail))
            {
                await _userManager.CreateAsync(adminUser, "Admin123");
                await _userManager.AddToRoleAsync(adminUser, nameof(Role.Admin));
            }

            // User as Doctor
            var doctorEmail = "dr.john@gmail.com";
            var doctorUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "john",
                LastName = "De",
                UserName = doctorEmail,
                Email = doctorEmail,
                EmailConfirmed = true
            };

            if (!_userManager.Users.Any(u => u.Email == doctorEmail))
            {
                await _userManager.CreateAsync(doctorUser, "Doctor123");
                await _userManager.AddToRoleAsync(doctorUser, nameof(Role.Doctor));

                _context.Doctors.Add(
                    Doctor.Create
                    (Guid.NewGuid(), 
                    doctorUser.Id, 
                    "General Medicine").Value);
            }

            // User as Patient
            var patientEmail = "patient.mary@gmail.com";
            var patientUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "mary",
                LastName = "De",
                UserName = patientEmail,
                Email = patientEmail,
                EmailConfirmed = true
            };

            if (!_userManager.Users.Any(u => u.Email == patientEmail))
            {
                await _userManager.CreateAsync(patientUser, "Patient123");
                await _userManager.AddToRoleAsync(patientUser, nameof(Role.Patient));

                _context.Patients.Add(
                    Patient.Create
                    (Guid.NewGuid(), patientUser.Id,
                    new DateTime(1990, 1, 1),
                    Gender.Female).Value);

            }

            await _context.SaveChangesAsync();
        }

    }

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

            await initialiser.InitialiseAsync();

            await initialiser.SeedAsync();
        }
 }


}