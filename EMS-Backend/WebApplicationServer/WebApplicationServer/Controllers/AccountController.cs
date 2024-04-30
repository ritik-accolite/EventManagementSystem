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
        public async Task<IActionResult> RegisterPerson(RegisterViewModel person)
        {

            string message = "";
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
                    return BadRequest(result);
                }

                message = "Registered Successfully";
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, please try again" + ex.Message);
            }
            return Ok(result);

        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginPerson(LoginViewModel login)
        {
            string message = "";

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
                    return Unauthorized("Check your Login info");
                }
                message = "Login Successfully";
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong" + ex.Message);
            }
            return Ok(message);
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
                return BadRequest("Something went wrong" + ex.Message);
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
