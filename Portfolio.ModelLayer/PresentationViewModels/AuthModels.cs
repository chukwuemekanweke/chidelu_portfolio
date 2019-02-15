using BoilerPlate.ModelLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BoilerPlate.ModelLayer.PresentationViewModels
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }


    public class LoginModel
    {
        /// <summary>
        /// Can be email or phonenumber
        /// </summary>

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }

    public class EmailConfirmationModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
    }


    public class EmailConfirmationResponse
    {

        public string Email { get; set; }
        public EmailConfirmation ConfirmationStatus { get; set; }
    }

    public class RenewJWTTokenModel
    {
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public string JWt { get; set; }
    }

}
