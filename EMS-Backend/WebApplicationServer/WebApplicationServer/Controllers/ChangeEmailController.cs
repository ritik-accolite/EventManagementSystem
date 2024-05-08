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

        //private readonly IGetAllPerson _getAllPerson;
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public ChangeEmailController(UserManager<Person> userManager, SignInManager<Person> signInManager, ISendRegisterSuccessMailService sendRegisterSuccessMailService)
        {
            //_getAllPerson = getAllPerson;
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
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

                //var u = User.FindFirstValue(ClaimTypes.Name);

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


                    //string path = Path.GetFullPath("C:\\Users\\ajay.k_int1595\\Desktop\\Ems-Project\\EventManagementSystem\\EMS-Backend\\WebApplicationServer\\WebApplicationServer\\HtmlTemplate\\ChangeEmail.html");
                    //string htmlString = System.IO.File.ReadAllText(path);
                    ////htmlString = htmlString.Replace("{{title}}", "Email Changed Successfull");
                    ////htmlString = htmlString.Replace("{{Username}}", model.Email);
                    //bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(model.Email, "Email Updated Successfully", htmlString);

                    //if (!emailSent)
                    //{
                    //    // Handle email sending failure
                    //    response.Status = 500;
                    //    response.Message = "Failed to send  Update Email mail";
                    //    return response;
                    //}
                    return response;
                }
            }
            response.Status = 403;
            response.Message = "Something went wrong";
            return response;
        }
    }
}
