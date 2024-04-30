using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Services
{
    public class AddBookedEventService : IAddBookedEventService
    {
        private readonly ApplicationDbContext _context;


        public AddBookedEventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> AddBookedEvent(AddBookedEventViewModel addBookedEvent, string Id)
        {
            BookedEvent eventbookedToBeAdded = new BookedEvent
            {
                //EventName = addBookedEvent.EventName
                EventId = addBookedEvent.EventId,
                EventOrganizerId = addBookedEvent.EventOrganizerId,
                UserId = addBookedEvent.UserId,               
            };

            await _context.BookedEvents.AddAsync(eventbookedToBeAdded);
            await _context.SaveChangesAsync();

            ResponseViewModel response = new ResponseViewModel();
            if (eventbookedToBeAdded == null)
            {
                response.Status = 500;
                response.Message = "Unable to book event, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully booked event.";
            return response;
        }


        public async Task<GetAllBookedEventResposeViewModel> GetAllBookedEvents()
        {
            GetAllBookedEventResposeViewModel response = new GetAllBookedEventResposeViewModel();
            response.Status = 200;
            response.Message = "All Booked Events Fetched";
            response.AllBookedEvents = await _context.BookedEvents.ToListAsync();
            return response;
        }


        public async Task<GetBookedEventByIdResponseViewModel> GetBookedEventsById(int BookingId)
        {
            GetBookedEventByIdResponseViewModel response = new GetBookedEventByIdResponseViewModel();
            response.Status = 200;
            response.Message = "All Events Fetched";
            response.GetBookedEventById = await _context.BookedEvents.FindAsync(BookingId);
            return response;
        }




        public async Task<ResponseViewModel> DeleteBookedEvent(int id)
        {
            ResponseViewModel response = new ResponseViewModel();

            try
            {
                var eventToDelete = await _context.BookedEvents.FindAsync(id);
                if (eventToDelete == null)
                {
                    response.Status = 404;
                    response.Message = "Booked Event not found";
                }
                else
                {
                    _context.BookedEvents.Remove(eventToDelete);
                    await _context.SaveChangesAsync();
                    response.Status = 200;
                    response.Message = "Booked Event deleted successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error deleting Booked event: {ex.Message}";
            }

            return response;
        }

    }
}
