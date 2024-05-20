using System.ComponentModel.DataAnnotations;

namespace WebApplicationServer.Models.ViewModels
{
    public class ReviewViewModel
    {
        [Required]
        public string Description { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public bool IsReported { get; set; }
    }
}
