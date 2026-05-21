using PersonalAccount.Models.Students;

namespace PersonalAccount.Repository
{
    public interface IStudentRepo<T> where T : StudentModel
    {
        public Task<T?> GetByEmailAsync(string email);
        public Task<T?> GetByIdAsync(int id);
        Task UpdateByIdAsync(int id, StudentModel student);
        Task UpdatePasswordHashAsync(int id, string passwordHash);
    }
}
