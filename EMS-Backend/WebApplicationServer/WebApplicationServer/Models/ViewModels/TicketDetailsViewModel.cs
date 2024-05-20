namespace WebApplicationServer.Models.ViewModels
{
    public class TicketDetailsViewModel
    {
        public string UserName { get; set; }
        public int TotalTickets { get; set; }
        public decimal TotalAmountReceived { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
        public int TotalTicketsAvailable { get; set; }

    }

}
