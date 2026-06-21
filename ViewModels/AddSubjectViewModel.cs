using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.ViewModels;

public class AddSubjectViewModel
{
    [Required(ErrorMessage = "Введите название дисциплины")]
    public string Name { get; set; } = string.Empty;
}