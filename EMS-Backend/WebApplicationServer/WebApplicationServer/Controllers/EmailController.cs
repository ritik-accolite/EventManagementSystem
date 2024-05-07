using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IAddBookedEventService _addBookedEventService;

        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly ApplicationDbContext _context;


        public EmailController(IAddBookedEventService addBookedEventService, ISendRegisterSuccessMailService sendRegisterSuccessMailService, ApplicationDbContext context)
        {
            _addBookedEventService = addBookedEventService;
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _context = context;
        }


        [HttpPost("SendEmailNotification")]
        public async Task<ResponseViewModel> SendEmailNotification(int eventId)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                //var organizer = await _userManager.GetUserAsync(User);
                var organizer = User.FindFirstValue(ClaimTypes.Name);
                var role = User.FindFirstValue(ClaimTypes.Role);

                if (organizer == null || role != "Organizer")
                {
                    //response = new ResponseViewModel();
                    response.Status = 401;
                    response.Message = "You are either not logged in or you are not a Organizer.";
                    return response;
                }

                // Get the list of users who have booked the event
                var bookedUsers = await _context.BookedEvents
                    .Where(be => be.EventId == eventId)
                    .Select(be => be.User.Email)
                    .ToListAsync();

                // Send email notification to each booked user
                foreach (var email in bookedUsers)
                {
                    bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(email, "Event booking", "congrats you have an upcoming event");
                    if (!emailSent)
                    {
                        // Handle email sending failure for individual users
                        // You can choose to continue sending emails or break the loop
                        //response = new ResponseViewModel();
                        response.Status = 500;
                        response.Message = "Failed to send Mail";
                        return response;
                    }
                }

                response.Status = 200;
                response.Message = "Email notification sent successfully to all booked users";
                return response;

                //return Ok("Email notification sent successfully to all booked users.");
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = "something went wrong" + ex.Message;
                return response;
                //return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
