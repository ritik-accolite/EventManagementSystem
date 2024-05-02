using Microsoft.AspNetCore.Mvc;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services.IService
{
    public interface IAddEventService
    {
        public Task<ResponseViewModel> AddEvent(AddEventViewModel addEvent, string Id);

        public Task<GetAllEventResponseViewModel> GetAllEvents();

        public Task<GetEVentByIdResposeViewModel> GetEventById(int id);

        public Task<ResponseViewModel> DeleteEvent(int id);

        public Task<ResponseViewModel> UpdateEvent(int id, UpdateEventViewModel updateEvent, string userId);
        public Task<GetEventByAppliedFilterResponseViewModel>? GetEventsByCategory(string category);
        public Task<GetEventByAppliedFilterResponseViewModel>? GetEventsByLocation(string location);

        public Task<List<TicketDetailsViewModel>> GetTicketDetailsForOrganizer(int eventId, string organizerId);
    }
}
