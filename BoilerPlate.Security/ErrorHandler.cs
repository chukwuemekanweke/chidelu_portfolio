using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.Security
{
    public class ErrorHandler
    {

        public string HandleIdentityErrors(IEnumerable<IdentityError> identityErrors)
        {
            string errorMessage = "";

            foreach (var error in identityErrors)
            {
                errorMessage += $"{error.Description}. ";
            }
            return errorMessage;
        }

    }
}
