using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeEmailController : ControllerBase
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public ChangeEmailController(UserManager<Person> userManager, SignInManager<Person> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}
