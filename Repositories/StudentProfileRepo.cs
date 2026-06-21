using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public class StudentProfileRepo(
    AppDbContext ctx,
    IMapper<StudentProfileEntity, StudentProfileModel> mapper
) : ProfileRepo<StudentProfileEntity, StudentProfileModel>(ctx, mapper, c => c.StudentProfiles)
    , IStudentProfileRepo
{
    public async Task UpdateGroupByAccountIdAsync(int accountId, int groupId)
    {
        var student = await GetByAccountIdAsync(accountId) ?? throw new KeyNotFoundException();
        var updatedEntity = mapper.ToEntity(new StudentProfileModel() { GroupId = groupId });

        await UpdateByIdAsync(student.Id, entity => entity.GroupId = updatedEntity.GroupId);
    }
}