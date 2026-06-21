using PersonalAccount.Models;
using PersonalAccount.Repositories;

namespace PersonalAccount.Services.Cabinet;

public interface IAdminCabinetService
{
    Task<List<AccountModel>> GetAllStudentAccountsAsync();
    Task<List<StudentProfileModel>> GetAllStudentProfilesAsync();
    Task<List<GroupModel>> GetAllGroupsAsync();
    Task<List<SubjectModel>> GetAllSubjectsAsync();
    Task AddStudentProfileAsync(string email, string fullName);
    Task AddTeacherProfileAsync(string email, string fullName);
    Task AddGroupAsync(string groupName, string description = "", Uri? imageUrl = null);
    Task AddSubjectAsync(string name);
    Task ChangeStudentGroupAsync(int studentAccountId, int groupId);
    Task DeleteGroupAsync(int groupId);
    Task DeleteSubjectAsync(int subjectId);
}