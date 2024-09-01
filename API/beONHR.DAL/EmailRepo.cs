using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Email;
using beONHR.Entities.EmailTemplate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities.User;
using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using beONHR.Entities.Context;

namespace beONHR.DAL
{
    public interface IEmailRepo
    {
        Task<ClientResponse> GenerateForgotPasswordTokenAsync(forgotmail user);
        Task<ClientResponse> SendEmailApplyLeave(EmailMessage user);
        Task<ClientResponse> SendEmailActionOnLeave(EmailMessage user);

    }
    public class EmailRepo : IEmailRepo
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly RoleManager<AspNetRoles> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly EmailConfiguration _emailConf;
        private readonly MainContext _context;
        public EmailRepo(UserManager<AspNetUsers> userManager, RoleManager<AspNetRoles> roleManager, IConfiguration configuration, IOptions<EmailConfiguration> email,MainContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailConf = email.Value;
            _context = context;
        }

        public async Task<ClientResponse> GenerateForgotPasswordTokenAsync(forgotmail user)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(await _userManager.FindByEmailAsync(user.Email));
                if (!string.IsNullOrEmpty(token))
                {
                    response = await SendForgotPasswordEmail(user, token);
                }
                return response;

            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task<ClientResponse> SendForgotPasswordEmail(forgotmail user, string token)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                // Retrieve employee details
                var employee = _context.Employees.FirstOrDefault(e => e.Email == user.Email);
                if (employee == null)
                {
                    throw new Exception("Employee not found");
                }

                // Construct full name
                string fullName = $"{employee.FirstName} {employee.MiddleName} {employee.LastName}";

                // Fetch domain and link from configuration
                string appDomain = _configuration.GetSection("Application:AppDomain").Value;
                string forgetlink = _configuration.GetSection("Application:ForgotPassword").Value;

                // Create email message options
                EmailMessage options = new EmailMessage
                {
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", fullName),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + forgetlink, token))
                }
                };

                // Send email
                response = await SendEmailForForgotPassword(options);

                return response;
            }
            catch (Exception ex)
            {
                // Log the exception (if you have logging in place)
                throw;
            }
        }

        //private async Task<ClientResponse> SendForgotPasswordEmail(forgotmail user, string token)
        //{
        //    ClientResponse response = new ClientResponse();
        //    try
        //    {
        //        string appDomain = _configuration.GetSection("Application:AppDomain").Value;
        //        string forgetlink = _configuration.GetSection("Application:ForgotPassword").Value;


        //        EmailMessage options = new EmailMessage
        //        {
        //            ToEmails = new List<string>() { user.Email },
        //            PlaceHolders = new List<KeyValuePair<string, string>>()
        //        {
        //            new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
        //            new KeyValuePair<string, string>("{{Link}}",
        //                string.Format(appDomain + forgetlink, token))
        //        }
        //        };

        //        response = await SendEmailForForgotPassword(options);

        //        return response;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        public async Task<ClientResponse> SendEmailForForgotPassword(EmailMessage emailMessage)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                emailMessage.Subject = UpdatePlaceHolders("Hello {{UserName}}, reset your password.", emailMessage.PlaceHolders);

                EmailTemplate emailTemplate = new EmailTemplate();
                var text = emailTemplate.forgetpassword;
                emailMessage.Body = UpdatePlaceHolders(text, emailMessage.PlaceHolders);

                response = await SendEmail(emailMessage);

                return response;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<ClientResponse> SendEmailApplyLeave(EmailMessage emailMessage)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                emailMessage.Subject = "Employee Request for leave.";

                EmailTemplate emailTemplate = new EmailTemplate();
                var text = emailTemplate.applyleave;
                emailMessage.Body = UpdatePlaceHolders(text, emailMessage.PlaceHolders);

                response = await SendEmail(emailMessage);

                return response;

            }
            catch (Exception)
            {

                throw;
            }

        }   public async Task<ClientResponse> SendEmailActionOnLeave(EmailMessage emailMessage)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                emailMessage.Subject = "Replay of Request for leave.";

                EmailTemplate emailTemplate = new EmailTemplate();
                var text = emailTemplate.actionOnleave;
                emailMessage.Body = UpdatePlaceHolders(text, emailMessage.PlaceHolders);

                response = await SendEmail(emailMessage);

                return response;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<ClientResponse> SendEmail(EmailMessage emailMessage)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                MailMessage mail = new MailMessage
                {
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body,
                    From = new MailAddress(_emailConf.SenderAddress, _emailConf.SenderDisplayName),
                    IsBodyHtml = _emailConf.IsBodyHTML
                };
                foreach (var toEmail in emailMessage.ToEmails)
                {
                    mail.To.Add(toEmail);
                }
                NetworkCredential networkCredential = new NetworkCredential(_emailConf.UserName, _emailConf.Password);

                SmtpClient smtp = new SmtpClient()
                {
                    Host = _emailConf.Host,
                    Port = _emailConf.Port,
                    EnableSsl = _emailConf.EnableSSL,
                    UseDefaultCredentials = false,
                    Credentials = networkCredential
                };
                mail.BodyEncoding = Encoding.Default;

                await smtp.SendMailAsync(mail);

                response.Message = "Mail send Sucessfully";
                response.HttpResponse = null;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}
