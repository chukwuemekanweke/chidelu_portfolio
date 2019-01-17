using BoilerPlate.ModelLayer.PresentationViewModels;
using BoilerPlate.Security.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security.Interface
{
    public interface  ILoginService
    {
         Task<Jwt> Login(LoginModel model, Func<string, string, string> generateConfirmationLink, string PathToEmailFile);
        Task<Jwt> LoginWithoutEmailConfirmation(LoginModel model);
    }
}
