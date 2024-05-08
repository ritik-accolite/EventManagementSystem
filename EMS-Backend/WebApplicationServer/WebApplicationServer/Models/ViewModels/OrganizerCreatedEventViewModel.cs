namespace WebApplicationServer.Models.ViewModels
{
    public class OrganizerCreatedEventViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Event_Time { get; set; }
        public string EventCategory { get; set; }
        public string EventDescription { get; set; }        
        public string ChiefGuest { get; set; }
        public string EventLocation { get; set; }
        public decimal TicketPrice { get; set; }
        public int Capacity { get; set; }
        public string BannerImage { get; set; }


    }
}
