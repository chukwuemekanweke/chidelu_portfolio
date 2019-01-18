using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security.Interface
{
    public interface  IEmailConfirmationService
    {
        Task ConfirmEmail(string userid, string token);
    }
}
