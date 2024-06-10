
using Stripe;
using Stripe.Checkout;
using WebApplicationServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Data;
using Microsoft.AspNetCore.Mvc;
using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Services
{
    public class StripeService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public StripeService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
            StripeConfiguration.ApiKey = _configuration.GetValue<string>("Stripe:SecretKey");
        }

        public async Task<Session> CreateCheckoutSessionAsync(int eventId, string userId, int numberOfTickets)
        {
            var domain = "https://wonderful-river-0937e261e.5.azurestaticapps.net";

            var eventEntity = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventEntity == null)
            {
                throw new Exception("Event not found");
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var options = new SessionCreateOptions
            {
                //SuccessUrl = "http://localhost:4200/user-dash/mybookings",
                SuccessUrl = $"{domain}/api/checkout/bookingconfirmation?session_id={{CHECKOUT_SESSION_ID}}",

                CancelUrl = $"{domain}/api/checkout/cancel",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(eventEntity.TicketPrice * 100),
                            Currency = "inr",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = eventEntity.EventName,
                            },
                        },
                        Quantity = numberOfTickets,
                    },
                },
                Mode = "payment",
                InvoiceCreation = new Stripe.Checkout.SessionInvoiceCreationOptions
                {
                    Enabled = true,
                },
                CustomerEmail = user.Email,
                Metadata = new Dictionary<string, string>
                {
                    { "EventId", eventId.ToString() },
                    { "UserId", userId },
                    { "NumberOfTickets", numberOfTickets.ToString() }
                }
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            
            return session;
        }

        //    public async Task HandleSuccessfulPaymentAsync(string sessionId)
        //    {
        //        var service = new SessionService();
        //        var session = await service.GetAsync(sessionId);

        //        int eventId = int.Parse(session.Metadata["EventId"]);
        //        string userId = session.Metadata["UserId"];
        //        int numberOfTickets = int.Parse(session.Metadata["NumberOfTickets"]);

        //        var eventEntity = await _context.Events.FindAsync(eventId);

        //        if (eventEntity == null)
        //        {
        //            throw new Exception("Event not found");
        //        }

        //        var bookedEvent = new BookedEvent
        //        {
        //            EventId = eventId,
        //            EventOrganizerId = eventEntity.EventOrganizerId,
        //            UserId = userId,
        //            NumberOfTickets = numberOfTickets,
        //            BookingDate = DateTime.Now
        //        };

        //        _context.BookedEvents.Add(bookedEvent);
        //        await _context.SaveChangesAsync();
        //    }



        public async Task<ResponseViewModel> HandleSuccessfulPaymentAsync(string sessionId)
        {
            var response = new ResponseViewModel();

            try
            {
                var service = new SessionService();
                var session = await service.GetAsync(sessionId);

                int eventId = int.Parse(session.Metadata["EventId"]);
                string userId = session.Metadata["UserId"];
                int numberOfTickets = int.Parse(session.Metadata["NumberOfTickets"]);

                // Check if the user has already booked tickets for this event
                var existingBooking = await _context.BookedEvents
                    .FirstOrDefaultAsync(be => be.EventId == eventId && be.UserId == userId);

                if (existingBooking != null)
                {
                    response.Status = 400;
                    response.Message = "You have already booked tickets for this event";
                    return response;
                }

                // Retrieve the event from the database
                var eventEntity = await _context.Events.FindAsync(eventId);

                if (eventEntity == null)
                {
                    response.Status = 404;
                    response.Message = "Event not found";
                    return response;
                }

                // Check if there are enough tickets available for this event
                if (eventEntity.Capacity < numberOfTickets)
                {
                    response.Status = 401;
                    response.Message = "Not enough tickets available for this event";
                    return response;
                }

                // Ensure the user cannot book more than 5 tickets at once
                if (numberOfTickets > 5)
                {
                    response.Status = 402;
                    response.Message = "You cannot book more than 5 tickets at once";
                    return response;
                }

                // Create a new booked event entity
                var bookedEvent = new BookedEvent
                {
                    EventId = eventId,
                    EventOrganizerId = eventEntity.EventOrganizerId,
                    UserId = userId,
                    NumberOfTickets = numberOfTickets,
                    BookingDate = DateTime.Now
                };

                // Add the booked event to the context and save changes
                _context.BookedEvents.Add(bookedEvent);
                eventEntity.Capacity -= numberOfTickets;
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Event booked successfully";
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = $"Error booking event: {ex.Message}";
            }

            return response;
        }

    }
}



