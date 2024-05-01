using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationServer.Models;
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
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager, IGetAllPerson getAllPerson)
        {
            _getAllPerson = getAllPerson;
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
        public async Task<ResponseViewModel> LoginPerson(LoginViewModel login)
        {
            string message = "";
            ResponseViewModel response = new ResponseViewModel();

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
                message = "Login Successfully";
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = ex.Message;
                return response;
            }
            response.Status = 200;
            response.Message = "Successfully Logged In";
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
