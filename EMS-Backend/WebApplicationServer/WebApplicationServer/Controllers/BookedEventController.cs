﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Models.ResponseModels;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookedEventController : ControllerBase
    {
        private string connectionString = "Server=tcp:ems-server.database.windows.net,1433;Initial Catalog=emsdatabase;Persist Security Info=False;User ID=ajaykarode;Password=Emspassword@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private readonly IAddBookedEventService _addBookedEventService;
        private readonly ISendRegisterSuccessMailService _sendRegisterSuccessMailService;
        private readonly ApplicationDbContext _context;


        public BookedEventController(IAddBookedEventService addBookedEventService, ISendRegisterSuccessMailService sendRegisterSuccessMailService, ApplicationDbContext context)
        {
            _addBookedEventService = addBookedEventService;
            _sendRegisterSuccessMailService = sendRegisterSuccessMailService;
            _context = context;
        }

        [HttpGet]
        public async Task<GetAllBookedEventResposeViewModel> GetAllBookedEvents()
        {
            GetAllBookedEventResposeViewModel response = new GetAllBookedEventResposeViewModel();

            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Something Went Wrong. Unable to get booked Events.";
                return response;
            }
            //var role = User.FindFirstValue(ClaimTypes.Role);
            //var user = await _userManager.(User);


            var user = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue("Role");

            if (user == null || role == "User")
            {
                response.Status = 401;
                response.Message = "You are either not logged in or you are not a Organizer.";
                return response;
            }
            try
            {
                response.Status = 200;
                response.Message = "All Events Fetched Successfully";
                response.AllBookedEvents = await _addBookedEventService.GetAllBookedEvents();
                return response;

            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;
                return response;
            }
        }


        [HttpGet("{BookingId:int}")]
        public async Task<GetBookedEventByIdResponseViewModel> GetBookedEventsById(int BookingId)
        {
            GetBookedEventByIdResponseViewModel response = new GetBookedEventByIdResponseViewModel();

            try
            {
                response.Status = 200;
                response.Message = "Booked Event Fetched Successfully";
                response.GetBookedEventById = await _addBookedEventService.GetBookedEventsById(BookingId);
                //return response;

            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;
                
            }
            return response;


        }


        [HttpDelete("unbookEvent/{bookingId}")]
        public async Task<ResponseViewModel> UnbookEvent(int bookingId)
        {
            ResponseViewModel response = new ResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var user = User.FindFirstValue(ClaimTypes.Name);           
            var role = User.FindFirstValue("Role");

            //var role = User.FindFirstValue(ClaimTypes.Role);
            //var user = await _userManager.GetUserAsync(User);

            if (user == null || role == "Organizer")
            {
                response.Status = 401;
                response.Message = "You are either not loggedIn or You are not User.";
                return response;
            }
            response = await _addBookedEventService.UnbookEvent(bookingId);
            return response;
        }




        [HttpGet("GetBookedEventsByUser")]
        public async Task<GetAllBookedEventsWithDetailsResponseViewModel> GetBookedEventsByUser()
        {
            GetAllBookedEventsWithDetailsResponseViewModel response;
            //var user = await _userManager.GetUserAsync(User);
            var user = User.FindFirstValue(ClaimTypes.Name);

            if (user == null)
            {
                response = new GetAllBookedEventsWithDetailsResponseViewModel();
                response.Status = 401;
                response.Message = "User Not Found";
                return response;
            }

            var id = User.FindFirstValue("Id");
            var bookedEvents = await _addBookedEventService.GetBookedEventsWithDetailsByUser(id);
            response = new GetAllBookedEventsWithDetailsResponseViewModel
            {
                Status = 200,
                Message = "Booked events fetched successfully",
                BookedEvents = bookedEvents
            };
            return response;
        }



        //[HttpPost]
        //[Route("BookEvent")]
        //public async Task<ResponseViewModel> BookTickets(AddBookedEventViewModel addBookedEvent)
        //{
        //    ResponseViewModel response;
        //    if (!ModelState.IsValid)
        //    {
        //        response = new ResponseViewModel();
        //        response.Status = 422;
        //        response.Message = "Please enter all the details.";
        //        return response;
        //    }
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null || user.Role == "Organizer")
        //    {
        //        response = new ResponseViewModel();
        //        response.Status = 401;
        //        response.Message = "You are either not logged in or you are not a user.";
        //        return response;
        //    }
        //    try
        //    {
        //        //var response = await _addBookedEventService.BookTickets(addBookedEvent);
        //        response = await _addBookedEventService.BookTickets(addBookedEvent);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response = new ResponseViewModel();
        //        response.Status = 500;
        //        response.Message = ex.Message;
        //        return response;
        //    }
        //}


        [HttpPost]
        [Route("BookEvent")]
        public async Task<ResponseViewModel> BookTickets(AddBookedEventViewModel addBookedEvent)
        {
            ResponseViewModel response = new ResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please enter all the details.";
                return response;
            }

            var user = User.FindFirstValue(ClaimTypes.Name);
            //var role = User.FindFirstValue(ClaimTypes.Role);
            var role = User.FindFirstValue("Role");
            //var user = await _userManager.GetUserAsync(User);
            if (user == null || role == "Organizer")
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not logged in or you are not a user.";
                return response;
            }
            try
            {
                response.Status = 200;
                response.Message = "Ticket Booked Successfully";
                response = await _addBookedEventService.BookTickets(addBookedEvent);
                //return response;
            }
            catch (Exception ex)
            {
                response = new ResponseViewModel();
                response.Status = 500;
                response.Message = ex.Message;
                //return response;
            }
            return response;
        }


        [HttpGet("tracktickets/{organizerId}")]
        public async Task<EventTicketStatusResponseViewModel> GetEventTicketStatus(string organizerId)
        {
            EventTicketStatusResponseViewModel response = new EventTicketStatusResponseViewModel();
            try
            {
                response.Status = 200;
                response.Message = "Ticket Status Fetched Successfully";
                response.ticketStatus = await _addBookedEventService.GetEventTicketStatus(organizerId);               

            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;
                //return response;
            }
            return response;
        }


        //ONLY FOR ADMIN ACCESS 
        [HttpGet("admin/getbookingdetails")]
       public async Task<GetAllBookedEventByAdminResponseViewModel> GetBookedEventByAdmin()
        {
            GetAllBookedEventByAdminResponseViewModel response = new GetAllBookedEventByAdminResponseViewModel();

            try
            {
                response.Status = 200;
                response.Message = "All Booked Events Fetched";
                response.AllBookedEvents = await _addBookedEventService.GetAllBookedEventsByAdmin();
                //return response;
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;
                //return response;
            }
            return response; 
        }
    }
}












