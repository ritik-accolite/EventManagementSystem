using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationServer.Data;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookedEventController : ControllerBase
    {

        private readonly IAddBookedEventService _addBookedEventService;


        public BookedEventController(IAddBookedEventService addBookedEventService)
        {
            _addBookedEventService = addBookedEventService;
        }

        [HttpGet]
        public async Task<GetAllBookedEventResposeViewModel> GetAllBookedEvents()
        {
            GetAllBookedEventResposeViewModel response = new GetAllBookedEventResposeViewModel();

            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Something Went Wrong. Unable to get booked Events.";
                return response;
            }


            var user = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue("Role");

            if (user == null || role == "User")
            {
                response.Status = 401;
                response.Message = "You are either not logged in or you are not a Organizer.";
                return response;
            }
            try
            {
                response.Status = 200;
                response.Message = "All Events Fetched Successfully";
                response.AllBookedEvents = await _addBookedEventService.GetAllBookedEvents();
                return response;

            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;
                return response;
            }
        }


        [HttpGet("{BookingId:int}")]
        public async Task<GetBookedEventByIdResponseViewModel> GetBookedEventsById(int BookingId)
        {
            GetBookedEventByIdResponseViewModel response = new GetBookedEventByIdResponseViewModel();

            try
            {
                response.Status = 200;
                response.Message = "Booked Event Fetched Successfully";
                response.GetBookedEventById = await _addBookedEventService.GetBookedEventsById(BookingId);

            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;

            }
            return response;
        }


        [HttpDelete("unbookEvent/{bookingId}")]
        public async Task<ResponseViewModel> UnbookEvent(int bookingId)
        {
            ResponseViewModel response = new ResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var user = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue("Role");

            if (user == null || role == "Organizer")
            {
                response.Status = 401;
                response.Message = "You are either not loggedIn or You are not User.";
                return response;
            }
            response = await _addBookedEventService.UnbookEvent(bookingId);
            return response;
        }




        [HttpGet("GetBookedEventsByUser")]
        public async Task<GetAllBookedEventsWithDetailsResponseViewModel> GetBookedEventsByUser()
        {
            GetAllBookedEventsWithDetailsResponseViewModel response;

            var user = User.FindFirstValue(ClaimTypes.Name);

            if (user == null)
            {
                response = new GetAllBookedEventsWithDetailsResponseViewModel();
                response.Status = 401;
                response.Message = "User Not Found";
                return response;
            }

            var id = User.FindFirstValue("Id");
            var bookedEvents = await _addBookedEventService.GetBookedEventsWithDetailsByUser(id);
            response = new GetAllBookedEventsWithDetailsResponseViewModel
            {
                Status = 200,
                Message = "Booked events fetched successfully",
                BookedEvents = bookedEvents
            };
            return response;
        }

        [HttpGet("GetBookedEventsByUserId/{userId}")]
        public async Task<GetAllBookedEventsWithDetailsResponseViewModel> GetBookedEventsByUserId(string userId)
        {
            GetAllBookedEventsWithDetailsResponseViewModel response;
            var id = userId;
            var bookedEvents = await _addBookedEventService.GetBookedEventsWithDetailsByUser(id);
            response = new GetAllBookedEventsWithDetailsResponseViewModel
            {
                Status = 200,
                Message = "Booked events fetched successfully",
                BookedEvents = bookedEvents
            };
            return response;
        }


        [HttpPost]
        [Route("BookEvent")]
        public async Task<ResponseViewModel> BookTickets(AddBookedEventViewModel addBookedEvent)
        {
            ResponseViewModel response = new ResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please enter all the details.";
                return response;
            }

            var user = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue("Role");
            if (user == null || role == "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not logged in or you are not a user.";
                return response;
            }
            try
            {
                response.Status = 200;
                response.Message = "Ticket Booked Successfully";
                response = await _addBookedEventService.BookTickets(addBookedEvent);
            }

            catch (Exception ex)
            {
                response = new ResponseViewModel();
                response.Status = 500;
                response.Message = ex.Message;
            }

            return response;
        }


        [HttpGet("tracktickets/{organizerId}")]
        public async Task<EventTicketStatusResponseViewModel> GetEventTicketStatus(string organizerId)
        {
            EventTicketStatusResponseViewModel response = new EventTicketStatusResponseViewModel();
            try
            {
                response.Status = 200;
                response.Message = "Ticket Status Fetched Successfully";
                response.ticketStatus = await _addBookedEventService.GetEventTicketStatus(organizerId);

            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }


        //ONLY FOR ADMIN ACCESS 
        [HttpGet("admin/getbookingdetails")]
        public async Task<GetAllBookedEventByAdminResponseViewModel> GetBookedEventByAdmin()
        {
            GetAllBookedEventByAdminResponseViewModel response = new GetAllBookedEventByAdminResponseViewModel();

            try
            {
                response.Status = 200;
                response.Message = "All Booked Events Fetched";
                response.AllBookedEvents = await _addBookedEventService.GetAllBookedEventsByAdmin();
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
