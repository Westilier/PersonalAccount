using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.ViewModels;

public class AddGroupViewModel
{
    [Required(ErrorMessage = "Введите название группы")]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;

}
