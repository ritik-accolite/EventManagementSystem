//namespace WebApplicationServer.Models.ResponseModels
//{
//    public class CheckoutResponseViewModel
//    {
//    }
//}


//namespace WebApplicationServer.Models.ViewModels
namespace WebApplicationServer.Models.ResponseModels
{
    public class CheckoutResponseViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public string? token { get; set; }
        public string? Url { get; set; }
    }
}
