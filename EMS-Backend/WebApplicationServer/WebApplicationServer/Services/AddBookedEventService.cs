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

        public async Task<List<BookedEvent>> GetAllBookedEvents()
        {
            var bookedEvents = await _context.BookedEvents.ToListAsync();
            return bookedEvents;
        }

        //ONLY ADMIN CAN ACCESS 
        public async Task<List<GetAllBookedEventByAdminViewModel>> GetAllBookedEventsByAdmin()
        {
            var bookedevents = await _context.BookedEvents
                .Include(be => be.Event)
                .Select(be => new GetAllBookedEventByAdminViewModel
                {
                    BookingId = be.BookingId,
                    eventid = (int)be.EventId,
                    userId = be.UserId,
                    OrganizerId = be.EventOrganizerId,
                    BookingDate = be.BookingDate,
                    TicketPrice = be.Event.TicketPrice,
                    NumberOfTickets = be.NumberOfTickets,
                    TotalPrice = be.Event.TicketPrice * be.NumberOfTickets
                }).ToListAsync();
            return bookedevents;
        }


        public async Task<BookedEvent> GetBookedEventsById(int BookingId)
        {          
            var bookedEvent = await _context.BookedEvents.FindAsync(BookingId);
            return bookedEvent;
        }

    public async Task<ResponseViewModel> BookTickets(AddBookedEventViewModel addBookedEvent)
    {
        ResponseViewModel response = new ResponseViewModel();

        try
        {
            // Check if the user has already booked tickets for the same event
            var existingBooking = await _context.BookedEvents
                .FirstOrDefaultAsync(be => be.EventId == addBookedEvent.EventId && be.UserId == addBookedEvent.UserId);

            if (existingBooking != null)
            {
                response.Status = 400;
                response.Message = "You have already booked tickets for this event";
                return response;
            }

            var eventEntity = await _context.Events.FindAsync(addBookedEvent.EventId);
            if (eventEntity == null)
            {
                response.Status = 404;
                response.Message = "Event not found";
                return response;
            }

            if (eventEntity.Capacity < addBookedEvent.NumberOfTickets)
            {
                response.Status = 400;
                response.Message = "Not enough tickets available for this event";
                return response;
            }

            var bookedEvent = new BookedEvent
            {
                EventId = addBookedEvent.EventId,
                EventOrganizerId = eventEntity.EventOrganizerId,
                UserId = addBookedEvent.UserId,
                BookingDate = DateTime.Now,
                NumberOfTickets = addBookedEvent.NumberOfTickets
            };

            if(bookedEvent.NumberOfTickets > 5)
            {
                    response.Status = 400;
                    response.Message = "You cannot Book more than 5 tickets.";
                    return response;
            }

            _context.BookedEvents.Add(bookedEvent);
            eventEntity.Capacity -= addBookedEvent.NumberOfTickets;
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
                eventToUpdate.Capacity += bookedEvent.NumberOfTickets;

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

        //if user want to see events booked by him
        public async Task<List<EventDetailsViewModel>> GetBookedEventsWithDetailsByUser(string userId)
        {
            var bookedEvents = await _context.BookedEvents
                .Where(be => be.UserId == userId)
                .Include(be => be.Event)
                    .ThenInclude(e => e.Organizer)
                .Include(be => be.User)
                .Select(be => new EventDetailsViewModel
                {
                    BookingId = be.BookingId,
                    EventName = be.Event.EventName,
                    EventDate = be.Event.EventDate,
                    EventId = be.Event.EventId,
                    EventLocation = be.Event.EventLocation,
                    BookingDate = be.BookingDate,
                    UserName = be.User.UserName,
                    TicketPrice = be.Event.TicketPrice,
                    EventTime = be.Event.Event_Time,
                    EventOrganizerName = be.Event.Organizer.UserName,
                    NumberOfTickets = be.NumberOfTickets,
                    TotalPrice = be.NumberOfTickets * be.Event.TicketPrice
                })
                .ToListAsync();

            return bookedEvents;
        }

        public async Task<EventTicketStatusViewModel> GetEventTicketStatus(string organizerId)
        {
            var eventTicketStatus = new EventTicketStatusViewModel();

            // Get events created by the organizer
            var events = await _context.Events
                .Where(e => e.EventOrganizerId == organizerId)
                .ToListAsync();

            foreach (var @event in events)
            {
                var eventStatus = new EventTicketStatusViewModel.EventStatus
                {
                    eventId = @event.EventId,
                    EventName = @event.EventName,
                    TotalTicketsSold = 0,
                    TotalTicketsLeft = @event.Capacity,
                    TicketPrice = @event.TicketPrice
                };

                // Get booked events for the current event
                var bookedEvents = await _context.BookedEvents
                    .Include(be => be.User)
                    .Where(be => be.EventId == @event.EventId)
                    .ToListAsync();

                foreach (var bookedEvent in bookedEvents)
                {
                    eventStatus.TotalTicketsSold += bookedEvent.NumberOfTickets;

                    // Add user details who booked the event
                    eventStatus.UserTickets.Add(new EventTicketStatusViewModel.UserTicket
                    {
                        Username = $"{bookedEvent.User.FirstName} {bookedEvent.User.LastName}",
                        TotalTicketsBooked = bookedEvent.NumberOfTickets,                       
                        TotalPayable = bookedEvent.NumberOfTickets * @event.TicketPrice
                    });
                }

                eventTicketStatus.Events.Add(eventStatus);
            }

            return eventTicketStatus;
        }

    }
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





//public async Task<ResponseViewModel> BookTickets(AddBookedEventViewModel addBookedEvent)
//{
//    ResponseViewModel response = new ResponseViewModel();

//    // Find the event by ID
//    var @event = await _context.Events.FindAsync(addBookedEvent.EventId);
//    if (@event == null)
//    {
//        response.Status = 404;
//        response.Message = "Event not found";
//        return response;
//    }

//    // Check if there are enough tickets available
//    if (@event.Capacity < addBookedEvent.NumberOfTickets)
//    {
//        response.Status = 400;
//        response.Message = "Not enough tickets available for this event";
//        return response;
//    }

//    // Add booked event to the database
//    var bookedEvent = new BookedEvent
//    {
//        EventId = addBookedEvent.EventId,
//        EventOrganizerId = @event.EventOrganizerId,
//        UserId = addBookedEvent.UserId,
//        BookingDate = DateTime.Now,
//        NumberOfTickets = addBookedEvent.NumberOfTickets
//    };
//    _context.BookedEvents.Add(bookedEvent);

//    // Update the event's capacity
//    @event.Capacity -= addBookedEvent.NumberOfTickets;

//    // Save changes to the database
//    await _context.SaveChangesAsync();

//    response.Status = 200;
//    response.Message = "Event booked successfully";
//    return response;
//}






//public async Task<ResponseViewModel> DeleteBookedEvent(int id)
//{
//    ResponseViewModel response = new ResponseViewModel();

//    try
//    {
//        var eventToDelete = await _context.BookedEvents.FindAsync(id);
//        if (eventToDelete == null)
//        {
//            response.Status = 404;
//            response.Message = "Booked Event not found";
//        }
//        else
//        {
//            _context.BookedEvents.Remove(eventToDelete);
//            await _context.SaveChangesAsync();
//            response.Status = 200;
//            response.Message = "Booked Event deleted successfully";
//        }
//    }
//    catch (Exception ex)
//    {
//        response.Status = 500;
//        response.Message = $"Error deleting Booked event: {ex.Message}";
//    }

//    return response;
//}





//public async Task<ResponseViewModel> BookEvent(AddBookedEventViewModel addBookedEvent)
//{
//    ResponseViewModel response = new ResponseViewModel();

//    try
//    {
//        var eventToBook = await _context.Events.FindAsync(addBookedEvent.EventId);
//        if (eventToBook == null)
//        {
//            response.Status = 404;
//            response.Message = "Event not found";
//            return response;
//        }

//        if (eventToBook.Capacity <= 0)
//        {
//            response.Status = 422;
//            response.Message = "Event is already fully booked";
//            return response;
//        }
//        BookedEvent bookedEvent = new BookedEvent
//        {
//            EventId = addBookedEvent.EventId,
//            EventOrganizerId = eventToBook.EventOrganizerId,
//            UserId = addBookedEvent.UserId,
//            BookingDate = addBookedEvent.BookingDate
//        };

//        eventToBook.Capacity--;

//        await _context.BookedEvents.AddAsync(bookedEvent);
//        await _context.SaveChangesAsync();

//        response.Status = 200;
//        response.Message = "Event booked successfully";
//    }
//    catch (Exception ex)
//    {
//        response.Status = 500;
//        response.Message = $"Error booking event: {ex.Message}";
//    }

//    return response;
//}








//public async Task<ResponseViewModel> UnbookEvent(int bookingId)
//{
//    ResponseViewModel response = new ResponseViewModel();
//    try
//    {
//        var bookedEvent = await _context.BookedEvents.FindAsync(bookingId);
//        if (bookedEvent == null)
//        {
//            response.Status = 404;
//            response.Message = "Booked event not found";
//            return response;
//        }

//        var eventToUpdate = await _context.Events.FindAsync(bookedEvent.EventId);
//        if (eventToUpdate == null)
//        {
//            response.Status = 404;
//            response.Message = "Event not found";
//            return response;
//        }

//        _context.BookedEvents.Remove(bookedEvent);

//        // Increase event capacity
//        eventToUpdate.Capacity += 1; // Assuming each booking reserves one seat
//        await _context.SaveChangesAsync();

//        response.Status = 200;
//        response.Message = "Event successfully unbooked";
//    }
//    catch (Exception ex)
//    {
//        response.Status = 500;
//        response.Message = $"Error unbooking event: {ex.Message}";
//    }

//    return response;
//}