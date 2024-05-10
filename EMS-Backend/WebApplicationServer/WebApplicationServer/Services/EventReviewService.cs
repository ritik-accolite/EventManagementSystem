

using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Services.IService;
using WebApplicationServer.Models.ViewModels;
using Microsoft.EntityFrameworkCore;



namespace WebApplicationServer.Services
{


    public class EventReviewService : IEventReviewService
    {
        private readonly ApplicationDbContext _context;

        public EventReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddReview(int eventId, string userId, ReviewViewModel reviewRequest)
        {
            // Check if the event has already occurred
            var eventDetails = await _context.Events.FindAsync(eventId);
            if (eventDetails == null || eventDetails.EventDate > DateTime.Now)
            {
                throw new Exception("Event either does not exist or has not occurred yet.");
            }

            // Check if the user has booked the event
            var isUserBookedEvent = await _context.BookedEvents.AnyAsync(be => be.EventId == eventId && be.UserId == userId);
            if (!isUserBookedEvent)
            {
                throw new Exception("User has not booked this event.");
            }

            reviewRequest.IsReported = false;
            //bool isReported = reviewRequest.IsReported;
            // Add review to the database
            var review = new Review
            {
                EventId = eventId,
                UserId = userId,
                Description = reviewRequest.Description,
                Rating = reviewRequest.Rating,
                IsReported = reviewRequest.IsReported // Default to false if not provided
                // You can add other fields as needed
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}