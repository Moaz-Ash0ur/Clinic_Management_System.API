using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Identity
{  
    public class RefreshToken : AuditableEntity
    {
        public string? Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsActive => RevokedAt == null && !IsExpired;
        public string UserId { get; set; }


        private RefreshToken()
        { }

        private RefreshToken(Guid id, string? token, string userId, DateTime expiresOn) : base(id)
        {
            Token = token;
            ExpiresOn = expiresOn;
            UserId = userId;
        }

        public static Result<RefreshToken> Create(Guid id, string? token, string? userId, DateTime expiresOnUtc)
        {
            if (id == Guid.Empty)
            {
                return RefreshTokenErrors.IdRequired;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return RefreshTokenErrors.TokenRequired;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RefreshTokenErrors.UserIdRequired;
            }

            if (expiresOnUtc <= DateTimeOffset.Now)
            {
                return RefreshTokenErrors.ExpiryInvalid;
            }

            return new RefreshToken(id, token, userId, expiresOnUtc);
        }





    }


    public static class RefreshTokenErrors
    {
        public static readonly Error IdRequired =
            Error.Validation("RefreshToken_Id_Required", "Refresh token ID is required.");

        public static readonly Error TokenRequired =
            Error.Validation("RefreshToken_Token_Required", "Token value is required.");

        public static readonly Error UserIdRequired =
            Error.Validation("RefreshToken_UserId_Required", "User ID is required.");

        public static readonly Error ExpiryInvalid =
            Error.Validation("RefreshToken_Expiry_Invalid", "Expiry must be in the future.");
    }


}
