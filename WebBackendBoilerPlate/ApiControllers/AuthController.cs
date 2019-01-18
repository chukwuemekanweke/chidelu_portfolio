using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.ModelLayer.PresentationViewModels;
using BoilerPlate.Security.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBackendBoilerPlate.Models;

namespace WebBackendBoilerPlate.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        IRegistrationService RegistrationService { get; set; }
        ILoginService LoginService { get; set; }
        IHostingEnvironment HostingEnvironment;
        UserManager<ApplicationUser> UserManager { get; set; }
        IEmailConfirmationService EmailConfirmationService { get; set; }

        public AuthController(IRegistrationService registrationService,ILoginService loginService,UserManager<ApplicationUser> userManager, IEmailConfirmationService emailConfirmationService, IHostingEnvironment hostingEnvironment)
        {
            RegistrationService = registrationService;
            HostingEnvironment = hostingEnvironment;
            LoginService = loginService;
            UserManager = userManager;
            EmailConfirmationService = emailConfirmationService;
        }



        [HttpGet("[action]")]
        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            try
            {

                await EmailConfirmationService.ConfirmEmail(userid, token);

                string  redirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/EmailConfirmationSuccessful";              

                return Redirect(redirectUrl);
            }
            catch(Exception ex)
            {
                return Redirect($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/Error");
            }
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterModel model)
        {
            try
            {
                var pathToFile = HostingEnvironment.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "Email1.html";
                Func<string,string,string> confirmationLinkGenerator = (code,userId)=>Url.Action("ConfirmEmail",
                 "Auth", new
                 {
                     userid = userId,
                     token = code
                 },
                  protocol: HttpContext.Request.Scheme
                  );

                RegistrationService.Register(model, confirmationLinkGenerator,pathToFile);
                return Ok(null, "User Registered Successfully", ResponseStatus.OK);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(null, ex.Message, ResponseStatus.APP_ERROR);
            }
            catch (Exception ex)
            {
                return BadRequest(null, "Oops Something Went Wrong", ResponseStatus.FATAL_ERROR);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                var pathToFile = HostingEnvironment.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "Email1.html";
                Func<string, string, string> confirmationLinkGenerator = (code, userId) => Url.Action("ConfirmEmail",
                   "Auth", new
                   {
                       userid = userId,
                       token = code
                   },
                    protocol: HttpContext.Request.Scheme
                    );

                var jwtPayload = LoginService.Login(model, confirmationLinkGenerator, pathToFile);
                return Ok(jwtPayload, "Jwt Payload For Successfull Login Attempt", ResponseStatus.OK);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(null, ex.Message, ResponseStatus.APP_ERROR);
            }
            catch (Exception ex)
            {
                return BadRequest(null, "Oops Something Went Wrong", ResponseStatus.FATAL_ERROR);
            }
        }





    }
}