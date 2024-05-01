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
        public async Task<IActionResult> LogoutPerson()
        {
            try
            {
                await _signInManager.SignOutAsync();
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Ok("Logout Successfully");
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = ex.Message;
                return response;
            }
            response.Status = 200;
            response.Message ="Successfully Logged In";
            return response;
        }
        }



        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
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
                        return BadRequest("Something went wrong");
                    }
                    await _signInManager.RefreshSignInAsync(user);
                    return Ok("Email Updated Successfully");
                }
            }
            return Ok(model);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return Ok("Password updated successfully");
                }
                else
                {
                    return BadRequest("Failed to update password");
                }
            }
            return BadRequest(ModelState);
        }

    }
}
