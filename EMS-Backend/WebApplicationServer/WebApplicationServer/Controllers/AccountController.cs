using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager)
        {
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
            catch(Exception ex) 
            {
                return BadRequest("Something went wrong" + ex.Message);
            }
            return Ok(message);
        }


    }
}
