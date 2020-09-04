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

        //Add, Update, Save(insert or update), Delete, Get, GetXxxList, GetXxxListPage

        //[Route("/GetStudent")]
        //[HttpPost]
        //public async Task<IActionResult> GetStudent(Student oStudent)
        //{
        //    var result = await Task.Run(() => _oStudentService.Get(oStudent.StudentID));
        //    if (result.StudentID > 0)
        //        return Ok(result);
        //    result.Message = "조회결과가 없습니다";
        //    return Ok(result);
        //}

        [Route("/GetStudent")]
        [HttpPost]
        public async Task<Student> GetStudent(Student oStudent)
        {
            var result = await Task.Run(() => _oStudentService.Get(oStudent.StudentID));
            if (result.StudentID > 0)
                return result;
            result.Message = "조회결과가 없습니다";
            return result;
        }

        [Route("/GetStudentList")]
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

        [Route("/SaveStudent")]
        [HttpPost]
        public async Task<Student> SaveStudent(Student oStudent)
        {
            return ModelState.IsValid ? await Task.Run(() => _oStudentService.Save(oStudent)) : oStudent;
        }

        [Route("/DeleteStudent")]
        [HttpPost]
        public async Task<string> DeleteStudent(Student oStudent)
        {
            return await Task.Run(() => _oStudentService.Delete(oStudent.StudentID));
        }
    }
}
