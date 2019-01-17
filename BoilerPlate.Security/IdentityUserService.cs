using BoilerPlate.ModelLayer.Identity;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security.Interface
{
    public class IdentityUserService
    {
        UserManager<ApplicationUser> UserManager { get; set; }
        RoleManager<IdentityRole> RoleManager { get; set; }
        IEmailSender EmailSender { get; set; }
        public IdentityUserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            EmailSender = emailSender;
        }


        public async Task<ApplicationUser> GetApplicationUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            ApplicationUser applicationUser = await UserManager.GetUserAsync(claimsPrincipal);
            return applicationUser;
        }


        public async Task<ApplicationUser> GetApplicationUserAsync(string identityUserId)
        {
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(identityUserId);
            return applicationUser;
        }

        public async Task ResendEmailConfirmationAsync(string emailAddress, Func<string,string,string> generateConfirmationLink, string PathToEmailFile)
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(emailAddress);
            if (user != null && !user.EmailConfirmed)
            {
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user);

                string confirmationLink = generateConfirmationLink(code, user.Id);
                string body = arrangeEmailTemplateForEmailConfirmation(user.UserName, PathToEmailFile, confirmationLink);
                SendGrid.Response emailResponse = await EmailSender.SendEmailAsync(emailAddress, "Confirm your email",body);

            }
        }


        private string arrangeEmailTemplateForEmailConfirmation(string username, string pathToEmailFile, string callbackUrl)
        {

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToEmailFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = builder.HtmlBody;

            messageBody = messageBody.Replace("{{0}}", username);
            messageBody = messageBody.Replace("{{1}}", callbackUrl);


            return messageBody;

        }

    }
}
