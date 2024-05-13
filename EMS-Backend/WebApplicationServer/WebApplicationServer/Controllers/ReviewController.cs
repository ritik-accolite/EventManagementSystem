using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    public class ReviewController : ControllerBase
    {

        private readonly IGetAllPerson _getAllPerson;
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IEventReviewService _eventReviewService;

        public ReviewController(IEventReviewService eventReviewService, UserManager<Person> userManager, SignInManager<Person> signInManager, IGetAllPerson getAllPerson, ISendRegisterSuccessMailService sendRegisterSuccessMailService)
        {
            _getAllPerson = getAllPerson;
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _userManager = userManager;
            _signInManager = signInManager;
            _eventReviewService = eventReviewService;
        }

        [HttpPost]
        [Route("events/{eventId}/reviews")]
        [Authorize]
        public async Task<IActionResult> AddReview(int eventId, [FromBody] ReviewViewModel reviewRequest)
        {
            string userId = User.FindFirst("Id").Value;

            try
            {
                var result = await _eventReviewService.AddReview(eventId, userId, reviewRequest);

                if (result)
                {
                    return Ok(new { Status = 200, Message = "Review added successfully." });
                }
                else
                {
                    return BadRequest(new { Status = 400, Message = "Failed to add review." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, new { Status = 500, Message = $"Error adding review: {ex.Message}" });
            }
        }
    }
}
