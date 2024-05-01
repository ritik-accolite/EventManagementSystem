namespace WebApplicationServer.Models.ResponseModels
{
    public class GetEventByAppliedFilterResponseViewModel
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public List<Event>? CategoryEvents { get; set; }
    }
}
