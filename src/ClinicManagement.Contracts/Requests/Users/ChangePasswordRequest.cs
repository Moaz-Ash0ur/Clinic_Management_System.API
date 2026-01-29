using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Contracts.Requests.Users
{
    public sealed record ChangePasswordRequest(
     string UserId,
     string CurrentPassword,
     string NewPassword
 );


    public sealed record ConfirmEmailRequest(
    string Email,
    string Token
);

    public sealed record ForgotPasswordRequest(
    string Email
);

    public sealed record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword
);


}
