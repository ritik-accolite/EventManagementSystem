using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationServer.Models.ViewModels
{
    public class AddBookedEventViewModel
    {
        [ForeignKey("Event")]
        public int? EventId { get; set; }

        [ForeignKey("Organizer")]
        public string? EventOrganizerId { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}
