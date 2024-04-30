using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services;
using WebApplicationServer.Services.IService;


namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private string connectionString = "Server=tcp:ems-server.database.windows.net,1433;Initial Catalog=emsdatabase;Persist Security Info=False;User ID=ajaykarode;Password=Emspassword@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private readonly IAddEventService _addEventService;
        private readonly UserManager<Person> _userManager;

        public EventController(UserManager<Person> userManager, IAddEventService addEventService)
        {
            _addEventService = addEventService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<GetAllEventResponseViewModel> GetAllEvents()
        {
            var events = await _addEventService.GetAllEvents();
            return events;
        }


        [HttpGet("{EventId:int}")]
        public async Task<GetEVentByIdResposeViewModel> GetEventById(int EventId)
        {
            var events = await _addEventService.GetEventById(EventId);
            return events;

        }



        [HttpPost]
        [Route("addEvent")]
        public async Task<ResponseViewModel> AddEvent(AddEventViewModel addEvent)
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
            if (user == null || user.Role != "Organiser")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or You are not Orgainser.";
                return response;
            }
            response = await _addEventService.AddEvent(addEvent, user.Id);

            return response;
        }



        /*        public IActionResult GetAllEvents()
                {
                    List<Event> events = new List<Event>();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string sqlQuery = "SELECT * FROM Event";
                        SqlCommand command = new SqlCommand(sqlQuery, connection);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Event @event = new Event
                            {
                                EventId = Convert.ToInt32(reader["EventId"]),
                                EventName = Convert.ToString(reader["EventName"]),
                                EventCategory = Convert.ToString(reader["EventCategory"]),
                                Description = Convert.ToString(reader["Description"]),
                                ChiefGuest = Convert.ToString(reader["ChiefGuest"]),
                                EventDate = Convert.ToDateTime(reader["EventDate"]),
                                Event_Time = Convert.ToString(reader["Event_Time"].ToString()),
                                EventLocation = Convert.ToString(reader["EventLocation"]),
                                TicketPrice = Convert.ToDecimal(reader["TicketPrice"]),
                                Capacity = Convert.ToInt32(reader["Capacity"]),
                                BannerImage = Convert.ToString(reader["BannerImage"]),
                                //EventOrganizerId = Convert.ToInt32(reader["EventOrganizerId"])
                            };

                            events.Add(@event);
                        }

                        reader.Close();
                    }

                    return Ok(events);
                }*/
        // {
        //    var events = await _addEventService.GetAllEvents();
        //    return events;
        //}

 


        //[HttpGet("{id}")]
        //public IActionResult GetEvent(int id)
        //{
        //    Event @event = null;

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        string sqlQuery = "SELECT * FROM Event WHERE EventId = @EventId";
        //        SqlCommand command = new SqlCommand(sqlQuery, connection);
        //        command.Parameters.AddWithValue("@EventId", id); // Use the id parameter to identify the event to retrieve
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            @event = new Event
        //            {
        //                EventId = Convert.ToInt32(reader["EventId"]),
        //                EventName = Convert.ToString(reader["EventName"]),
        //                EventCategory = Convert.ToString(reader["EventCategory"]),
        //                Description = Convert.ToString(reader["Description"]),
        //                ChiefGuest = Convert.ToString(reader["ChiefGuest"]),
        //                EventDate = Convert.ToDateTime(reader["EventDate"]),
        //                Event_Time = Convert.ToString(reader["Event_Time"].ToString()),
        //                EventLocation = Convert.ToString(reader["EventLocation"]),
        //                TicketPrice = Convert.ToDecimal(reader["TicketPrice"]),
        //                Capacity = Convert.ToInt32(reader["Capacity"]),
        //                BannerImage = Convert.ToString(reader["BannerImage"]),
        //                //EventOrganizerId = Convert.ToInt32(reader["EventOrganizerId"])
        //            };
        //        }
        //        reader.Close();
        //    }

        //    if (@event == null)
        //    {
        //        return NotFound(); // No event found with the provided id
        //    }

        //    return Ok(@event);
        //}



        [HttpPost]
        public IActionResult PostEvent([FromBody] Event @event)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "INSERT INTO Event (EventName, EventCategory, Description, ChiefGuest, EventDate, Event_Time, EventLocation, TicketPrice, Capacity, BannerImage, EventOrganizerId) VALUES (@EventName, @EventCategory, @Description, @EventDate, @Event_Time, @EventLocation, @TicketPrice, @Capacity, @BannerImage, @EventOrganizerId)";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@EventName", @event.EventName);
                command.Parameters.AddWithValue("@EventCategory", @event.EventCategory);
                command.Parameters.AddWithValue("@Description", @event.Description);
                command.Parameters.AddWithValue("@ChiefGuest", @event.ChiefGuest);
                command.Parameters.AddWithValue("@EventDate", @event.EventDate);
                command.Parameters.AddWithValue("@Event_Time", @event.Event_Time);
                command.Parameters.AddWithValue("@EventLocation", @event.EventLocation);
                command.Parameters.AddWithValue("@TicketPrice", @event.TicketPrice);
                command.Parameters.AddWithValue("@Capacity", @event.Capacity);
                command.Parameters.AddWithValue("@BannerImage", @event.BannerImage);
                command.Parameters.AddWithValue("@EventOrganizerId", @event.EventOrganizerId);
                connection.Open();
                command.ExecuteNonQuery();
            }
            return Ok();
        }



        [HttpPut("{id}")]
        public IActionResult PutEvent(int id, [FromBody] Event @event)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Event SET EventName = @EventName, EventCategory = @EventCategory, Description = @Description, ChiefGuest = @ChiefGuest, EventDate = @EventDate, Event_Time = @Event_Time, EventLocation = @EventLocation, TicketPrice = @TicketPrice, Capacity = @Capacity, BannerImage = @BannerImage, EventOrganizerId = @EventOrganizerId WHERE EventId = @EventId";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@EventId", id);
                command.Parameters.AddWithValue("@EventName", @event.EventName);
                command.Parameters.AddWithValue("@EventCategory", @event.EventCategory);
                command.Parameters.AddWithValue("@Description", @event.Description);
                command.Parameters.AddWithValue("@ChiefGuest", @event.ChiefGuest);
                command.Parameters.AddWithValue("@EventDate", @event.EventDate);
                command.Parameters.AddWithValue("@Event_Time", @event.Event_Time);
                command.Parameters.AddWithValue("@EventLocation", @event.EventLocation);
                command.Parameters.AddWithValue("@TicketPrice", @event.TicketPrice);
                command.Parameters.AddWithValue("@Capacity", @event.Capacity);
                command.Parameters.AddWithValue("@BannerImage", @event.BannerImage);
                command.Parameters.AddWithValue("@EventOrganizerId", @event.EventOrganizerId);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound(); // No rows updated, event not found
                }
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "DELETE FROM Event WHERE EventId = @EventId";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@EventId", id); // Use the id parameter to identify the event to delete
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound(); // No rows deleted, event not found
                }
            }
            return NoContent();
        }
    }
}

