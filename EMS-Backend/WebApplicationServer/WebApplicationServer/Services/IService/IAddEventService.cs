using Microsoft.AspNetCore.Mvc;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services.IService
{
    public interface IAddEventService
    {
        //public Task<ResponseViewModel> AddEvent(AddEventViewModel addEvent, string Id);
        public Task<ResponseViewModel> AddEvent(AddEventViewModel addEvent, string Id, IFormFile bannerImage);

        //public Task<GetAllEventResponseViewModel> GetAllEvents();

        public Task<List<EventViewModel>> GetAllEvents();

        public Task<GetEVentByIdResposeViewModel> GetEventById(int id);

        public Task<ResponseViewModel> DeleteEvent(int id);

        public Task<ResponseViewModel> UpdateEvent(int id, UpdateEventViewModel updateEvent, string userId);
        public Task<GetEventByAppliedFilterResponseViewModel>? GetEventsByCategory(string category);
        public Task<GetEventByAppliedFilterResponseViewModel>? GetEventsByLocation(string location);

        public Task<List<TicketDetailsViewModel>> GetTicketDetailsForOrganizer(int eventId, string organizerId);

        //public Task<List<OrganizerCreatedEventViewModel>> GetOrganizerCreatedEvents(string organizerId);
        public Task<List<OrganizerCreatedEventViewModel>> GetOrganizerCreatedEvents(string organizerId);
        public Task<List<string>> GetUniqueEventCategories();
        public Task<List<string>> GetUniqueEventLocation();

        //public Task<GetPastEventsResponseViewModel> GetPastEvents();

        public Task<IEnumerable<EventViewModel>> GetPastEvents();


        //public Task<GetUpcomingEventsResponseViewModel> GetUpcomingEvents();
        public Task<IEnumerable<EventViewModel>> GetUpcomingEvents();

        public Task<EventDetailsWithUserViewModel> GetEventDetails(int eventId);

    }
}
