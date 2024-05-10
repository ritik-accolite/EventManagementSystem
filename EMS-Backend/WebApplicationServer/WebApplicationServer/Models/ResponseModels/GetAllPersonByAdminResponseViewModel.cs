using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class GetAllPersonByAdminResponseViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<GetAllPersonByAdminViewModel>? AllPersons { get; set; }
    }
}
