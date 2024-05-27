using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Tls;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IGetAllPerson _getAllPerson;
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager, IGetAllPerson getAllPerson, ISendRegisterSuccessMailService sendRegisterSuccessMailService)
        {
            _getAllPerson = getAllPerson;
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ResponseViewModel> RegisterPerson(RegisterViewModel person)
        {
            string message = "";
            ResponseViewModel response = new ResponseViewModel();
            IdentityResult result = new();

            var userExist = await _userManager.FindByEmailAsync(person.Email);
            if (userExist != null)
            {
                response.Status = 403;
                response.Message = "User Already Exist";
                return response;
            }
            try
            {
                var user = new Person()
                {
                    Email = person.Email,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Role = person.Role,
                    PhoneNumber = person.PhoneNumber,
                    UserName = person.Email,
                    //Password = person.Password
                };

                result = await _userManager.CreateAsync(user, person.Password);
                if (!result.Succeeded)
                {
                    response.Status = 403;
                    response.Message = "Failed To Create Account";
                    return response;
                }
                message = "Registered Successfully";
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email });

                string email = user.Email;
                string space = ".                    ";

                string htmlString = $"Hello {email}, Your Account has been Successfully Created, Please Confirm Your Account By Copying Below Link in the browser : https://eventhubfusion.azurewebsites.net{confirmationLink}{space}BestRegards, EventHub Team";
                bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(user.Email, "Account Created Successfully", htmlString);

                if (!emailSent)
                {
                    // Handle email sending failure
                    response.Status = 500;
                    response.Message = "Failed to send registration email";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = ex.Message;
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully Registered";
            return response;
        }

        [HttpPost("login")]
        public async Task<AuthenticatedLoginResponseViewModel> LoginPerson(LoginViewModel login)
        {
            string message = "";
            string token = string.Empty;
            AuthenticatedLoginResponseViewModel response = new AuthenticatedLoginResponseViewModel();

            try
            {
                Person person = await _userManager.FindByEmailAsync(login.Email);

                if (person == null || !person.EmailConfirmed)
                {
                    
                    response.Status = 403;
                    response.Message = "User Not Found or Email is not Confirmed";
                    return response;
                }


                if (person.IsBlocked)
                {
                    response.Status = 403;
                    response.Message = "You cannot login.You have been blocked by Admin";
                    return response;

                }

                var result = await _signInManager.PasswordSignInAsync(person, login.Password, true, false);

                if (!result.Succeeded)
                {
                    response.Status = 403;
                    response.Message = "Unauthorised Access";
                    return response;
                }

                string email = login.Email;
                bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(login.Email, "Successful log on to EventHub", $"Dear {email},{Environment.NewLine}We are pleased to inform you that your recent login to EventHub was successful. If this was not you, please secure your account immediately.{Environment.NewLine}You can review your account details and recent activities by logging into your account. If you have any questions or encounter any issues, our support team is here to help.{Environment.NewLine}Thank you for being a valued member of our community!{Environment.NewLine}Best Regards,{Environment.NewLine}EventHub Team");

                if (!emailSent)
                {
                    // Handle email sending failure
                    response.Status = 500;
                    response.Message = "Failed to send Login email";
                    return response;
                }
                message = "Login Successfully";

                string role = person.Role;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, person.UserName)
                };
                claims.Add(new Claim(type: "Role", value: person.Role));
                claims.Add(new Claim(type: "Id", value: person.Id));


                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JlVhjeoTKfL8JgQ0Xg2m3BxAP34f5S9tTmN7Gc1A8Zq"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5299",
                    audience: "https://localhost:5299",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = ex.Message;
                return response;
            }
            response.Status = 200;
            response.Message = "Successfully Logged In";
            response.Token = token;
            return response;
        }

        [HttpPost("logout")]
        public async Task<ResponseViewModel> LogoutPerson()
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                await _signInManager.SignOutAsync();
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                response.Status = 200;
                response.Message = "Logged Out Successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = ex.Message;
                return response;
            }
        }


        [HttpGet("ConfirmEmail")]
        public async Task<ResponseViewModel> ConfirmEmail(string token, string email)
        {
            ResponseViewModel response = new ResponseViewModel();

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    response.Status = 200;
                    response.Message = "Email Confirmed Successfully";
                    return response;
                }
            }

            response.Status = 400;
            response.Message = "User Does Not Exist. Something went Wrong. Email Not Sent";
            return response;
        }
    }
}