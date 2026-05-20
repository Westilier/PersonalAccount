using PersonalAccount.Models.Students;

namespace PersonalAccount.Services
{
    public interface IStudentService
    {
        Task UpdateByIdAsync(int id, StudentModel student);
    }
}
