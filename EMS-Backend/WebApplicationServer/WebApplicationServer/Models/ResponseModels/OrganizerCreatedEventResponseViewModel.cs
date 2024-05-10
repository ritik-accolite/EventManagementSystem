using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class OrganizerCreatedEventResponseViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<OrganizerCreatedEventViewModel>? AllEvents { get; set; }
    }
}
