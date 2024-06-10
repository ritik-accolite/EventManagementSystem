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

                if (bookedEvent.NumberOfTickets > 5)
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



    


        public async Task<ETicketViewModel> GenerateETicketAsync(int bookingId)
        {
            var response = new ETicketViewModel();

            var bookedEventDetails = await _context.BookedEvents
                .Where(be => be.BookingId == bookingId)
                .Include(be => be.Event)
                .ThenInclude(e => e.Organizer)
                .Include(be => be.User)
                .FirstOrDefaultAsync();


            var eTicket = new ETicketViewModel
            {
                BookingId = bookingId,
                EventName = bookedEventDetails.Event.EventName,
                EventDate = bookedEventDetails.Event.EventDate,
                EventLocation = bookedEventDetails.Event.EventLocation,
                BookingDate = bookedEventDetails.BookingDate,
                UserName = $"{bookedEventDetails.User?.FirstName} {bookedEventDetails.User?.LastName}",
                UserEmail = bookedEventDetails.User.Email,
                UserPhone = bookedEventDetails.User?.PhoneNumber,
                TicketPrice = bookedEventDetails.Event.TicketPrice,
                EventTime = bookedEventDetails.Event.Event_Time,
                ChiefGuest = bookedEventDetails.Event.ChiefGuest,
                NumberOfTickets = bookedEventDetails.NumberOfTickets,
                BannerImage = bookedEventDetails.Event.BannerImage,
                EventDescription = bookedEventDetails.Event.Description,
                EventCategory = bookedEventDetails.Event.EventCategory,
                OrganizerName = $"{bookedEventDetails.Event.Organizer?.FirstName} {bookedEventDetails.Event.Organizer?.LastName}",
                OrganizerEmail = bookedEventDetails.Event.Organizer?.Email,
                TotalPrice = bookedEventDetails.NumberOfTickets * bookedEventDetails.Event.TicketPrice
            };
            return eTicket;
        }
        }
}