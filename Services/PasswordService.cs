using Microsoft.AspNetCore.Identity;
using PersonalAccount.Models.Students;
using PersonalAccount.Repository;
using System.Web.Helpers;

namespace PersonalAccount.Services
{
    public class PasswordService(IStudentRepo<StudentAuthModel> students, IPasswordHasher<StudentAuthModel> hasher) : IPasswordService
    {

        public async Task<bool> ValidatePasswordAsync(int id, string password)
        {
            var student = await students.GetByIdAsync(id);
            if (student is null) return false;

            var result = hasher.VerifyHashedPassword(student, student.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed) return false;
            return true;
        }

        public async Task UpdatePasswordAsync(int id, string password)
        {
            var student = await students.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Студент с ID {id} не найден.");

            var passwordHash = hasher.HashPassword(student, password);
            await students.UpdatePasswordHashAsync(id, passwordHash);
        }
    }
}
