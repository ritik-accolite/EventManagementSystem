using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services.IService
{
    public interface IAddBookedEventService
    {
        public Task<ResponseViewModel> AddBookedEvent(AddBookedEventViewModel addBookedEvent, string Id);

        public Task<GetAllBookedEventResposeViewModel> GetAllBookedEvents();

        public Task<GetBookedEventByIdResponseViewModel> GetBookedEventsById(int id);

        public Task<ResponseViewModel> DeleteBookedEvent(int id);


    }
}
