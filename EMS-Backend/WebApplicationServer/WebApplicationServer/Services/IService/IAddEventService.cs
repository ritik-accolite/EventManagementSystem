using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services.IService
{
    public interface IAddEventService
    {
        public Task<ResponseViewModel> AddEvent(AddEventViewModel addEvent, string Id);

        public Task<GetAllEventResponseViewModel> GetAllEvents();

        public Task<GetEVentByIdResposeViewModel> GetEventById(int id);
    }
}
