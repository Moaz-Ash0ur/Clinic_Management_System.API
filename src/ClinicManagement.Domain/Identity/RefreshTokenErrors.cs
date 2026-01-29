using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Identity
{
    public static class RefreshTokenErrors
    {
        public static readonly Error NotFound =
           Error.Validation("RefreshToken_Not_Found", "Refresh Token Not Found"); 

        public static readonly Error AlreadyRevoked =
            Error.Conflict("RefreshToken_Already_Revoked","Refresh token has already been revoked.");

        public static readonly Error TokenExpired =
            Error.Validation("RefreshToken_Expired","Refresh token has expired.");

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
