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
            catch(Exception ex) 
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
}
