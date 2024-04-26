using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationServer.Models
{
    public class BookedEvents
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }

        [ForeignKey("Organizer")]
        public int EventOrganizerId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public DateTime BookingDate { get; set; }

        // Navigation properties
        public virtual Event Event { get; set; }
        public virtual Person Organizer { get; set; }
        public virtual Person User { get; set; }
    }
}
