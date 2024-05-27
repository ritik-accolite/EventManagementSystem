using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationServer.Models;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackTicketDetailsController : ControllerBase
    {

        private readonly IAddEventService _addEventService;

        public TrackTicketDetailsController(IAddEventService addEventService)
        {
            _addEventService = addEventService;
        }

        //[Authorize(Roles = "Organizer")]
        [HttpGet("trackTicketDetails/{eventId}")]
        public async Task<IActionResult> TrackTicketDetails(int eventId)
        {
            var organizerId = User.FindFirstValue("Id");

            var ticketDetails = await _addEventService.GetTicketDetailsForOrganizer(eventId, organizerId);
            return Ok(ticketDetails);
        }
    }
}


