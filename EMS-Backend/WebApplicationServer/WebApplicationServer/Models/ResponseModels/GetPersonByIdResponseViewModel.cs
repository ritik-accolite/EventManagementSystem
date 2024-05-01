namespace WebApplicationServer.Models.ResponseModels
{
    public class GetPersonByIdResponseViewModel
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public Person GetPersonById { get; set; }
    }
}
