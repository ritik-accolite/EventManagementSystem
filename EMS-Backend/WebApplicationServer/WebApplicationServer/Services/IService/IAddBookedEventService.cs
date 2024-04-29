using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services.IService
{
    public interface IAddBookedEventService
    {
        public Task<ResponseViewModel> AddBookedEvent(AddBookedEventViewModel addBookedEvent, string Id);
    }
}
