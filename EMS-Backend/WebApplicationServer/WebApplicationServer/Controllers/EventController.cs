using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Drawing;
using System.Reflection;
using System;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using WebApplicationServer.Data.Migrations;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services;
using WebApplicationServer.Services.IService;
using static System.Net.Mime.MediaTypeNames;


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
        public async Task<ActionResult<GetAllEventResponseViewModel>> GetAllEvents()
        {
            try
            {
                var events = await _addEventService.GetAllEvents();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GetAllEventResponseViewModel
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}",
                    AllEvents = null
                });
            }
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


            var organizer = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var Id = User.FindFirstValue("Id");
            //var user = await _userManager.GetUserAsync(User);
            if (organizer == null || role != "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or You are not Orgainser.";
                return response;
            }
            response = await _addEventService.AddEvent(addEvent, Id);

            return response;
        }


        [HttpDelete("{EventId:int}")]
        public async Task<ResponseViewModel> DeleteEvent(int EventId)
        {
            ResponseViewModel response;

            var organizer = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);

            //var user = await _userManager.GetUserAsync(User);
            if (organizer == null || role != "Organizer")
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

            var organizer = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var Id = User.FindFirstValue("Id");

            //var user = await _userManager.GetUserAsync(User);
            if (organizer == null || role != "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not logged in or not an organizer.";
                return response;
            }

            response = await _addEventService.UpdateEvent(id, updateEvent, Id);
            return response;
        }


        ////[Authorize(Roles = "Organizer")]
        //[HttpGet("trackTicketDetails/{eventId}")]
        //public async Task<IActionResult> TrackTicketDetails(int eventId)
        //{
        //    var organizerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var ticketDetails = await _addEventService.GetTicketDetailsForOrganizer(eventId, organizerId);
        //    return Ok(ticketDetails);
        //}


        [Authorize(Roles = "Organizer")]
        [HttpGet]
        [Route("myevents")]
        public async Task<IActionResult> GetOrganizerCreatedEvents()
        {
            var organizerId = User.FindFirst("id")?.Value; // Assuming the organizer's ID is stored in a claim named "id"
            var events = await _addEventService.GetOrganizerCreatedEvents(organizerId);
            return Ok(events);
        }

        [HttpGet("eventCategories")]
        public async Task<IActionResult> GetUniqueEventCategories()
        {
            var eventCategories = await _addEventService.GetUniqueEventCategories();
            return Ok(eventCategories);
        }




        [HttpGet("pastEvents")]
        public async Task<IActionResult> GetPastEvents()
        {
            var pastEvents = await _addEventService.GetPastEvents();
            return Ok(pastEvents);
        }


        [HttpGet("upcomingEvents")]
        public async Task<IActionResult> GetUpcomingEvents()
        {
            var upcomingEvents = await _addEventService.GetUpcomingEvents();
            return Ok(upcomingEvents);
        }
    }
}





