namespace WebApplicationServer.Models.ViewModels
{
    public class ETicketViewModel
    {
        public int BookingId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
        public DateTime BookingDate { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public decimal TicketPrice { get; set; }

        public string ChiefGuest {  get; set; }
        public string EventTime { get; set; }
        public int NumberOfTickets { get; set; }
        public string BannerImage { get; set; }
        public string EventDescription { get; set; }
        public string EventCategory { get; set; }
        public string OrganizerName { get; set; }
        public string OrganizerEmail { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
