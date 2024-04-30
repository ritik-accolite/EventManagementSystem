namespace WebApplicationServer.Models.ResponseModels
{
    public class GetAllPersonResponseViewModel
    {

        public int Status { get; set; }

        public string Message { get; set; }

        public List<Person>? AllPersons { get; set; }
    }
}
