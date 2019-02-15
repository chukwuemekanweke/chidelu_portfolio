using BoilerPlate.DataLayer.Interface;
using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.Security.Interface;
using BoilerPlate.Security.Models;
using BoilerPlate.ServiceLayer.EntityServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.Security
{
    public class SecurityServicefactory : ISecurityServiceFactory
    {

        UserManager<ApplicationUser> UserManager { get; set; }
        RoleManager<IdentityRole> RoleManager { get; set; }
        IEmailSender EmailSender { get; set; }
        IUserService UserService { get; set; }
        JwtConfiguration JwtConfiguration { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public SecurityServicefactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JwtConfiguration jwtConfiguration,
                                      IUserService userService, IEmailSender emailSender, IUnitOfWork unitOfWork)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            UserService = userService;
            EmailSender = emailSender;
            JwtConfiguration = jwtConfiguration;
            UnitOfWork = unitOfWork;
        }


        public ISecurityBaseService GetService(Type type)
        {
            if (type == typeof(IRegistrationService))
                return new RegistrationService(UserManager, RoleManager, UserService, EmailSender, UnitOfWork);
            else if (type == typeof(IRefreshTokenService))
                return new RefreshTokenService(this);
            else if (type == typeof(IJWTService))
                return new JWTService(this,JwtConfiguration);
            else if (type == typeof(IIdentityUserService))
                return new IdentityUserService(UserManager, RoleManager, EmailSender);
            else if (type == typeof(ILoginService))
                return new LoginService(UserManager, RoleManager,this, UserService, EmailSender);           
            else
                throw new InvalidOperationException("Type Not Supported");

        }
    }
}
