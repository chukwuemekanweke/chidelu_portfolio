using BoilerPlate.ModelLayer.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security.Interface
{
    public interface IIdentityUserService
    {

        Task<ApplicationUser> GetApplicationUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<ApplicationUser> GetApplicationUserAsync(string identityUserId);
        Task ResendEmailConfirmationAsync(string emailAddress, Func<string, string, string> generateConfirmationLink, string PathToEmailFile);

    }
}
