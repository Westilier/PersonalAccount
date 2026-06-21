using PersonalAccount.Data.Entities;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public interface IStudentProfileRepo : IProfileRepo<StudentProfileModel>
{
    Task UpdateGroupByAccountIdAsync(int accountId, int groupId);
    Task<int> CountByGroupIdAsync(int groupId);
}