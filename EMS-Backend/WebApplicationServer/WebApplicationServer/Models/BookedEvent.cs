using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationServer.Models
{
    public class BookedEvent
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("Event")]
        public int? EventId { get; set; }

        [ForeignKey("Organizer")]
        public string? EventOrganizerId { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }

        public DateTime BookingDate { get; set; }


        //new
        public int NumberOfTickets { get; set; }

        // Navigation properties
        public virtual Event? Event { get; set; }
        public virtual Person? Organizer { get; set; }
        public virtual Person? User { get; set; }

    }
}
