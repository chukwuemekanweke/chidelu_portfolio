using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.Security.Interface;
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


        public SecurityServicefactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }


        public ISecurityBaseService GetService(Type type)
        {
            if (type == typeof(IRegistrationService))
                return new RegistrationService(UserManager, RoleManager);//todo: Add LitecoinMerchantWallet
            else if (type == typeof(IRefreshTokenService))
                return new RefreshTokenService();
            else if (type == typeof(IJWTService))
                return new RefreshTokenService();
            else
                throw new InvalidOperationException("Type Not Supported");

        }
    }
}
