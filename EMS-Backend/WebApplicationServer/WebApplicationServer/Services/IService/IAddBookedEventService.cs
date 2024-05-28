using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services.IService
{
    public interface IAddBookedEventService
    {
        public Task<List<BookedEvent>> GetAllBookedEvents();
        public Task<BookedEvent> GetBookedEventsById(int BookingId);
        public Task<ResponseViewModel> UnbookEvent(int bookingId);
        public Task<List<EventDetailsViewModel>> GetBookedEventsWithDetailsByUser(string userId);
        public Task<ResponseViewModel> BookTickets(AddBookedEventViewModel addBookedEvent);
        public Task<EventTicketStatusViewModel> GetEventTicketStatus(string organizerId);
        public Task<List<GetAllBookedEventByAdminViewModel>> GetAllBookedEventsByAdmin();
    }
}