//[HttpPost]
//[Route("addBookedEvent")]
//public async Task<ResponseViewModel> AddBookedEvent(AddBookedEventViewModel addBookedEvent)
//{
//    ResponseViewModel response;
//    if (!ModelState.IsValid)
//    {
//        response = new ResponseViewModel();
//        response.Status = 422;
//        response.Message = "Please Enter all the details.";
//        return response;
//    }
//    var user = await _userManager.GetUserAsync(User);
//    if (user == null || user.Role == "Organizer")
//    {
//        response = new ResponseViewModel();
//        response.Status = 401;
//        response.Message = "You are either not loggedIn or You are not User.";
//        return response;
//    }
//    response = await _addBookedEventService.AddBookedEvent(addBookedEvent, user.Id);

//    return response;
//}





//[HttpGet("GetBookedEventsByUser")]
//public async Task<ActionResult<GetAllBookedEventsWithDetailsResponseViewModel>> GetBookedEventsByUser()
//{
//    var user = await _userManager.GetUserAsync(User);
//    if (user == null)
//    {
//        return BadRequest("User not found");
//    }

//    var bookedEvents = await _addBookedEventService.GetBookedEventsWithDetailsByUser(user.Id);
//    var response = new GetAllBookedEventsWithDetailsResponseViewModel
//    {
//        Status = 200,
//        Message = "Booked events fetched successfully",
//        BookedEvents = bookedEvents
//    };
//    return Ok(response);
//}







//[HttpPost]
//[Route("BookEvent")]
//public async Task<IActionResult> BookTickets(AddBookedEventViewModel addBookedEvent)
//{
//    try
//    {
//        var response = await _addBookedEventService.BookTickets(addBookedEvent);
//        return StatusCode(response.Status, response);
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, new { Message = ex.Message });
//    }
//}





//[HttpDelete("{BookingId:int}")]
//public async Task<ResponseViewModel> DeleteBookedEvent(int BookingId)
//{
//    ResponseViewModel response;

//    //DOUBT -- IF USER WANT TO UNBOOK THE EVENT AFTER BOOKING.

//    //var user = await _userManager.GetUserAsync(User);
//    //if (user == null || user.Role != "Organizer")
//    //{
//    //    response = new ResponseViewModel();
//    //    response.Status = 401;
//    //    response.Message = "You are either not loggedIn or You are not Organizer.";
//    //    return response;
//    //}

//    response = await _addBookedEventService.DeleteBookedEvent(BookingId);
//    return response;
//}


//[HttpPost]
//[Route("bookEvent")]
//public async Task<ResponseViewModel> BookEvent(AddBookedEventViewModel addBookedEvent)
//{
//    ResponseViewModel response;
//    if (!ModelState.IsValid)
//    {
//        response = new ResponseViewModel();
//        response.Status = 422;
//        response.Message = "Please Enter all the details.";
//        return response;
//    }
//    var user = await _userManager.GetUserAsync(User);
//    if (user == null || user.Role == "Organizer")
//    {
//        response = new ResponseViewModel();
//        response.Status = 401;
//        response.Message = "You are either not loggedIn or You are not User.";
//        return response;
//    }
//    response = await _addBookedEventService.BookEvent(addBookedEvent);
//    return response;
//}




//[HttpGet]
//public async Task<GetAllBookedEventResposeViewModel> GetAllBookedEvents()
//{
//    var bookedevents = await _addBookedEventService.GetAllBookedEvents();
//    return bookedevents;
//}