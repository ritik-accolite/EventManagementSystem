//using System.ComponentModel.DataAnnotations;

//namespace WebApplicationServer.Models.ViewModels
//{
//    public class ChangePasswordViewModel
//    {
//        [Required]
//        [Display(Name = "Old Password")]
//        public string? OldPassword { get; set; }

//        [Required]
//        [Display(Name = "New Password")]
//        public string? NewPassword { get; set; }

//        [Required]
//        [Compare("NewPassword")]
//        [Display(Name = "Confirm New Password")]
//        public string? ConfirmNewPassword { get; set; }
//    }
//}



using System.ComponentModel.DataAnnotations;

namespace WebApplicationServer.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

