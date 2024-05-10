using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class GetAllCategoriesResponseViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<string>? AllEventCategory { get; set; }
    }
}




