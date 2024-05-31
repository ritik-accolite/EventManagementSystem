using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class GetETicketByBookingIdResponseViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public ETicketViewModel? ETicket { get; set; }

    }
}
