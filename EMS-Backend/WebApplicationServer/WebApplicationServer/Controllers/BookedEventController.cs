using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookedEventController : ControllerBase
    {
        private string connectionString = "Server=tcp:ems-server.database.windows.net,1433;Initial Catalog=emsdatabase;Persist Security Info=False;User ID=ajaykarode;Password=Emspassword@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private readonly IAddBookedEventService _addBookedEventService;
        private readonly UserManager<Person> _userManager;

        public BookedEventController(UserManager<Person> userManager, IAddBookedEventService addBookedEventService)
        {
            _addBookedEventService = addBookedEventService;
            _userManager = userManager;
        }



        //[HttpPost]
        //[Route("addBookedEvent")]
        //public async Task<ResponseViewModel> AddBookedEvent(AddBookedEventViewModel addBookedEvent)
        //{
        //    ResponseViewModel response;
        //    if (!ModelState.IsValid)
        //    {
        //        response = new ResponseViewModel();
        //        response.Status = 422;
        //        response.Message = "Please Enter all the details.";
        //        return response;
        //    }
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null || user.Role == "Organizer")
        //    {
        //        response = new ResponseViewModel();
        //        response.Status = 401;
        //        response.Message = "You are either not loggedIn or You are not User.";
        //        return response;
        //    }
        //    response = await _addBookedEventService.AddBookedEvent(addBookedEvent, user.Id);

        //    return response;
        //}


        [HttpGet]
        public async Task<GetAllBookedEventResposeViewModel> GetAllBookedEvents()
        {
            var bookedevents = await _addBookedEventService.GetAllBookedEvents();
            return bookedevents;
        }


        [HttpGet("{BookingId:int}")]
        public async Task<GetBookedEventByIdResponseViewModel> GetBookedEventsById(int BookingId)
        {
            var events = await _addBookedEventService.GetBookedEventsById(BookingId);
            return events;

        }




        //[HttpDelete("{BookingId:int}")]
        //public async Task<ResponseViewModel> DeleteBookedEvent(int BookingId)
        //{
        //    ResponseViewModel response;

        //    //DOUBT -- IF USER WANT TO UNBOOK THE EVENT AFTER BOOKING.

        //    //var user = await _userManager.GetUserAsync(User);
        //    //if (user == null || user.Role != "Organizer")
        //    //{
        //    //    response = new ResponseViewModel();
        //    //    response.Status = 401;
        //    //    response.Message = "You are either not loggedIn or You are not Organizer.";
        //    //    return response;
        //    //}

        //    response = await _addBookedEventService.DeleteBookedEvent(BookingId);
        //    return response;
        //}


        [HttpPost]
        [Route("bookEvent")]
        public async Task<ResponseViewModel> BookEvent(AddBookedEventViewModel addBookedEvent)
        {
            ResponseViewModel response;
            if (!ModelState.IsValid)
            {
                response = new ResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Role == "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or You are not User.";
                return response;
            }
            response = await _addBookedEventService.BookEvent(addBookedEvent);
            return response;
        }



        [HttpDelete("unbookEvent/{bookingId}")]
        public async Task<ResponseViewModel> UnbookEvent(int bookingId)
        {
            ResponseViewModel response;
            if (!ModelState.IsValid)
            {
                response = new ResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Role == "Organizer")
            {
                response = new ResponseViewModel();
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                response = new GetAllBookedEventsWithDetailsResponseViewModel();
                response.Status = 401;
                response.Message = "User Not Found";
                return response;
            }

            var bookedEvents = await _addBookedEventService.GetBookedEventsWithDetailsByUser(user.Id);
            response = new GetAllBookedEventsWithDetailsResponseViewModel
            {
                Status = 200,
                Message = "Booked events fetched successfully",
                BookedEvents = bookedEvents
            };
            return response;
        }



        //[HttpGet("GetBookedEventsByUser")]
        //public async Task<ActionResult<GetAllBookedEventsWithDetailsResponseViewModel>> GetBookedEventsByUser()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return BadRequest("User not found");
        //    }

        //    var bookedEvents = await _addBookedEventService.GetBookedEventsWithDetailsByUser(user.Id);
        //    var response = new GetAllBookedEventsWithDetailsResponseViewModel
        //    {
        //        Status = 200,
        //        Message = "Booked events fetched successfully",
        //        BookedEvents = bookedEvents
        //    };
        //    return Ok(response);
        //}


    }
}

