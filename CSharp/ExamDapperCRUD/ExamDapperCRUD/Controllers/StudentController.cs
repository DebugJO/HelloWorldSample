using ExamDapperCRUD.IServices;
using ExamDapperCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExamDapperCRUD.Controllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _oStudentService;

        public StudentController(IStudentService oStudentService)
        {
            _oStudentService = oStudentService;
        }

        [Route("/getStudent")]
        [HttpPost]
        public Student GetStudent(Student oStudent)
        {
            var result = _oStudentService.Get(oStudent.StudentID);

            if (result.StudentID > 0)
                return result;
            result.Message = "조회결과가 없습니다";
            return result;
        }

        [Route("/getStudentList")]
        [HttpPost]
        public async Task<IEnumerable<Student>> GetStudentList()
        {
            var result = await _oStudentService.Gets();

            if (result.Count > 0)
            {
                return result;
            }

            result[0].Message = "조회결과가 없습니다";
            return result;
        }

        [Route("/setStudent")]
        [HttpPost]
        public Student SetStudent(Student oStudent)
        {
            return ModelState.IsValid ? _oStudentService.Save(oStudent) : oStudent;
        }

        [Route("/deleteStudent")]
        [HttpPost]
        public string DeleteStudent(Student oStudent)
        {
            return _oStudentService.Delete(oStudent.StudentID);
        }
    }
}