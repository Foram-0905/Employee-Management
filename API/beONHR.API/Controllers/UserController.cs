using Azure;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.ForgotPassword;
using beONHR.Entities.User;
using beONHR.Infrastructure.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly RoleManager<AspNetRoles> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _user;

        public UserController(IUserService userservice,UserManager<AspNetUsers> userManager, RoleManager<AspNetRoles> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _user = userservice;

        }

        
      
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(Register register)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _user.RegisterUser(register);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] Login login)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                //OneOf<LoginResponse, ClientResponse> userOneOf = new OneOf<LoginResponse, ClientResponse>();

                objresp = await _user.UserLogin(login);

                return Ok(objresp);

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenResponse tokenResponse)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                //OneOf<LoginResponse, ClientResponse> userOneOf = new OneOf<LoginResponse, ClientResponse>();

                objresp = await _user.RefreshToken(tokenResponse);

                return Ok(objresp);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("ForgotPassword/{Mail}")]
        public async Task<ClientResponse> ForgotPassword(string Mail)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
               
                var user = await _userManager.FindByEmailAsync(Mail);
                if (user != null)
                {
                    forgotmail forgotmail = new forgotmail()
                    { 
                        Email = user.Email,
                        FirstName = user.UserName
                    };
                    objresp = await _user.SendForgotPasswordEmail(forgotmail);

                }
                else
                {
                    
                    objresp.Message = "User Not Found";
                    objresp.HttpResponse = null;
                    objresp.IsSuccess = false;
                    objresp.StatusCode = HttpStatusCode.OK;
                }
                

                //sendmail.EmailSent = true;
                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword reset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                reset.Token = reset.Token.Replace(' ', '+');
                objresp = await _user.ResetPasswordAsync(reset);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
