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
}
