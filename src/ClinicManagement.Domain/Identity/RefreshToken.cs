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


        public Result<Success> Revoke()
        {
            if (RevokedAt != null)
                return RefreshTokenErrors.AlreadyRevoked;

            if (IsExpired)
                return RefreshTokenErrors.TokenExpired;

            RevokedAt = DateTime.UtcNow;
            return Result.Success;
        }




    }


}
