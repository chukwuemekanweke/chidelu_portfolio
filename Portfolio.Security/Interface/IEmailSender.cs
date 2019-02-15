using SendGrid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security.Interface
{
  
        public interface IEmailSender
        {
            Task<Response> SendEmailAsync(string email, string subject, string message);
        }
        
}
