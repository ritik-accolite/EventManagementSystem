using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Services.IService;
using WebApplicationServer.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Models.ResponseModels;
using Azure;



namespace WebApplicationServer.Services
{
    public class EventReviewService : IEventReviewService
    {
        private readonly ApplicationDbContext _context;
        public EventReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllReviewViewModel>> GetAllReviews()
        {
            var AllReviews = await _context.Reviews.ToListAsync();
            var allreviewslist = AllReviews.Select(e =>  new GetAllReviewViewModel{

                ReviewId = e.ReviewId,
                UserId = e.UserId,
                EventId = e.EventId,
                Description = e.Description,
                Rating = e.Rating,
                IsReported = e.IsReported

            }).ToList();

            return allreviewslist;
        }

    public async Task<ResponseViewModel> AddReview(int eventId, string userId, ReviewViewModel reviewRequest)
        {
            ResponseViewModel response = new ResponseViewModel();

            var eventDetails = await _context.Events.FindAsync(eventId);
            if (eventDetails == null)
            {
                response.Status = 400;
                response.Message = "Event either does not exist or has not occurred yet.";
                return response;
            }

            var isUserBookedEvent = await _context.BookedEvents.AnyAsync(be => be.EventId == eventId && be.UserId == userId);
            if (!isUserBookedEvent)
            {
                response.Status = 400;
                response.Message = "User has not booked this event.";
                return response;
               
            }

            var existingReview = await _context.Reviews.AnyAsync(r => r.EventId == eventId && r.UserId == userId);
            if (existingReview)
            {
                response.Status = 400;
                response.Message = "User has already given review for this event.";
                return response;
            }

            var review = new Review
            {
                EventId = eventId,
                UserId = userId,
                Description = reviewRequest.Description,
                Rating = reviewRequest.Rating,
                IsReported = reviewRequest.IsReported
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            response.Status = 200;
            response.Message = "Review Added Successfully";
            return response;
        }

        public async Task<List<GetAllReviewViewModel>> GetReviewByEventId(int eventid)
        {
            var AllReviews = await _context.Reviews.Where(r => r.EventId == eventid).ToListAsync();
            var allreviewslist = AllReviews.Select(e => new GetAllReviewViewModel
            {

                ReviewId = e.ReviewId,
                UserId = e.UserId,
                EventId = e.EventId,
                Description = e.Description,
                Rating = e.Rating,
                IsReported = e.IsReported

            }).ToList();

            return allreviewslist;
        }
    }
}