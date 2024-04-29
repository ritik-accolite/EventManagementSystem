using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Services
{
    public class AddEventService : IAddEventService
    {
        private readonly ApplicationDbContext _context;


        public AddEventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> AddEvent(AddEventViewModel addEvent, string Id )
        {
            Event eventToBeAdded = new Event
            {
                EventName = addEvent.EventName,
                EventCategory = addEvent.EventCategory,
                EventLocation = addEvent.EventLocation,
                Event_Time = addEvent.Event_Time,
                EventDate = addEvent.EventDate,
                ChiefGuest = addEvent.ChiefGuest,
                Description = addEvent.Description,
                Capacity = addEvent.Capacity,
                TicketPrice = addEvent.TicketPrice,
                BannerImage = addEvent.BannerImage,
                EventOrganizerId = Id
            };
            await _context.Events.AddAsync(eventToBeAdded);
            await _context.SaveChangesAsync();


        ResponseViewModel response = new ResponseViewModel();
            if (eventToBeAdded == null)
            {
                response.Status = 500;
                response.Message = "Unable to add event, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Event Successfully added.";
            return response;
        }

        public async Task<GetAllEventResponseViewModel> GetAllEvents()
        {
            GetAllEventResponseViewModel response = new GetAllEventResponseViewModel();
            response.Status = 200;
            response.Message = "All Events Fetched";
            response.AllEvents = await _context.Events.ToListAsync();
            return response;
        }


        public async Task<GetEVentByIdResposeViewModel> GetEventById(int EventId)
        {
            GetEVentByIdResposeViewModel response = new GetEVentByIdResposeViewModel();
            response.Status = 200;
            response.Message = "All Events Fetched";
            response.GetEventById = await  _context.Events.FindAsync(EventId);
            return response;
        }
    }
}
