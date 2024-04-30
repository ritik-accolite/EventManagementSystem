using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationServer.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(255)]
        public string EventName { get; set; }

        [Required]
        [StringLength(100)]
        public string EventCategory { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ChiefGuest { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        //[Required]
        //[DataType(DataType.Time)]
        //public TimeSpan EventTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Event_Time { get; set; }
        
        [Required]
        public string EventLocation { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TicketPrice { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        [StringLength(255)]
        public string BannerImage { get; set; }

        [Required]
        public string? EventOrganizerId { get; set; }

        [ForeignKey("EventOrganizerId")]
        public Person? Organizer { get; set; }

    }
}
