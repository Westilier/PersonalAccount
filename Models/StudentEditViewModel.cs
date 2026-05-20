using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.Models
{
    public class StudentEditViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "GroupName is required")]
        public string GroupName { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
    }
}
