// BookedEventWithDetailsViewModel.cs
namespace WebApplicationServer.Models.ViewModels
{
    public class BookedEventWithDetailsViewModel
    {
        public int BookingId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }

        public int EventId { get; set; }
        public string EventLocation { get; set; }
        public DateTime BookingDate { get; set; }
        public string UserName { get; set; }
        public decimal TicketPrice { get; set; }
        public string EventTime { get; set; }
        public string EventOrganizerName { get; set; }
        public int NumberOfTickets { get; set; } // Number of tickets booked by the user
        public decimal TotalPrice { get; set; }
    }
}
