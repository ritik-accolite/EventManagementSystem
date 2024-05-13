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

        private readonly IGetAllPerson _getAllPerson;
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _context;

        public ForgetPasswordController(ApplicationDbContext context, UserManager<Person> userManager, SignInManager<Person> signInManager, IGetAllPerson getAllPerson, ISendRegisterSuccessMailService sendRegisterSuccessMailService)
        {
            _getAllPerson = getAllPerson;
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }



        [HttpPost("send-reset-password-token-email/{email}")]
        public async Task<ResponseViewModel> SendMail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == email);
            //var Id = User.FindFirstValue("Id");

            
            ResponseViewModel response = new ResponseViewModel();

            if (user == null)
            {
                response.Status = 404;
                response.Message = "Email Doesn't Exist";
                return response;
            }

            var tokenByte = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenByte);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(30);


            string path = Path.GetFullPath("C:\\Users\\ajay.k_int1595\\Desktop\\Ems-Project\\EventManagementSystem\\EMS-Backend\\WebApplicationServer\\WebApplicationServer\\HtmlTemplate\\ResetPasswordToken.html");
            string htmlString = System.IO.File.ReadAllText(path);
            htmlString = htmlString.Replace("{{emailtoken}}", emailToken);
            htmlString = htmlString.Replace("{{firstname}}", user.FirstName);
            htmlString = htmlString.Replace("{{lastname}}", user.LastName);
            htmlString = htmlString.Replace("{{title}}", "Reset Password");
            htmlString = htmlString.Replace("{{Username}}", user.Email);
            //htmlString = htmlString.Replace("{{firstName}}", user.FirstName);
            //htmlString = htmlString.Replace("{{lastName}}", user.LastName);

            bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(user.Email, "Reset Password Mail", htmlString);

            if (!emailSent)
            {
                // Handle email sending failure
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
            if(tokenCode!= resetPassword.EmailToken || emailTokenExpiry < DateTime.Now)
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
