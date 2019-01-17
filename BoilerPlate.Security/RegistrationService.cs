﻿using BoilerPlate.ModelLayer.Entity;
using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.ModelLayer.PresentationViewModels;
using BoilerPlate.Security.Interface;
using BoilerPlate.ServiceLayer.EntityServices.Interfaces;
using Microsoft.AspNetCore.Http;
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
    public class RegistrationService: IRegistrationService, ISecurityBaseService
    {
        UserManager<ApplicationUser> UserManager { get; set; }
        RoleManager<IdentityRole> RoleManager { get; set; }
        IUserService UserService { get; set; }
        IEmailSender EmailSender { get; set; }
        public RegistrationService(UserManager<ApplicationUser>userManager, RoleManager<IdentityRole> roleManager, IUserService userService, IEmailSender emailSender)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            UserService = userService;
            EmailSender = emailSender;
        }


        public async Task Register(RegisterModel model, Func<string,string,string> generateConfirmationLink, string PathToEmailFile )
        {
            try
            {
                ErrorHandler errorHandler = new ErrorHandler();
                string email = model.Email.Trim();
                string userName = model.Email.Trim();
                string phoneNumber = model.Phone.Trim();


                ApplicationUser identityUser = new ApplicationUser
                {
                    Email = email,
                    UserName = userName,
                    PhoneNumber = phoneNumber,
                };

                IdentityResult result = await UserManager.CreateAsync(identityUser, model.Password);

                if (!result.Succeeded)
                {
                    string errorMessage = errorHandler.HandleIdentityErrors(result.Errors);
                    throw new Exception(errorMessage);
                }

                //await UserManager.AddToRoleAsync(identityUser, AppRoles.userRole);

                USER user = UserService.CreateUser(email, userName, phoneNumber, identityUser.Id);
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(identityUser);
                string confirmationLink = generateConfirmationLink(code, identityUser.Id);
                                              
                var emailBody = arrangeEmailTemplateForEmailConfirmation(userName, PathToEmailFile,HtmlEncoder.Default.Encode(confirmationLink));

                //SendGrid.Response emailResponse = await emailSender.SendEmailAsync(model.EmailAddress, "Confirm your email",
                //    $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

                Response emailResponse = await EmailSender.SendEmailAsync(email, "Boiler Plate Email Account Confirmation",emailBody);


            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string arrangeEmailTemplateForEmailConfirmation(string username,string pathToEmailFile, string callbackUrl)
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

        public async Task RegisterWithoutEmailConfirmation(RegisterModel model)
        {
            try
            {
                ErrorHandler errorHandler = new ErrorHandler();
                string email = model.Email.Trim();
                string userName = model.Email.Trim();
                string phoneNumber = model.Phone.Trim();


                ApplicationUser identityUser = new ApplicationUser
                {
                    Email = email,
                    UserName = userName,
                    PhoneNumber = phoneNumber,
                    EmailConfirmed = true,
                };

                IdentityResult result = await UserManager.CreateAsync(identityUser, model.Password);

                if (!result.Succeeded)
                {
                    string errorMessage = errorHandler.HandleIdentityErrors(result.Errors);
                    throw new Exception(errorMessage);
                }

                //await UserManager.AddToRoleAsync(identityUser, AppRoles.userRole);

                USER user = UserService.CreateUser(email, userName, phoneNumber, identityUser.Id);
                
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
