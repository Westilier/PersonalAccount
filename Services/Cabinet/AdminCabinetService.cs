using PersonalAccount.Models;
using PersonalAccount.Repositories;
using PersonalAccount.Types;

namespace PersonalAccount.Services.Cabinet;

public class AdminCabinetService(
    IStudentProfileRepo studentProfileRepo,
    ITeacherProfileRepo teacherProfileRepo,
    IAccountRepo accountRepo,
    IGroupRepo groupRepo,
    ISubjectRepo subjectRepo)
    : IAdminCabinetService
{
    public async Task<List<AccountModel>> GetAllStudentAccountsAsync() =>
        await accountRepo.GetAllByRoleAsync(AccountRoles.Student);

    public async Task<List<StudentProfileModel>> GetAllStudentProfilesAsync() => await studentProfileRepo.GetAllAsync();

    public async Task<List<GroupModel>> GetAllGroupsAsync() => await groupRepo.GetAllAsync();

    public async Task<List<SubjectModel>> GetAllSubjectsAsync() => await subjectRepo.GetAllAsync();

    public async Task AddStudentProfileAsync(string email, string fullName) =>
        await AddProfileAsync(studentProfileRepo, email, fullName);

    public async Task AddTeacherProfileAsync(string email, string fullName) =>
        await AddProfileAsync(teacherProfileRepo, email, fullName);

    public async Task AddGroupAsync(string groupName, string description = "", Uri? imageUrl = null) =>
        await groupRepo.AddAsync(new GroupModel
        {
            Name = groupName,
            Description = description,
            ImageUrl = imageUrl
        });

    private async Task AddProfileAsync<
        TProfileModel>(IProfileRepo<TProfileModel> profileRepo,
        string email,
        string fullName) where TProfileModel : ProfileModel, new()
    {
        var account = await accountRepo.GetByEmailAsync(email);
        if (account == null) return;

        var profile = new TProfileModel
        {
            FullName = fullName,
            AccountId = account.Id
        };

        await profileRepo.AddAsync(profile);
    }
    public async Task AddSubjectAsync(string name)
    {
        await subjectRepo.AddAsync(new SubjectModel
        {
            Name = name,
        });
    }

}