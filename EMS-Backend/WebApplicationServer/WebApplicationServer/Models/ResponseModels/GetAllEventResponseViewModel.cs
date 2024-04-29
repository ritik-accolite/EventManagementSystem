namespace WebApplicationServer.Models.ResponseModels
{
    public class GetAllEventResponseViewModel
    {
            public int Status { get; set; }

            public string Message { get; set; }

            public List<Event>? AllEvents { get; set; }
}
}
