using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services.IService
{
    public interface IEventReviewService
    {
        public Task<bool> AddReview(int eventId, string userId, ReviewViewModel reviewRequest);
    }

}
