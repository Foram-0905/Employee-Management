using Azure;
using Azure.Core;
using beONHR.DAL;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.ForgotPassword;
using beONHR.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using OneOf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace beONHR.Infrastructure.User
{
    public interface IUserService
    {
        Task<ClientResponse> RegisterUser(Register register);
        Task<ClientResponse> UserLogin(Login login);
        Task<ClientResponse> RefreshToken(TokenResponse newAccessToken);
        Task<ClientResponse> SendForgotPasswordEmail(forgotmail user);
        Task<ClientResponse> ResetPasswordAsync(ResetPassword data);

    }
    public class UserService : IUserService
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly RoleManager<AspNetRoles> _roleManager;
        private readonly IEmailRepo _emailRepo;
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<AspNetUsers> userManager,
            RoleManager<AspNetRoles> roleManager,
            IConfiguration configuration,
            IEmailRepo emailRepo, IUserRepo userRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailRepo = emailRepo;
            _userRepo = userRepo;

        }
        public async Task<ClientResponse> RegisterUser(Register register)
        {
            try
            {
                return await _userRepo.RegisterUser(register);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ClientResponse> UserLogin(Login login)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                LoginResponse resUser = new();
                var user = await _userManager.FindByEmailAsync(login.username);
                if (user != null)
                {
                    if (user != null && await _userManager.CheckPasswordAsync(user, login.password))
                    {
                        AspNetRoles role = new AspNetRoles();
                        var auth = new List<Claim>
                        {
                        new Claim(ClaimTypes.Name, login.username),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

                        };
                        var userRole = await _userManager.GetRolesAsync(user);
                        foreach (var roles in userRole)
                        {
                            auth.Add(new Claim(ClaimTypes.Role, roles));

                            resUser.Role=roles;
                            role  = await _roleManager.FindByNameAsync(roles);
                        }


                        var jwtToken = Genrate(auth);


                        resUser.RoleId = role.Id;
                        resUser.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                        resUser.Expiration = jwtToken.ValidTo;
                        resUser.Email = user.Email;
                        resUser.Id = new Guid(user.Id);
                        resUser.PreferedLanguage = user.PreferredLanguage;
                        resUser.SignDate= DateTime.UtcNow;
                        resUser.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.UserName);

                   
                        resUser.PreferedLanguage = user.PreferredLanguage;

                        response.Message = "User Login successfully";
                        response.HttpResponse = resUser;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                        return response;

                    }
                    else
                    {

                        response.Message = "User Login Failed! Incorrect User and Password";
                        response.HttpResponse = null;
                        response.IsSuccess = false;
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        return response;
                    }
                }
                else
                {

                    response.Message = "No User Found";
                    response.HttpResponse = null;
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    return response;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<ClientResponse> RefreshToken(TokenResponse tokenResponse)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                string email = null;
                ClaimsPrincipal? principal = null;

                if (tokenResponse.Expiration == true)
                {
                    // Use the provided username to generate a new token
                    if (!string.IsNullOrEmpty(tokenResponse.email))
                    {
                        email = tokenResponse.email;
                        // Create a minimal ClaimsPrincipal
                        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) });
                        principal = new ClaimsPrincipal(claimsIdentity);
                    }
                    else
                    {
                        response.Message = "Username is required when token is expired.";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    }
                }
                else
                {
                    // Use the existing token to get principal
                    principal = GetPrincipalFromExpiredToken(tokenResponse.token);
                    email = principal.Identity.Name;
                }

                if (principal == null)
                {
                    response.Message = "Invalid access token or refresh token";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    response.Message = "Invalid access token or refresh token";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                if (tokenResponse.Expiration == true)
                {
                    // Generate a new token using the claims from the principal
                    var newAccessToken = Genrate(principal.Claims.ToList());

                    var res = new TokenResponse
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                        Expiration = false,
                        email = email
                    };

                    response.Message = "User Login successfully";
                    response.HttpResponse = res;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                    return response;
                }
                else
                {
                    // Return the existing token response
                    response.Message = "Token is still valid.";
                    response.HttpResponse = tokenResponse;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = "An error occurred while processing your request.";
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var JwtKey = _configuration.GetSection("Jwt:secretKey").Value;
            var Jwtissuer = _configuration.GetSection("Jwt:issuer").Value;
            var JwtValidAudience = _configuration.GetSection("Jwt:ValidateAudience").Value;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey)),
                ValidateLifetime = false, // Don't validate lifetime here
                ValidIssuer = Jwtissuer,
                ValidAudience = JwtValidAudience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is JwtSecurityToken jwtSecurityToken &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal;
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                var usernameClaim = principal?.Identity?.Name;
                if (usernameClaim != null)
                {
                    var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usernameClaim) });
                    return new ClaimsPrincipal(claimsIdentity);
                }
            }
            throw new SecurityTokenException("Invalid token");
        }

        //JWT token Genarate
        JwtSecurityToken Genrate(List<Claim> claim)
        {
            try
            {
                var JwtKey = _configuration.GetSection("Jwt:secretKey").Value;
                var Jwtissuer = _configuration.GetSection("Jwt:issuer").Value;
                var JwtValidAudience = _configuration.GetSection("Jwt:ValidateAudience").Value;
                var expireTime = _configuration.GetSection("Jwt:expiryMinutes").Value;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
                var token = new JwtSecurityToken(
                    issuer: Jwtissuer,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(expireTime)),
                    audience: JwtValidAudience,
                    claims: claim.ToArray(),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                var TokenJwt = new JwtSecurityTokenHandler().WriteToken(token);

                return token;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
     
        public static string GenerateAccessTokenFromRefreshToken(string refreshToken, string secret)
        { 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(15), // Extend expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<ClientResponse> SendForgotPasswordEmail(forgotmail user)
        {
            try
            {
                return await _emailRepo.GenerateForgotPasswordTokenAsync(user);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> ResetPasswordAsync(ResetPassword data)
        {
            try
            {
                return await _userRepo.ResetPasswordAsync(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
