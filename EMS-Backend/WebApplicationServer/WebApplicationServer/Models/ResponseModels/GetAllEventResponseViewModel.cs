using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class GetAllEventResponseViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<EventViewModel> AllEvents { get; set; }
    }
}
