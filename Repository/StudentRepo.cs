using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Models.Students;
using PersonalAccount.Repository.Mappers;

namespace PersonalAccount.Repository;

public class StudentRepo<T>(AppDbContext context,  IMapper<StudentEntity, T> mapper) : IStudentRepo<T> where T : StudentModel
{
    private DbSet<StudentEntity> Students => context.Students;
    
    public async Task<T?> GetByEmailAsync(string email)
    {
        var entity = await Students
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Email == email);
        return mapper.ToModel(entity);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var entity = await Students.FindAsync(id);
        return mapper.ToModel(entity);
    }

    public async Task UpdateByIdAsync(int id, StudentModel student)
    {
        var findStudent = Students.FirstOrDefault(note => note.Id == id) ?? throw new IndexOutOfRangeException();
        findStudent.Update(new StudentEntity(){
            FullName = student.FullName,
            GroupName = student.GroupName,
            PhotoUrl = student.PhotoUrl?.ToString(),
            });
        await context.SaveChangesAsync();
    }
}