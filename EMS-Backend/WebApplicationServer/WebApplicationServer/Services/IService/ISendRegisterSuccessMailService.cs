namespace WebApplicationServer.Services.IService
{
    public interface ISendRegisterSuccessMailService
    {
        Task<bool> SendRegisterSuccessMailAsync(string email, string Subject, string message);
    }
}
