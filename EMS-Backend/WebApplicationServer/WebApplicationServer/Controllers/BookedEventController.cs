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



        [HttpPost]
        [Route("addBookedEvent")]
        public async Task<ResponseViewModel> AddBookedEvent(AddBookedEventViewModel addBookedEvent)
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
            response = await _addBookedEventService.AddBookedEvent(addBookedEvent, user.Id);

            return response;
        }



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




        [HttpDelete("{BookingId:int}")]
        public async Task<ResponseViewModel> DeleteBookedEvent(int BookingId)
        {
            ResponseViewModel response;

            //DOUBT -- IF USER WANT TO UNBOOK THE EVENT AFTER BOOKING.

            //var user = await _userManager.GetUserAsync(User);
            //if (user == null || user.Role != "Organizer")
            //{
            //    response = new ResponseViewModel();
            //    response.Status = 401;
            //    response.Message = "You are either not loggedIn or You are not Organizer.";
            //    return response;
            //}

            response = await _addBookedEventService.DeleteBookedEvent(BookingId);
            return response;
        }

    }
}









// POST: api/BookedEvent/BookEvent
//[HttpPost]
//[Route("BookEvent")]
//public IActionResult BookEvent(int eventId, int organizerId, int userId)
//{
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    {
//        string sqlQuery = "INSERT INTO BookedEvents (EventId, EventOrganizerId, UserId, BookingDate) VALUES (@EventId, @EventOrganizerId, @UserId, @BookingDate)";
//        SqlCommand command = new SqlCommand(sqlQuery, connection);
//        command.Parameters.AddWithValue("@EventId", eventId);
//        command.Parameters.AddWithValue("@EventOrganizerId", organizerId);
//        command.Parameters.AddWithValue("@UserId", userId);
//        command.Parameters.AddWithValue("@BookingDate", DateTime.Now); // Set the current date/time as the booking date
//        connection.Open();
//        command.ExecuteNonQuery();
//    }
//    return Ok();
//}


// GET: api/BookedEvent/GetAllBookedEvents
//[HttpGet]
//[Route("GetAllBookedEvents")]
//public IActionResult GetAllBookedEvents()
//{
//    List<BookedEvent> bookedEvents = new List<BookedEvent>();

//    using (SqlConnection connection = new SqlConnection(connectionString))
//    {
//        string sqlQuery = "SELECT * FROM BookedEvents";
//        SqlCommand command = new SqlCommand(sqlQuery, connection);
//        connection.Open();
//        SqlDataReader reader = command.ExecuteReader();

//        while (reader.Read())
//        {
//            BookedEvent bookedEvent = new BookedEvent
//            {
//                BookingId = (int)reader["BookingId"],
//                EventId = (int)reader["EventId"],
//                //EventOrganizerId = (int)reader["EventOrganizerId"],
//                //UserId = (int)reader["UserId"],
//                BookingDate = (DateTime)reader["BookingDate"]
//            };
//            bookedEvents.Add(bookedEvent);
//        }
//        reader.Close();
//    }
//    return Ok(bookedEvents);
//}