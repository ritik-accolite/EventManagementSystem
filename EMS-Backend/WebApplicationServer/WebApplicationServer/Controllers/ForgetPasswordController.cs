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
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Email == email);
            //var Id = User.FindFirstValue("Id");


            ResponseViewModel response = new ResponseViewModel();

            if (user == null)
            {
                response.Status = 404;
                response.Message = "Email Doesn't Exist";
                return response;
            }

            var tokenByte = RandomNumberGenerator.GetBytes(64);
            var emailtoken = Convert.ToBase64String(tokenByte);
            user.ResetPasswordToken = emailtoken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);


            //string path = Path.GetFullPath("C:\\Users\\ajay.k_int1595\\Desktop\\Ems-Project\\EventManagementSystem\\EMS-Backend\\WebApplicationServer\\WebApplicationServer\\HtmlTemplate\\RegisterSuccessfull.html");
            //string htmlString = System.IO.File.ReadAllText(path);
            //htmlString = htmlString.Replace("{{title}}", "Registration Confirmation");
            //htmlString = htmlString.Replace("{{Username}}", user.Email);
            //htmlString = htmlString.Replace("{{ConfirmationLink}}", Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }));
            //htmlString = htmlString.Replace("{{url}}", "https://localhost:5299/api/Account/login");
            bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(user.Email, "Reset Password Mail", emailtoken);

            if (!emailSent)
            {
                // Handle email sending failure
                response.Status = 500;
                response.Message = "Failed to send registration email";
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
