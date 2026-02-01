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

        public string CreatedByIp { get; private set; }
        public Guid? ReplacedByTokenId { get; private set; }
        public string? RevokedByIp { get; private set; }


        private RefreshToken()
        { }

        private RefreshToken(Guid id, string? token, string userId, DateTime expiresOn,string createdByIp) : base(id)
        {
            Token = token;
            ExpiresOn = expiresOn;
            UserId = userId;
            CreatedByIp = createdByIp;
        }

        public static Result<RefreshToken> Create(Guid id, string? token, string? userId, DateTime expiresOnUtc,string CreatedByIp)
        {
            if (id == Guid.Empty)
            {
                return RefreshTokenErrors.IdRequired;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return RefreshTokenErrors.TokenRequired;
            }

            if (string.IsNullOrWhiteSpace(CreatedByIp))
            {
                return RefreshTokenErrors.IpAddressRequired;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RefreshTokenErrors.UserIdRequired;
            }

            if (expiresOnUtc <= DateTimeOffset.Now)
            {
                return RefreshTokenErrors.ExpiryInvalid;
            }

            return new RefreshToken(id, token, userId, expiresOnUtc, CreatedByIp);
        }


        public Result<Success> Revoke(string? revokedByIp,Guid? replacedByTokenId = null)
        {
            if (RevokedAt != null)
                return RefreshTokenErrors.AlreadyRevoked;

            if (IsExpired)
                return RefreshTokenErrors.TokenExpired;

            RevokedAt = DateTime.UtcNow;

            if (revokedByIp != null)
            RevokedByIp = revokedByIp;

            if (replacedByTokenId != null)
            ReplacedByTokenId = replacedByTokenId;

            return Result.Success;
        }




    }


}
