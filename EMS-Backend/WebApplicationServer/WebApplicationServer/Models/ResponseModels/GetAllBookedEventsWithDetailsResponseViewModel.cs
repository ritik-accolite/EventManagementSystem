using System.Collections.Generic;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class GetAllBookedEventsWithDetailsResponseViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<BookedEventWithDetailsViewModel>? BookedEvents { get; set; }
    }
}