using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Email;
using beONHR.Entities.DTO.ForgotPassword;
using beONHR.Entities.User;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



namespace beONHR.DAL
{
    public interface IUserRepo
    {
        Task<ClientResponse> ResetPasswordAsync(ResetPassword data);
        Task<ClientResponse> RegisterUser(Register register);
    }

    public class UserRepo : IUserRepo
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly RoleManager<AspNetRoles> _roleManager;
        private readonly IEmailRepo _emailRepo;
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _configuration;
        private readonly IDataProtector _dataProtector;
        public UserRepo(UserManager<AspNetUsers> userManager,
            RoleManager<AspNetRoles> roleManager,
            IConfiguration configuration, IDataProtectionProvider dataProtectionProvider,
            IEmailRepo emailRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailRepo = emailRepo;
            _dataProtector = dataProtectionProvider.CreateProtector("DataProtectorTokenProvider");

        }

        public async Task<ClientResponse> RegisterUser(Register register)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var userexist = await _userManager.FindByEmailAsync(register.Email);
                if (userexist != null)
                {
                    response.Message = "User Already exists";

                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.Forbidden;

                    return response;
                }

                AspNetUsers user = new()
                {
                    Email = register.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = register.username,
                   
                };
                if (await _roleManager.RoleExistsAsync(register.Role))
                {

                    var result = await _userManager.CreateAsync(user, register.password);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var emailConfirm = await _userManager.ConfirmEmailAsync(user, token);

                    if (!result.Succeeded)
                    {

                        response.Message = "User Create Failed";
                        response.HttpResponse = null;
                        response.IsSuccess = false;
                        response.StatusCode = HttpStatusCode.InternalServerError;

                        return response;
                    }
                    await _userManager.AddToRoleAsync(user, register.Role);
                    var userData = await _userManager.FindByEmailAsync(register.Email);
                    response.Message = "User Create Successfully";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;

                    return response;
                }
                else
                {

                    response.Message = "User role not existes";
                    response.HttpResponse = null;
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;

                    return response;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public async Task<ClientResponse> ResetPasswordAsync(ResetPassword data)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var resetTokenArray = Convert.FromBase64String(data.Token);
                var unprotectedResetTokenArray = _dataProtector.Unprotect(resetTokenArray);
                var reader = new BinaryReader(new MemoryStream(unprotectedResetTokenArray));
                reader.ReadInt64();
                var userId = reader.ReadString();


                var user = await _userManager.FindByIdAsync(userId);

                if (user != null && user.IsDeleted != true)
                {
                    var res = await _userManager.ResetPasswordAsync(user, data.Token, data.NewPassword);

                    if (!res.Succeeded)
                    {
                        foreach (var error in res.Errors)

                            response.Message = error.Description.ToString();
                        response.HttpResponse = null;
                        response.IsSuccess = false;
                        response.StatusCode = HttpStatusCode.NotModified;
                    }
                    else
                    {

                        response.Message = "Password set Sucessfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "No User Found";
                    response.HttpResponse = null;
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.Unauthorized;
                }


                return response;

            }
            catch (Exception)
            {

                throw;
            }

        }
    }

}
