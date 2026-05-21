using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.Models
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "OldPassword field is required")]
        public string OldPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "NewPassword field is required")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
