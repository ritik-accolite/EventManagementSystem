using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System.Security.Claims;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendETicketsController : ControllerBase
    {
            private readonly IGetAllPerson _getAllPerson;
            private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
            private readonly UserManager<Person> _userManager;
            private readonly SignInManager<Person> _signInManager;
            private readonly IAddBookedEventService _addBookedEventService;
            private readonly ApplicationDbContext _context;
        private readonly IAddEventService _addEventService;


        public SendETicketsController(IAddEventService addEventService ,ApplicationDbContext context,IAddBookedEventService addBookedEventService,UserManager<Person> userManager, SignInManager<Person> signInManager, IGetAllPerson getAllPerson, ISendRegisterSuccessMailService sendRegisterSuccessMailService)
            {
                _getAllPerson = getAllPerson;
                _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
                _userManager = userManager;
                _signInManager = signInManager;
                _addBookedEventService = addBookedEventService;
                _context = context;
            _addEventService = addEventService;

            }

        [HttpGet("generateticket")]
        public async Task<IActionResult> GenerateETicket(int bookingId)
            {
            //    var bookedEventDetails = await _context.BookedEvents
            //.Where(be => be.BookingId == bookingId)
            //.Include(be => be.Event)
            //.Include(be => be.User)
            //.FirstOrDefaultAsync();

            var bookedEventDetails = await _context.BookedEvents
        .Where(be => be.BookingId == bookingId)
        .Include(be => be.Event)
            .ThenInclude(e => e.Organizer)
        .Include(be => be.User)
        .FirstOrDefaultAsync();

            if (bookedEventDetails == null)
            {
                return NotFound(); // Handle case where booking ID does not exist
            }

            //if (bookedEventDetails == null || bookedEventDetails.User == null)
            //{
            //    return NotFound(); // Handle case where booking ID does not exist or user is not found
            //}


            // Check if bookedEventDetails.User is not null before accessing its properties
            //string userName = $"{bookedEventDetails.User.FirstName} {bookedEventDetails.User.LastName}";



            string htmlContent = $@"
        <h1>Booking ID: {bookingId}</h1>
        <h1>E-Ticket for {bookedEventDetails.Event.EventName}</h1>
        <p><strong>Event Name:</strong> {bookedEventDetails.Event.EventName}</p>
        <p><strong>Event Date:</strong> {bookedEventDetails.Event.EventDate}</p>
        <p><strong>Event Location:</strong> {bookedEventDetails.Event.EventLocation}</p>
        <p><strong>Booking Date:</strong> {bookedEventDetails.BookingDate}</p>
        <p><strong>User Name:</strong> {bookedEventDetails.User?.FirstName} {bookedEventDetails.User?.LastName}</p>
        <p><strong>Ticket Price:</strong> {bookedEventDetails.Event.TicketPrice}</p>
        <p><strong>Event Time:</strong> {bookedEventDetails.Event.Event_Time}</p>
        <p><strong>Total Tickets Booked:</strong> {bookedEventDetails.NumberOfTickets}</p>
        <p><strong>Total Price:</strong> {bookedEventDetails.NumberOfTickets * bookedEventDetails.Event.TicketPrice}</p>
    ";

            var document = new PdfDocument();
                //string HtmlContent = "<h1>Welcome To Event Hub</h1>";
                PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);
                byte[]? response = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    document.Save(ms);
                    response = ms.ToArray();
                }
                //string Filename = "ETicket_" + ticketNo + ".pdf";
                string filename = $"ETicket_{bookingId}.pdf";
                return File(response, "application/pdf", filename);
            }
        }
    }