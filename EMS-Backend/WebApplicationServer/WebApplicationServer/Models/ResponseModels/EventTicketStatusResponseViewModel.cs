﻿using WebApplicationServer.Models.ViewModels;

namespace WebApplicationServer.Models.ResponseModels
{
    public class EventTicketStatusResponseViewModel
    {

        public int Status { get; set; }
        public string Message { get; set; }
        public EventTicketStatusViewModel? ticketStatus { get; set; }
    }
}