using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationServer.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public string UserId { get; set; } // Id of the person who is logged in

        public int EventId { get; set; }

        [Required]
        public string Description { get; set; } // Review description

        [Range(1, 5), Required]
        public int Rating { get; set; } // Rating from 1 to 5

        public bool IsReported { get; set; } = false; // Report issue, default value is false

    }
}
