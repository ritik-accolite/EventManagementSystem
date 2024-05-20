using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class GetEventWithUserListResponseViewModel
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public EventDetailsWithUserViewModel? EventwithUser { get; set; }
    }
}
