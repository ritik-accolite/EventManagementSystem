using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationServer.Models.ViewModels
{
    public class AddEventViewModel
    {
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

        //[Required]
        [StringLength(255)]
        public string BannerImage { get; set; }


        // Add a property for the image file
        [Required]
        public IFormFile BannerImageFile { get; set; }
    }
}
