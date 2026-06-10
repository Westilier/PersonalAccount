namespace PersonalAccount.Models.ViewModels;

public class StudentInfoViewModel
{
    public int AccountId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;
}

public class AdminCabinetViewModel
{
    public List<StudentInfoViewModel> StudentInfos { get; set; } = [];
}

