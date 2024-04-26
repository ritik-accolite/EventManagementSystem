using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplicationServer.Models;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookedEventController : ControllerBase
    {
        private string connectionString = "Server=tcp:ems-server.database.windows.net,1433;Initial Catalog=emsdatabase;Persist Security Info=False;User ID=ajaykarode;Password=Emspassword@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // POST: api/BookedEvent/BookEvent
        [HttpPost]
        [Route("BookEvent")]
        public IActionResult BookEvent(int eventId, int organizerId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "INSERT INTO BookedEvents (EventId, EventOrganizerId, UserId, BookingDate) VALUES (@EventId, @EventOrganizerId, @UserId, @BookingDate)";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@EventId", eventId);
                command.Parameters.AddWithValue("@EventOrganizerId", organizerId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@BookingDate", DateTime.Now); // Set the current date/time as the booking date
                connection.Open();
                command.ExecuteNonQuery();
            }
            return Ok();
        }


        // GET: api/BookedEvent/GetAllBookedEvents
        [HttpGet]
        [Route("GetAllBookedEvents")]
        public IActionResult GetAllBookedEvents()
        {
            List<BookedEvents> bookedEvents = new List<BookedEvents>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT * FROM BookedEvents";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BookedEvents bookedEvent = new BookedEvents
                    {
                        BookingId = (int)reader["BookingId"],
                        EventId = (int)reader["EventId"],
                        EventOrganizerId = (int)reader["EventOrganizerId"],
                        UserId = (int)reader["UserId"],
                        BookingDate = (DateTime)reader["BookingDate"]
                    };
                    bookedEvents.Add(bookedEvent);
                }
                reader.Close();
            }

            return Ok(bookedEvents);
        }
    }
}
