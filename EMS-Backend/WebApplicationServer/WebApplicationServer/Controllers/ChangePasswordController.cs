using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public ChangePasswordController(UserManager<Person> userManager, SignInManager<Person> signInManager, ISendRegisterSuccessMailService sendRegisterSuccessMailService)
        {
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _userManager = userManager;
            _signInManager = signInManager;
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
