using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        public EventController(IAddEventService addEventService)
        {
            _addEventService = addEventService;
        }


        [HttpGet]
        public async Task<GetAllEventResponseViewModel> GetAllEvents()
        {
            GetAllEventResponseViewModel response = new GetAllEventResponseViewModel();
            try
            {
                response.Status = 200;
                response.Message = "All Events Fetched Successfully";
                response.AllEvents = await _addEventService.GetAllEvents();
                return response;
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"An error occurred: {ex.Message}";
                return response;
            }
        }


        [HttpGet("{EventId:int}")]
        public async Task<GetEVentByIdResposeViewModel> GetEventById(int EventId)
        {

            GetEVentByIdResposeViewModel response = new GetEVentByIdResposeViewModel();
            try
            {
                response.Status = 200;
                response.Message = "Event Fetched Successfully";
                var events = await _addEventService.GetEventById(EventId);
                return events;
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"An error occurred: {ex.Message}";
                return response;
            }
        }


        [HttpPost]
        [Route("addEvent")]
        public async Task<ResponseViewModel> AddEvent([FromForm] AddEventViewModel addEvent)
        {
            ResponseViewModel response;
            try
            {
                if (!ModelState.IsValid)
                {
                    response = new ResponseViewModel();
                    response.Status = 422;
                    response.Message = "Please Enter all the details.";
                    return response;
                }

                var organizer = User.FindFirstValue(ClaimTypes.Name);
                var Id = User.FindFirstValue("Id");
                var role = User.FindFirstValue("Role");
                if (organizer == null || role != "Organizer")
                {
                    response = new ResponseViewModel();
                    response.Status = 401;
                    response.Message = "You are either not logged in or You are not an organizer.";
                    return response;
                }

                response = await _addEventService.AddEvent(addEvent, Id, addEvent.BannerImageFile);
                return response;
            }
            catch (Exception ex)
            {
                response = new ResponseViewModel();
                response.Status = 500;
                response.Message = $"Error adding event: {ex.Message}";
                return response;
            }
        }


        [HttpDelete("{EventId:int}")]
        public async Task<ResponseViewModel> DeleteEvent(int EventId)
        {
            ResponseViewModel response;
            try
            {
                var organizer = User.FindFirstValue(ClaimTypes.Name);
                var role = User.FindFirstValue("Role");

                if (organizer == null || role != "Organizer")
                {
                    response = new ResponseViewModel();
                    response.Status = 401;
                    response.Message = "You are either not logged in or You are not an organizer.";
                    return response;
                }

                response = await _addEventService.DeleteEvent(EventId);
                return response;
            }
            catch (Exception ex)
            {
                response = new ResponseViewModel();
                response.Status = 500;
                response.Message = $"Error deleting event: {ex.Message}";
                return response;
            }
        }


        [HttpPut("updateEvent/{eventId}")]
        public async Task<ResponseViewModel> UpdateEvent(int eventId, UpdateEventViewModel updateEvent)
        {
            ResponseViewModel response;
            if (!ModelState.IsValid)
            {
                response = new ResponseViewModel();
                response.Status = 422;
                response.Message = "Please provide valid event details.";
                return response;
            }

            var organizer = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue("Role");
            var organizerId = User.FindFirstValue("Id");

            if (organizer == null || role != "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not logged in or not an organizer.";
                return response;
            }

            response = await _addEventService.UpdateEvent(eventId, updateEvent, organizerId);
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("myevents")]
        public async Task<OrganizerCreatedEventResponseViewModel> GetOrganizerCreatedEvents()
        {
            OrganizerCreatedEventResponseViewModel response = new OrganizerCreatedEventResponseViewModel();
            try
            {
                var organizerId = User.FindFirst("id")?.Value;

                response.Status = 200;
                response.Message = "All Events Fetched";
                response.AllEvents = await _addEventService.GetOrganizerCreatedEvents(organizerId);
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error fetching organizer created events: {ex.Message}";
                response.AllEvents = new List<OrganizerCreatedEventViewModel>();
            }
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("myevents/{organizerId}")]
        public async Task<OrganizerCreatedEventResponseViewModel> GetOrganizerCreatedEventsById(string organizerId)
        {
            OrganizerCreatedEventResponseViewModel response = new OrganizerCreatedEventResponseViewModel();
            try
            {
                response.Status = 200;
                response.Message = "All Events Fetched";
                response.AllEvents = await _addEventService.GetOrganizerCreatedEvents(organizerId);
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error fetching organizer created events: {ex.Message}";
                response.AllEvents = new List<OrganizerCreatedEventViewModel>();
            }
            return response;
        }


        [HttpGet("eventCategories")]
        public async Task<GetAllCategoriesResponseViewModel> GetUniqueEventCategories()
        {
            GetAllCategoriesResponseViewModel response = new GetAllCategoriesResponseViewModel();

            try
            {
                response.Status = 200;
                response.Message = "All Unique Categories Fetched";
                response.AllEventCategory = await _addEventService.GetUniqueEventCategories();
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error fetching event categories: {ex.Message}";
                response.AllEventCategory = new List<string>();
            }
            return response;
        }


        [HttpGet("eventlocation")]
        public async Task<GetAllCategoriesResponseViewModel> GetUniqueEventLocation()
        {
            GetAllCategoriesResponseViewModel response = new GetAllCategoriesResponseViewModel();

            try
            {
                response.Status = 200;
                response.Message = "All Unique Locations Fetched";
                response.AllEventCategory = await _addEventService.GetUniqueEventLocation();
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error fetching event location: {ex.Message}";
                response.AllEventCategory = new List<string>();
            }
            return response;
        }

        [HttpGet("pastEvents")]
        public async Task<GetPastEventsResponseViewModel> GetPastEvents()
        {
            GetPastEventsResponseViewModel response = new GetPastEventsResponseViewModel();

            try
            {
                response.Status = 200;
                response.Message = "All Past Events Fetched Successfully";
                response.AllEvents = (await _addEventService.GetPastEvents()).ToList();
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error fetching past events: {ex.Message}";
                response.AllEvents = new List<EventViewModel>();
            }
            return response;
        }


        [HttpGet("upcomingEvents")]
        public async Task<GetUpcomingEventsResponseViewModel> GetUpcomingEvents()
        {
            GetUpcomingEventsResponseViewModel response = new GetUpcomingEventsResponseViewModel();

            try
            {
                response.Status = 200;
                response.Message = "All Upcoming Events Fetched Successfully";
                response.AllEvents = (await _addEventService.GetUpcomingEvents()).ToList();
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error fetching upcoming events: {ex.Message}";
                response.AllEvents = new List<EventViewModel>();
            }
            return response;
        }

        [HttpGet("eventuserdetails/{eventId}")]
        public async Task<GetEventWithUserListResponseViewModel> GetEventDetails(int eventId)
        {
            GetEventWithUserListResponseViewModel response = new GetEventWithUserListResponseViewModel();

            try
            {
                response.Status = 200;
                response.Message = "All Details Fetched Successfully";
                response.EventwithUser = await _addEventService.GetEventDetails(eventId);

            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error fetching events: {ex.Message}";

            }
            return response;
        }
    }
}

