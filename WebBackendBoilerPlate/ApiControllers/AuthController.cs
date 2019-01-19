using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BoilerPlate.ModelLayer.Enum;
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
        IPasswordService PasswordService { get; set; }
        IRefreshTokenService RefreshTokenService { get; set; }
        public AuthController(IRegistrationService registrationService,ILoginService loginService,UserManager<ApplicationUser> userManager, IEmailConfirmationService emailConfirmationService,
                              IPasswordService passwordService,
                              IRefreshTokenService refreshtokenService,IHostingEnvironment hostingEnvironment)
        {
            RegistrationService = registrationService;
            HostingEnvironment = hostingEnvironment;
            LoginService = loginService;
            UserManager = userManager;
            EmailConfirmationService = emailConfirmationService;
            PasswordService = passwordService;
            RefreshTokenService = refreshtokenService;
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

        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> PasswordResetLink(string email)
        {
            try
            {
                var pathToFile = HostingEnvironment.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "ResetPassword.html";


                Func<string, string, string> confirmationLinkGenerator = (code, userId) => Url.Action("ResetPassword",
                   "Auth", new
                   {
                       userid = userId,
                       token = code
                   },
                    protocol: HttpContext.Request.Scheme
                    );

                await PasswordService.SendPasswordResetEmailLinkAsync(email, confirmationLinkGenerator, pathToFile);
                return Ok(null, $"Password reset Link Sent To {email}", ResponseStatus.OK);
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


        [HttpPost("[action]")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationModel model)
        {
            try
            {

                var email =  await EmailConfirmationService.ConfirmEmail(model.UserId, model.Token);

                string redirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/EmailConfirmationSuccessful";
                EmailConfirmationResponse emailConfirmationResponse = new EmailConfirmationResponse
                {
                    Email = email,
                    ConfirmationStatus = EmailConfirmation.CONFIRMED
                };

                return Ok(emailConfirmationResponse, "Email Confirmed Successfully", ResponseStatus.OK);
            }
            catch (InvalidOperationException ex)
            {
                EmailConfirmationResponse emailConfirmationResponse = new EmailConfirmationResponse
                {
                    ConfirmationStatus = EmailConfirmation.UNCONFIRMED
                };

                var user = await UserManager.FindByIdAsync(model.UserId);
                if (user != null)
                    emailConfirmationResponse.Email = user.Email;

                return BadRequest(emailConfirmationResponse, ex.Message, ResponseStatus.APP_ERROR);
            }
            catch (Exception ex)
            {
                EmailConfirmationResponse emailConfirmationResponse = new EmailConfirmationResponse
                {
                    ConfirmationStatus = EmailConfirmation.UNCONFIRMED
                };

                var user = await UserManager.FindByIdAsync(model.UserId);
                if (user != null)
                    emailConfirmationResponse.Email = user.Email;




                return BadRequest(emailConfirmationResponse, "Oops Something Went Wrong", ResponseStatus.FATAL_ERROR);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                var pathToFile = HostingEnvironment.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "ConfirmEmail.html";
                Func<string,string,string> confirmationLinkGenerator = (code,userId)=>Url.Action("ConfirmEmail",
                 "Auth", new
                 {
                     userid = userId,
                     token = code
                 },
                  protocol: HttpContext.Request.Scheme
                  );

                await RegistrationService.Register(model, confirmationLinkGenerator,pathToFile);
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




        [HttpPost("[action]")]
        public async Task< IActionResult> RegisterWithoutEmailConfirmation(RegisterModel model)
        {
            try
            {
                

                await RegistrationService.RegisterWithoutEmailConfirmation(model);
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var pathToFile = HostingEnvironment.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "ConfirmEmail.html";
                Func<string, string, string> confirmationLinkGenerator = (code, userId) => Url.Action("ConfirmEmail",
                   "Auth", new
                   {
                       userid = userId,
                       token = code
                   },
                    protocol: HttpContext.Request.Scheme
                    );

                var jwtPayload = await LoginService.Login(model, confirmationLinkGenerator, pathToFile);
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

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginWithoutEmailConfirmation(LoginModel model)
        {
            try
            {
               

                var jwtPayload = await LoginService.LoginWithoutEmailConfirmation(model);
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


        [HttpPost("[action]")]
        public async Task<IActionResult> RenewJWT(RenewJWTTokenModel model)
        {
            try
            {


                var payload =  await RefreshTokenService.Refresh(model.JWt, model.RefreshToken);
                return Ok(payload, "JWt Payload Generated SUccessfully", ResponseStatus.OK);
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