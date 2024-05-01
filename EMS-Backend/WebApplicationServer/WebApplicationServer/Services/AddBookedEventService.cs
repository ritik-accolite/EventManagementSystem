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

        //public async Task<ResponseViewModel> AddBookedEvent(AddBookedEventViewModel addBookedEvent, string Id)
        //{
        //    BookedEvent eventbookedToBeAdded = new BookedEvent
        //    {
        //        //EventName = addBookedEvent.EventName
        //        EventId = addBookedEvent.EventId,
        //        EventOrganizerId = addBookedEvent.EventOrganizerId,
        //        UserId = addBookedEvent.UserId,               
        //    };

        //    await _context.BookedEvents.AddAsync(eventbookedToBeAdded);
        //    await _context.SaveChangesAsync();

        //    ResponseViewModel response = new ResponseViewModel();
        //    if (eventbookedToBeAdded == null)
        //    {
        //        response.Status = 500;
        //        response.Message = "Unable to book event, please try again.";
        //        return response;
        //    }

        //    response.Status = 200;
        //    response.Message = "Successfully booked event.";
        //    return response;
        //}


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

        public async Task<ResponseViewModel> BookEvent(AddBookedEventViewModel addBookedEvent)
        {
            ResponseViewModel response = new ResponseViewModel();

            try
            {
                var eventToBook = await _context.Events.FindAsync(addBookedEvent.EventId);
                if (eventToBook == null)
                {
                    response.Status = 404;
                    response.Message = "Event not found";
                    return response;
                }

                if (eventToBook.Capacity <= 0)
                {
                    response.Status = 422;
                    response.Message = "Event is already fully booked";
                    return response;
                }
                BookedEvent bookedEvent = new BookedEvent
                {
                    EventId = addBookedEvent.EventId,
                    EventOrganizerId = eventToBook.EventOrganizerId,
                    UserId = addBookedEvent.UserId,
                    BookingDate = addBookedEvent.BookingDate
                };

                eventToBook.Capacity--;

                await _context.BookedEvents.AddAsync(bookedEvent);
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Event booked successfully";
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error booking event: {ex.Message}";
            }

            return response;
        }



        public async Task<ResponseViewModel> UnbookEvent(int bookingId)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var bookedEvent = await _context.BookedEvents.FindAsync(bookingId);
                if (bookedEvent == null)
                {
                    response.Status = 404;
                    response.Message = "Booked event not found";
                    return response;
                }

                var eventToUpdate = await _context.Events.FindAsync(bookedEvent.EventId);
                if (eventToUpdate == null)
                {
                    response.Status = 404;
                    response.Message = "Event not found";
                    return response;
                }

                _context.BookedEvents.Remove(bookedEvent);

                // Increase event capacity
                eventToUpdate.Capacity += 1; // Assuming each booking reserves one seat
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Event successfully unbooked";
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error unbooking event: {ex.Message}";
            }

            return response;
        }


        public async Task<List<BookedEventWithDetailsViewModel>> GetBookedEventsWithDetailsByUser(string userId)
        {
            var bookedEvents = await _context.BookedEvents
                .Where(be => be.UserId == userId)
                .Include(be => be.Event)
                    .ThenInclude(e => e.Organizer)
                .Include(be => be.User)
                .Select(be => new BookedEventWithDetailsViewModel
                {
                    BookingId = be.BookingId,
                    EventName = be.Event.EventName,
                    EventDate = be.Event.EventDate,
                    EventLocation = be.Event.EventLocation,
                    BookingDate = be.BookingDate,
                    UserName = be.User.UserName,
                    TicketPrice = be.Event.TicketPrice,
                    EventTime = be.Event.Event_Time,
                    EventOrganizerName = be.Event.Organizer.UserName
                })
                .ToListAsync();

            return bookedEvents;
        }


    }
}
