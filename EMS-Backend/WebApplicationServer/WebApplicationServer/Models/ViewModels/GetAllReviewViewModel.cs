using System.ComponentModel.DataAnnotations;
namespace WebApplicationServer.Models.ViewModels
{
    public class GetAllReviewViewModel
    {
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public string Description { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public bool IsReported { get; set; } = false;
    }
}