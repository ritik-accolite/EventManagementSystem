namespace WebApplicationServer.Models.ViewModels
{
    public class BookedUserViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
