namespace WebApplicationServer.Models.ViewModels
{
    public class GetRegisterSuccessMailViewModel
    {
        public string SecretKey { get; set; }
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set;}
        public bool EnableSSL { get; set; }
    }
}
