using PersonalAccount.Models.Students;
using PersonalAccount.Repository;

namespace PersonalAccount.Services
{
    public class StudentService(IStudentRepo<StudentModel> students) : IStudentService
    {

        public async Task UpdateByIdAsync(int id, StudentModel student)
        {
            await students.UpdateByIdAsync(id, student);
        }
    }
}
