using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Services;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendETicketsController : ControllerBase
    {
        private readonly IAddBookedEventService _addBookedEventService;
        public SendETicketsController(IAddBookedEventService addBookedEventService)
        {
            _addBookedEventService = addBookedEventService;
        }


        [HttpGet("generateticket/{bookingId}")]
        public async Task<GetETicketByBookingIdResponseViewModel> GenerateETicket(int bookingId)
        {
            var response = new GetETicketByBookingIdResponseViewModel();
            try
            {
                response.Status = 200;
                response.Message = "Data Fetched Successfully";
                response.ETicket = await _addBookedEventService.GenerateETicketAsync(bookingId);
                return response;
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"An error occurred: {ex.Message}";
                return response;
            }
        }
    }
}