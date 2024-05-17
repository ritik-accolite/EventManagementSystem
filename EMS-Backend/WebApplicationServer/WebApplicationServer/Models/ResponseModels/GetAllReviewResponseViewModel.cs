using WebApplicationServer.Models.ViewModels;
namespace WebApplicationServer.Models.ResponseModels
{
    public class GetAllReviewResponseViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<GetAllReviewViewModel>? AllReviews { get; set; }
    }
}