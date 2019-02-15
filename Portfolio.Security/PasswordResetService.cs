using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.Security.Interface;
using BoilerPlate.Security.Models;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using SendGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BoilerPlate.Security
{
    public class PasswordService : IPasswordService, ISecurityBaseService
    {

        UserManager<ApplicationUser> UserManager { get; set; }
        IEmailSender EmailSender { get; set; }
        public PasswordService(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            UserManager = userManager;
            EmailSender = emailSender;
        }


        public async Task ResetPasswordAsync(string userId, string resetToken, string newPassword)
        {

            ErrorHandler errorHandler = new ErrorHandler();
            var applicationUser = await UserManager.FindByIdAsync(userId);
            var identityResult = await UserManager.ResetPasswordAsync(applicationUser, resetToken, newPassword);

            if (!identityResult.Succeeded)
            {
                string errorMessage = errorHandler.HandleIdentityErrors(identityResult.Errors);
                throw new InvalidOperationException(errorMessage);
            }
                        
        }

        public async Task SendPasswordResetEmailLinkAsync(string email, Func<string,string,string> generateResetLinkDelegate,string emailPath)
        {
            var tokenPayload = await GeneratePasswordResetTokenAsync(email, emailPath);
            string confirmationLink = generateResetLinkDelegate(tokenPayload.ResetToken, tokenPayload.UserId);
            var emailBody = arrangeEmailTemplateForEmailConfirmation(tokenPayload.UserName, emailPath, HtmlEncoder.Default.Encode(confirmationLink));
            Response emailResponse = await EmailSender.SendEmailAsync(email, "Password Reset", emailBody);     
            
        }


        public async Task SendPasswordResetEmailTokenAsync(string email,  string emailPath)
        {
            try
            {
                var tokenPayload = await GeneratePasswordResetTokenAsync(email, emailPath);
                var emailBody = arrangeEmailTemplateForEmailConfirmation(tokenPayload.UserName, emailPath, tokenPayload.ResetToken);
                Response emailResponse = await EmailSender.SendEmailAsync(email, "Password Reset", emailBody);
            }
            catch(Exception ex)
            {

            }
        }


        public async Task<PasswordResetTokenPayload> GeneratePasswordResetTokenAsync(string email, string emailPath)
        {
            var user = await UserManager.FindByEmailAsync(email);
            var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            return new PasswordResetTokenPayload {
                UserId = user.Id,
                ResetToken = resetToken,
                UserName = user.UserName
            };

        }

        private string arrangeEmailTemplateForEmailConfirmation(string username, string pathToEmailFile, string payload)
        {

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = File.OpenText(pathToEmailFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = builder.HtmlBody;

            messageBody = messageBody.Replace("{{0}}", username);
            messageBody = messageBody.Replace("{{1}}", payload);


            return messageBody;

        }
    }
}
