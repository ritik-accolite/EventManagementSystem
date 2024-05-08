using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                    Password = person.Password
                };

                result = await _userManager.CreateAsync(user, person.Password);
                if (!result.Succeeded)
                {
                    response.Status = 403;
                    response.Message = "Unauthorised Access";
                    return response;
                }

                string path = Path.GetFullPath("C:\\Users\\ajay.k_int1595\\Desktop\\Ems-Project\\EventManagementSystem\\EMS-Backend\\WebApplicationServer\\WebApplicationServer\\HtmlTemplate\\RegisterSuccessfull.html");
                string htmlString = System.IO.File.ReadAllText(path);
                htmlString = htmlString.Replace("{{title}}", "Registration Confirmation");
                htmlString = htmlString.Replace("{{Username}}", user.Email);
                htmlString = htmlString.Replace("{{url}}", "https://localhost:5299/api/Account/login");
                bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(user.Email, "Account Created Successfully", htmlString);


                if (!emailSent)
                {
                    // Handle email sending failure
                    response.Status = 500;
                    response.Message = "Failed to send registration email";
                    return response;
                }
                message = "Registered Successfully";
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
                if (person != null && !person.EmailConfirmed)
                {
                    person.EmailConfirmed = true;
                }

                var result = await _signInManager.PasswordSignInAsync(person, login.Password, true, false);

                if (!result.Succeeded)
                {                    
                    response.Status = 403;
                    response.Message = "Unauthorised Access";
                    return response;
                }

               /* string path = Path.GetFullPath("C:\\Users\\ajay.k_int1595\\Desktop\\Ems-Project\\EventManagementSystem\\EMS-Backend\\WebApplicationServer\\WebApplicationServer\\HtmlTemplate\\LoginSuccessfull.html");
                string htmlString = System.IO.File.ReadAllText(path);
                htmlString = htmlString.Replace("{{title}}", "Login Successfull");
                htmlString = htmlString.Replace("{{Username}}",login.Email);
                bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(login.Email, "Successful log on to EventHub", htmlString);

                if (!emailSent)
                {
                    // Handle email sending failure
                    response.Status = 500;
                    response.Message = "Failed to send Login email";
                    return response;
                }*/
                message = "Login Successfully";
                // jwt logic for Role Based
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



        [HttpPost("ChangeEmail")]
        public async Task<ResponseViewModel> ChangeEmail(ChangeEmailViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var email = await _userManager.GetEmailAsync(user);
                if (model.EmailConfirmed != email)
                {
                    user.Email = model.EmailConfirmed;
                    var setUserNameResult = await _userManager.SetUserNameAsync(user, user.Email);
                    if (!setUserNameResult.Succeeded)
                    {
                        response.Status = 400;
                        response.Message = "Email Not Updated";
                        return response;
                    }
                    await _signInManager.RefreshSignInAsync(user);
                    response.Status = 200;
                    response.Message = "Email changed Successfully";


                    string path = Path.GetFullPath("C:\\Users\\ajay.k_int1595\\Desktop\\Ems-Project\\EventManagementSystem\\EMS-Backend\\WebApplicationServer\\WebApplicationServer\\HtmlTemplate\\ChangeEmail.html");
                    string htmlString = System.IO.File.ReadAllText(path);
                    //htmlString = htmlString.Replace("{{title}}", "Email Changed Successfull");
                    //htmlString = htmlString.Replace("{{Username}}", model.Email);
                    bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(model.Email, "Email Updated Successfully", htmlString);

                    if (!emailSent)
                    {
                        // Handle email sending failure
                        response.Status = 500;
                        response.Message = "Failed to send  Update Email mail";
                        return response;
                    }
                    return response;
                }
            }
            response.Status = 403;
            response.Message = "Something went wrong";
            return response;
        }

        [HttpPost("ChangePassword")]
        public async Task<ResponseViewModel> ChangePassword(ChangePasswordViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    string path = Path.GetFullPath("C:\\Users\\ajay.k_int1595\\Desktop\\Ems-Project\\EventManagementSystem\\EMS-Backend\\WebApplicationServer\\WebApplicationServer\\HtmlTemplate\\ChangePassword.html");
                    string htmlString = System.IO.File.ReadAllText(path);
                    bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(user.Email, "Password Updated Successfully", htmlString);

                    if (!emailSent)
                    {
                        // Handle email sending failure
                        response.Status = 500;
                        response.Message = "Failed to send Mail";
                        return response;
                    }
                    await _signInManager.RefreshSignInAsync(user);
                    response.Status = 200;
                    response.Message = "Password changed Successfully";
                    return response;
                }
                else
                {
                    response.Status = 400;
                    response.Message = "Password Not Updated";
                    return response;
                }
            }
            response.Status = 403;
            response.Message = "Something went wrong";
            return response;
        }


    }
}
