namespace WebApplicationServer.Models.ResponseModels
{
    public class GetAllBookedEventResposeViewModel
    {
            public int Status { get; set; }

            public string Message { get; set; }

            public List<BookedEvent>? AllBookedEvents { get; set; }
        }
    }
