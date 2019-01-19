using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.ModelLayer.PresentationViewModels;
using BoilerPlate.Security.Interface;
using BoilerPlate.Security.Models;
using BoilerPlate.ServiceLayer.EntityServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security
{
    public class LoginService : ILoginService, ISecurityBaseService
    {
        UserManager<ApplicationUser> UserManager { get; set; }
        RoleManager<IdentityRole> RoleManager { get; set; }
        ISecurityServiceFactory SecurityServiceFactory { get; set; }
        IUserService UserService { get; set; }
        IEmailSender EmailSender { get; set; }
        public LoginService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ISecurityServiceFactory securityServiceFactory,  IUserService userService, IEmailSender emailSender)
        {
            SecurityServiceFactory = securityServiceFactory;
            UserManager = userManager;
            RoleManager = roleManager;
            UserService = userService;
            EmailSender = emailSender;
        }

        public async Task<Jwt> Login(LoginModel model, Func<string, string, string> generateConfirmationLink, string PathToEmailFile)
        {
            try
            {
                Jwt jwt = null;
                IJWTService jwtService = SecurityServiceFactory.GetService(typeof(IJWTService)) as IJWTService;
                IIdentityUserService identityService = SecurityServiceFactory.GetService(typeof(IIdentityUserService)) as IIdentityUserService;

                ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    throw new InvalidOperationException("Incorrect email or password");
                }

                bool isPasswordCorrect = await UserManager.CheckPasswordAsync(user, model.Password);
                if (!isPasswordCorrect)
                {
                    throw new InvalidOperationException("Incorrect email or password");
                }

                var isEmailConfirmed = await UserManager.IsEmailConfirmedAsync(user);
                if (!isEmailConfirmed)
                {
                    await identityService.ResendEmailConfirmationAsync(user.Email, generateConfirmationLink, PathToEmailFile);
                    throw new InvalidOperationException($"{user.Email} Has Not Been Confirmed. Please Retry Email Confirmation");
                }

                if(model.RememberMe)
                    jwt = await jwtService.GenerateJWtWithRefreshTokenAsync(user);
                else
                    jwt = jwtService.GenerateJwtToken(user);
                return jwt;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Jwt> LoginWithoutEmailConfirmation(LoginModel model)
        {
            Jwt jwt = null;

            IJWTService jwtService = SecurityServiceFactory.GetService(typeof(IJWTService)) as IJWTService;
            ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new InvalidOperationException("Incorrect email or password");
            }

            bool isPasswordCorrect = await UserManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordCorrect)
            {
                throw new InvalidOperationException("Incorrect email or password");
            }

            if (model.RememberMe)
                jwt = await jwtService.GenerateJWtWithRefreshTokenAsync(user);
            else
                jwt = jwtService.GenerateJwtToken(user);
            return jwt;
        }

    }
}
