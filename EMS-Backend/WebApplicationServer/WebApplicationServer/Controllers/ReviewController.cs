using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;
namespace WebApplicationServer.Controllers
{
    public class ReviewController : ControllerBase
    {
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly IEventReviewService _eventReviewService;
        private readonly ApplicationDbContext _context;
        public ReviewController(IEventReviewService eventReviewService, ISendRegisterSuccessMailService sendRegisterSuccessMailService, ApplicationDbContext context)
        {
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _eventReviewService = eventReviewService;
            _context = context;
        }
        //GetAllReviews
        [HttpGet("admin/allreviews")]
        public async Task<GetAllReviewResponseViewModel> GetAllReviews()
        {
            GetAllReviewResponseViewModel response = new GetAllReviewResponseViewModel();
            try
            {
                response.Status = 200;
                response.Message = "All Reviews Fetched";
                response.AllReviews = await _eventReviewService.GetAllReviews();
                return response;
            }
            catch (Exception ex)
            {
                response.Status = 200;
                response.Message = $"Error Fetching reviews: {ex.Message}";
            }
            return response;
        }
        [HttpPost("events/reviews/{eventId}")]
        [Authorize]
        public async Task<ResponseViewModel> AddReview(int eventId, [FromBody] ReviewViewModel reviewRequest)
        {
            ResponseViewModel response = new ResponseViewModel();
            string userId = User.FindFirst("Id").Value;
            try
            {
                response.Status = 200;
                response.Message = "Review Added Successfully";
                response = await _eventReviewService.AddReview(eventId, userId, reviewRequest);
            }
            catch (Exception ex)
            {
                response.Status = 200;
                response.Message = $"Error adding review: {ex.Message}";
            }
            return response;
        }
        [HttpPost("resolveissue/{userId}")]
        public async Task<ResponseViewModel> ResolveIssue(string userId)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(user.Email, "Issue Resolved", "Your reported issue has been resolved.");
                if (!emailSent)
                {
                    // Handle email sending failure
                    response.Status = 500;
                    response.Message = "Failed to send Reported issue email";
                    return response;
                }
            }
            response.Status = 200;
            response.Message = "Email Sent Successfully";
            return response;
        }
        //GetReviewByEventId
        [HttpGet("admin/reviewsbyeventid")]
        public async Task<GetAllReviewResponseViewModel> GetReviewByEventId(int eventid)
        {
            GetAllReviewResponseViewModel response = new GetAllReviewResponseViewModel();
            try
            {
                response.Status = 200;
                response.Message = "All Reviews Fetched";
                response.AllReviews = await _eventReviewService.GetReviewByEventId(eventid);
                return response;
            }
            catch (Exception ex)
            {
                response.Status = 200;
                response.Message = $"Error Fetching reviews: {ex.Message}";
            }
            return response;
        }
    }
}