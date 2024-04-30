namespace WebApplicationServer.Models.ResponseModels
{
    public class GetEVentByIdResposeViewModel
    {
            public int Status { get; set; }

            public string Message { get; set; }

            public Event GetEventById { get; set; }
        }
    }

