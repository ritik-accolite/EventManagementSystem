using Azure;
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
            if (user == null || user.Role != "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or You are not Orgainser.";
                return response;
            }
            response = await _addEventService.AddEvent(addEvent, user.Id);

            return response;
        }


        [HttpDelete("{EventId:int}")]
        public async Task<ResponseViewModel> DeleteEvent(int EventId)
        {
            ResponseViewModel response;
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Role != "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or You are not Orgainser.";
                return response;
            }

            response = await _addEventService.DeleteEvent(EventId);
            return response;
        }


        [HttpPut("updateEvent/{id}")]
        public async Task<ResponseViewModel> UpdateEvent(int id, UpdateEventViewModel updateEvent, string userId)
        {
            ResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new ResponseViewModel();
                response.Status = 422;
                response.Message = "Please provide valid event details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Role != "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not logged in or not an organizer.";
                return response;
            }

            response = await _addEventService.UpdateEvent(id, updateEvent, user.Id);
            return response;
        }


        [HttpGet("GetEventsByCategory")]
        public async Task<GetEventByAppliedFilterResponseViewModel> GetEventsByCategory(string category)
        {
            var events = await _addEventService.GetEventsByCategory(category);
            return events;

        }


        [HttpGet("GetEventsByLocation")]
        public async Task<GetEventByAppliedFilterResponseViewModel> GetEventsByLocation(string location)
        {
            var events = await _addEventService.GetEventsByLocation(location);
            return events;
        }
    }
}
