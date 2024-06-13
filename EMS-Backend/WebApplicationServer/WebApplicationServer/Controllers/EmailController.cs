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
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly ApplicationDbContext _context;


        public EmailController(ISendRegisterSuccessMailService sendRegisterSuccessMailService, ApplicationDbContext context)
        {
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _context = context;
        }


        [HttpPost("SendEmailNotificationToBookedUsers")]
        public async Task<ResponseViewModel> SendEmailNotificationToBookedUsers(int eventId)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var organizer = User.FindFirstValue(ClaimTypes.Name);
                var role = User.FindFirstValue("Role");


                if (organizer == null || role != "Organizer")
                {
                    response.Status = 401;
                    response.Message = "You are either not logged in or you are not a Organizer.";
                    return response;
                }

                var bookedUsers = await _context.BookedEvents
                    .Where(be => be.EventId == eventId)
                    .Select(be => be.User.Email)
                    .ToListAsync();

                foreach (var email in bookedUsers)
                {
                    bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(email, "Upcoming Event Reminder", $"Hey {email},\r\n " +
                        $"We are excited to remind you about your upcoming event! We noticed that you have booked a ticket for the event, and we want to ensure you have all the necessary details to make the most of your experience.\r\n " +
                        $"Please take a moment to review your event details in your account.\r\n " +
                        $"If you have any questions or need further assistance, do not hesitate to contact us on 1800456123.\r\n" +
                        $"We look forward to seeing you at the event!\r\n" +
                        $"Best Regards,\r\n" +
                        $"EventHub Team");
                    if (!emailSent)
                    {
                        response.Status = 500;
                        response.Message = "Failed to send Mail";
                        return response;
                    }
                }

                response.Status = 200;
                response.Message = "Email notification sent successfully to all booked users";
                return response;

            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = "something went wrong" + ex.Message;
                return response;
            }
        }

        [HttpPost("SendEmailNotification/{eventId}")]
        public async Task<ResponseViewModel> SendEmailNotification(int eventId, SendEmailViewModel sendEmailViewModel)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var organizer = User.FindFirstValue(ClaimTypes.Name);
                var role = User.FindFirstValue("Role");
                if (organizer == null || role != "Organizer")
                {
                    response.Status = 401;
                    response.Message = "You are either not logged in or you are not a Organizer.";
                    return response;
                }
                var bookedUsers = await _context.BookedEvents
                    .Where(be => be.EventId == eventId)
                    .Select(be => be.User.Email)
                    .ToListAsync();
                foreach (var email in bookedUsers)
                {
                    bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(email, sendEmailViewModel.Subject, sendEmailViewModel.Message);
                    if (!emailSent)
                    {

                        response.Status = 500;
                        response.Message = "Failed to send Mail";
                        return response;
                    }
                }
                response.Status = 200;
                response.Message = "Email notification sent successfully to all booked users";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = "something went wrong" + ex.Message;
                return response;
            }
        }
    }
}
