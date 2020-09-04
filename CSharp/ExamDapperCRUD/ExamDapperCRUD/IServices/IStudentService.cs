using ExamDapperCRUD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExamDapperCRUD.IServices
{
    public interface IStudentService
    {
        Student Save(Student oStudent);
        Task<List<Student>> Gets();
        Student Get(int studentID);
        string Delete(int studentID);
    }
}