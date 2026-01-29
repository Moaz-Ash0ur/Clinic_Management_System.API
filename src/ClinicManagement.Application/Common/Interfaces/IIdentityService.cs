using ClinicManagement.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Security.Cryptography;

namespace ClinicManagement.Application.Common.Interfaces;

public interface IIdentityService :
    IAuthService,
    IUserManagementService,
    IPasswordService,
    IEmailConfirmationService
{

}
