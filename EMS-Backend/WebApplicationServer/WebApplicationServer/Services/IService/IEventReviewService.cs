using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services.IService
{   public interface IEventReviewService
    {
        public Task<List<GetAllReviewViewModel>> GetAllReviews();
        public Task<ResponseViewModel> AddReview(int eventId, string userId, ReviewViewModel reviewRequest);
        public Task<List<GetAllReviewViewModel>> GetReviewByEventId(int eventid);
    }
}
