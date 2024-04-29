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
                UserId = addBookedEvent.UserId
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
    }
}

