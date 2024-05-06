using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationServer.Models;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackTicketDetailsController : ControllerBase
    {

        private readonly IAddEventService _addEventService;
        private readonly UserManager<Person> _userManager;

        public TrackTicketDetailsController(UserManager<Person> userManager, IAddEventService addEventService)
        {
            _addEventService = addEventService;
            _userManager = userManager;
        }

        //[Authorize(Roles = "Organizer")]
        [HttpGet("trackTicketDetails/{eventId}")]
        public async Task<IActionResult> TrackTicketDetails(int eventId)
        {
            var organizerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ticketDetails = await _addEventService.GetTicketDetailsForOrganizer(eventId, organizerId);
            return Ok(ticketDetails);
        }
    }
}
