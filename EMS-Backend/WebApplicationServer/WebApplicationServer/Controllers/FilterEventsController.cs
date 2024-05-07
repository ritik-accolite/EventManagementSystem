using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterEventsController : ControllerBase
    {
        private readonly IAddEventService _addEventService;


        public FilterEventsController(IAddEventService addEventService)
        {
            _addEventService = addEventService;
  
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
