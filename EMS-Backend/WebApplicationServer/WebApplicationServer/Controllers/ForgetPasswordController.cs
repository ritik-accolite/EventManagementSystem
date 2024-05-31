using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ForgetPasswordController : ControllerBase
    {
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly ApplicationDbContext _context;

        public ForgetPasswordController(ApplicationDbContext context, ISendRegisterSuccessMailService sendRegisterSuccessMailService)
        {
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _context = context;
        }

        [HttpPost("send-reset-password-token-email/{email}")]
        public async Task<ResponseViewModel> SendMail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == email);
            ResponseViewModel response = new ResponseViewModel();

            if (user == null)
            {
                response.Status = 404;
                response.Message = "Email Doesn't Exist";
                return response;
            }

            var tokenByte = RandomNumberGenerator.GetInt32(100000,1000000);
            var emailToken = Convert.ToString(tokenByte);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(30);

            var firstName = user.FirstName;
            var lastName = user.LastName;
            var userName = user.Email;


            bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(user.Email, "Reset Password Mail", $"Dear {firstName}{lastName}, We have received a request to change password for {userName} Account. Please use the following Token to set new password. The Token is only valid for 30 minutes. Your token is : {emailToken}. If you have not initiated this request, please contact us on 1800329432 immediately.");

            if (!emailSent)
            {
                response.Status = 500;
                response.Message = "Failed to send reset password Token";
                return response;
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            response.Status = 200;
            response.Message = "Email Sent";
            return response;

        }

        [HttpPost("forget-password")]
        public async Task<ResponseViewModel> ResetPassword(ResetPassword resetPassword)
        {
            ResponseViewModel response = new ResponseViewModel();

            var newToken = resetPassword.EmailToken.Replace(" ", "+");
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPassword.Email);


            if (user == null)
            {
                response.Status = 404;
                response.Message = "Email Doesn't Exist";
                return response;
            }

            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiry;
            if (tokenCode != resetPassword.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                response.Status = 400;
                response.Message = "Invalid Reset Link";
                return response;
            }

            var passwordHasher = new PasswordHasher<Person>();
            user.PasswordHash = passwordHasher.HashPassword(user, resetPassword.NewPassword);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            response.Status = 200;
            response.Message = "Password Reset Succesfully";
            return response;
        }
    }
}
