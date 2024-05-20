namespace WebApplicationServer.Models.ViewModels
{
    public class GetAllBookedEventByAdminViewModel
    {
        public int BookingId { get; set; }
        public int eventid { get; set; }
        public string userId { get; set; }
        public string OrganizerId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TicketPrice { get; set; }
        public int NumberOfTickets { get; set; } // Number of tickets booked by the user
        public decimal TotalPrice { get; set; }
    }
}
