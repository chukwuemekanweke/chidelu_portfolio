using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.Security.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security
{
    public class EmailConfirmationService : ISecurityBaseService, IEmailConfirmationService
    {
        UserManager<ApplicationUser> UserManager { get; set; }
        public EmailConfirmationService(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task<string> ConfirmEmail(string userid, string token)
        {
            ErrorHandler errorHandler = new ErrorHandler();
            string redirectUrl = "";
            ApplicationUser user = await UserManager.FindByIdAsync(userid);
            if (user == null)
            {
                throw new InvalidOperationException("Oops Email Link Is Invalid");
            }
            IdentityResult result = await UserManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Oops Email Link Is Invalid");
            }
            return user.Email;

        }
    }
}
