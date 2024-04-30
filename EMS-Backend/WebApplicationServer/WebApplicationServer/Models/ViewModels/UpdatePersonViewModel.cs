using System.ComponentModel.DataAnnotations;

namespace WebApplicationServer.Models.ViewModels
{
    public class UpdatePersonViewModel
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        //[Required]
        //[StringLength(100)]
        //public string Email { get; set; }


    }
}
