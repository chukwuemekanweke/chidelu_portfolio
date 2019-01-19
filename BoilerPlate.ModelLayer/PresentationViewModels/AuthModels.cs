using BoilerPlate.ModelLayer.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.ModelLayer.PresentationViewModels
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }


    public class LoginModel
    {
        /// <summary>
        /// Can be email or phonenumber
        /// </summary>
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class EmailConfirmationModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }


    public class EmailConfirmationResponse
    {
        public string Email { get; set; }
        public EmailConfirmation ConfirmationStatus { get; set; }
    }

    public class RenewJWTTokenModel
    {
        public string RefreshToken { get; set; }
        public string JWt { get; set; }
    }

}
