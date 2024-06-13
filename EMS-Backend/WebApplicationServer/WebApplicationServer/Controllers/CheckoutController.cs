using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplicationServer.Services;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Models.ResponseModels;

namespace WebApplicationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly StripeService _stripeService;

        public CheckoutController(StripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("create-checkout-session")]
        public async Task<CheckoutResponseViewModel> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
        {
            var response = new CheckoutResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please enter all the details.";
                return response;
            }

            try
            {
                var session = await _stripeService.CreateCheckoutSessionAsync(request.EventId, request.UserId, request.NumberOfTickets);
                response.Status = 200;
                response.Message = "Checkout session created successfully.";
                response.token = session.Id;
                response.Url = session.Url;
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = ex.Message;
                response.token = null;
                response.Url = null;
            }

            return response;
        }

        [HttpGet("bookingconfirmation/{session_id}")]
        public async Task<ResponseViewModel> BookingConfirmation(string session_id)
        {
            var response = new ResponseViewModel();
            try
            {
                await _stripeService.HandleSuccessfulPaymentAsync(session_id);
                response.Status = 200;
                response.Message = "Payment successful, booking confirmed.";
            }
            catch (Exception ex)
            {
                response.Status = 400;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("cancel")]
        public ResponseViewModel Cancel()
        {
            var response = new ResponseViewModel
            {
                Status = 200,
                Message = "Payment was cancelled."
            };

            return response;
        }
    }

    public class CreateCheckoutSessionRequest
    {
        public int EventId { get; set; }
        public string UserId { get; set; }
        public int NumberOfTickets { get; set; }
    }
}
