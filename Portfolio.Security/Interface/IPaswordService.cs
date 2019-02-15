using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security.Interface
{
    public interface IPasswordService
    {
        Task SendPasswordResetEmailLinkAsync(string email, Func<string, string, string> generateResetLinkDelegate, string emailPath);
        Task SendPasswordResetEmailTokenAsync(string email,  string emailPath);
        Task ResetPasswordAsync(string userId, string resetToken, string newPassword);

    }
}
