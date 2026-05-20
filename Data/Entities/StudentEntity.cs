using PersonalAccount.Models.Students;

namespace PersonalAccount.Data.Entities;

public class StudentEntity
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public List<ConfirmationTokenEntity> ConfirmationTokens { get; set; } = [];

    public void Update(StudentEntity editable)
    {
        FullName = editable.FullName;
        GroupName = editable.GroupName;
        PhotoUrl = editable.PhotoUrl;
    }
}