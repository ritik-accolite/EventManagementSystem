using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class AuthenticatedLoginResponseViewModel : ResponseViewModel
    {
        public string? Token { get; set; }
    }
}
