namespace WebApplicationServer.Models.ResponseModels
{
    public class GetBookedEventByIdResponseViewModel
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public BookedEvent GetBookedEventById { get; set; }
    }
}
