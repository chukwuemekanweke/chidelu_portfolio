using BoilerPlate.ModelLayer.PresentationViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security.Interface
{
    public interface IRegistrationService
    {
        Task Register(RegisterModel model, Func<string, string, string> generateConfirmationLink, string PathToEmailFile);
        Task RegisterWithoutEmailConfirmation(RegisterModel model);
    }
}
