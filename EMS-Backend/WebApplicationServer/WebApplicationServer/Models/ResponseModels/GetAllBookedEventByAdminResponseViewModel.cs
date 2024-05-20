using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class GetAllBookedEventByAdminResponseViewModel
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public List<GetAllBookedEventByAdminViewModel>? AllBookedEvents { get; set; }
    }
}
