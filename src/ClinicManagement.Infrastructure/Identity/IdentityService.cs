using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Users.Command;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Identity;
using ClinicManagement.Domain.Roles;
using ClinicManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public IdentityService( UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        private async Task<Result<AppUserDto>> RegisterInternalAsync(RegisterCommand cmd, Role role)
        {
            if (await IsUserExist(cmd.Email))
                return Error.Conflict("Email already exists");

            var user = new AppUser
            {
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
                Email = cmd.Email,
                UserName = cmd.Email
            };

            var createResult = await _userManager.CreateAsync(user, cmd.password);

            if (!createResult.Succeeded)
                return Error.Failure(createResult.Errors.First().Description);

            var roleName = role.ToString();

            if (!await _roleManager.RoleExistsAsync(roleName))
                return Error.NotFound($"Role '{roleName}' not found");

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
                return Error.Failure(roleResult.Errors.First().Description);

            return await MapToDtoAsync(user);
        }

        public async Task<Result<AppUserDto>> RegisterDoctorAsync(RegisterCommand cmd)
        {
            return await RegisterInternalAsync(cmd, Role.Doctor);
        }

        public async Task<Result<AppUserDto>> RegisterPatientAsync(RegisterCommand cmd)
        {
            return await RegisterInternalAsync(cmd, Role.Patient);
        }

        public async Task<Result<AppUserDto>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Error.NotFound("user not found");

            var result = await _userManager.CheckPasswordAsync(user, password);

            if (!result)
                return Error.Failure("Invalid email or password");

            return await MapToDtoAsync(user);
        }
    
        public async Task<Result<AppUserDto>> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Error.NotFound("User not found");

            return await MapToDtoAsync(user);
        }

        public async Task<string?> GetUserNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.UserName;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> IsUserExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
            
        public async Task<Result<Success>> UpdateUser(string userId, string firstName, string lastName, string email)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ApplicationErrors.UserNotFound;

            var emailUser = await _userManager.FindByEmailAsync(email);
            if (emailUser != null && emailUser.Id != user.Id)
                return ApplicationErrors.UserAlreadyExists;

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.UserName = email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Error.Failure(result.Errors.First().Description);

            return Result.Success;
        }

        public async Task<Result<Success>> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ApplicationErrors.UserNotFound;

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return Error.Failure(result.Errors.First().Description);

            return Result.Success;
        }

        private async Task<AppUserDto> MapToDtoAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return new AppUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Email = user.Email!,
                role = Enum.Parse<Role>(roles.First(), ignoreCase: true)
            };
        }

        private AppUserDto MapToDto(AppUser user)
        {           
            return new AppUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Email = user.Email!
            };
        }

        public async Task<Result<List<AppUserDto>>?> GetAllUsers()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();

            if (users is not null)
            {
                return users.Select(u => MapToDto(u)).ToList();
            }

            return Error.NotFound("Users Not Found !");

        }



    }


}
