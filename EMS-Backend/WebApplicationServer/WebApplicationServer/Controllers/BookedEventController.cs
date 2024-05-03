using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Data;
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
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly ApplicationDbContext _context;


        public BookedEventController(UserManager<Person> userManager, IAddBookedEventService addBookedEventService, ISendRegisterSuccessMailService sendRegisterSuccessMailService, ApplicationDbContext context)
        {
            _addBookedEventService = addBookedEventService;
            _userManager = userManager;
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _context = context;
        }

        [HttpGet]
        public async Task<GetAllBookedEventResposeViewModel> GetAllBookedEvents()
        {
            GetAllBookedEventResposeViewModel response;
            if (!ModelState.IsValid)
            {
                response = new GetAllBookedEventResposeViewModel();
                response.Status = 422;
                response.Message = "Something Went Wrong. Unable to get booked Events.";
                return response;
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Role == "User")
            {
                response = new GetAllBookedEventResposeViewModel();
                response.Status = 401;
                response.Message = "You are either not logged in or you are not a Organizer.";
                return response;
            }
            try
            {
                //var response = await _addBookedEventService.BookTickets(addBookedEvent);
                response = await _addBookedEventService.GetAllBookedEvents();
                return response;
            }
            catch (Exception ex)
            {
                response = new GetAllBookedEventResposeViewModel();
                response.Status = 500;
                response.Message = ex.Message;
                return response;
            }
        }


        [HttpGet("{BookingId:int}")]
        public async Task<GetBookedEventByIdResponseViewModel> GetBookedEventsById(int BookingId)
        {
            var events = await _addBookedEventService.GetBookedEventsById(BookingId);
            return events;

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



        //[HttpPost]
        //[Route("BookEvent")]
        //public async Task<ResponseViewModel> BookTickets(AddBookedEventViewModel addBookedEvent)
        //{
        //    ResponseViewModel response;
        //    if (!ModelState.IsValid)
        //    {
        //        response = new ResponseViewModel();
        //        response.Status = 422;
        //        response.Message = "Please enter all the details.";
        //        return response;
        //    }
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null || user.Role == "Organizer")
        //    {
        //        response = new ResponseViewModel();
        //        response.Status = 401;
        //        response.Message = "You are either not logged in or you are not a user.";
        //        return response;
        //    }
        //    try
        //    {
        //        //var response = await _addBookedEventService.BookTickets(addBookedEvent);
        //        response = await _addBookedEventService.BookTickets(addBookedEvent);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response = new ResponseViewModel();
        //        response.Status = 500;
        //        response.Message = ex.Message;
        //        return response;
        //    }
        //}


        [HttpPost]
        [Route("BookEvent")]
        public async Task<ResponseViewModel> BookTickets(AddBookedEventViewModel addBookedEvent)
        {
            ResponseViewModel response;
            if (!ModelState.IsValid)
            {
                response = new ResponseViewModel();
                response.Status = 422;
                response.Message = "Please enter all the details.";
                return response;
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Role == "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not logged in or you are not a user.";
                return response;
            }
            try
            {
                response = await _addBookedEventService.BookTickets(addBookedEvent);
                return response;
            }
            catch (Exception ex)
            {
                response = new ResponseViewModel();
                response.Status = 500;
                response.Message = ex.Message;
                return response;
            }
        }


        [HttpGet("tracktickets/{organizerId}")]
        public async Task<IActionResult> GetEventTicketStatus(string organizerId)
        {
            var eventTicketStatus = await _addBookedEventService.GetEventTicketStatus(organizerId);
            return Ok(eventTicketStatus);
        }




        [HttpPost("SendEmailNotification")]
        public async Task<ResponseViewModel> SendEmailNotification(int eventId)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var organizer = await _userManager.GetUserAsync(User);
                if (organizer == null || organizer.Role != "Organizer")
                {
                    //response = new ResponseViewModel();
                    response.Status = 401;
                    response.Message = "You are either not logged in or you are not a Organizer.";
                    return response;
                }

                // Get the list of users who have booked the event
                var bookedUsers = await _context.BookedEvents
                    .Where(be => be.EventId == eventId)
                    .Select(be => be.User.Email)
                    .ToListAsync();

                // Send email notification to each booked user
                foreach (var email in bookedUsers)
                {
                    bool emailSent = await _sendRegisterSuccessMailService.SendRegisterSuccessMailAsync(email, "Event booking", "congrats you have an upcoming event");
                    if (!emailSent)
                    {
                        // Handle email sending failure for individual users
                        // You can choose to continue sending emails or break the loop
                        //response = new ResponseViewModel();
                        response.Status = 500;
                        response.Message = "Failed to send Mail";
                        return response;
                    }
                }

                response.Status = 200;
                response.Message = "Email notification sent successfully to all booked users";
                return response;

                //return Ok("Email notification sent successfully to all booked users.");
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = "something went wrong" + ex.Message;
                return response;
                //return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }







    }
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







//[HttpPost]
//[Route("BookEvent")]
//public async Task<IActionResult> BookTickets(AddBookedEventViewModel addBookedEvent)
//{
//    try
//    {
//        var response = await _addBookedEventService.BookTickets(addBookedEvent);
//        return StatusCode(response.Status, response);
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, new { Message = ex.Message });
//    }
//}





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


//[HttpPost]
//[Route("bookEvent")]
//public async Task<ResponseViewModel> BookEvent(AddBookedEventViewModel addBookedEvent)
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
//    response = await _addBookedEventService.BookEvent(addBookedEvent);
//    return response;
//}




//[HttpGet]
//public async Task<GetAllBookedEventResposeViewModel> GetAllBookedEvents()
//{
//    var bookedevents = await _addBookedEventService.GetAllBookedEvents();
//    return bookedevents;
//}