using Dapper;
using ExamDapperCRUD.Common;
using ExamDapperCRUD.IServices;
using ExamDapperCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ExamDapperCRUD.Services
{
    public class StudentService : IStudentService
    {
        private Student _oStudent = new Student();
        private List<Student> _oStudents = new List<Student>();

        private static DynamicParameters SetParameters(Student oStudent, int operatonType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StudentID", oStudent.StudentID);
            parameters.Add("@Name", oStudent.Name);
            parameters.Add("@Roll", oStudent.Roll);
            parameters.Add("@OperationType", operatonType);
            return parameters;
        }


        public Student Save(Student oStudent)
        {
            _oStudent = new Student();
            var operationType = Convert.ToInt32(oStudent.StudentID == 0 ? OperationType.Insert : OperationType.Update);

            try
            {
                using IDbConnection con = new SqlConnection(Global.ConnectionString);

                if (con.State == ConnectionState.Closed) con.Open();

                var oStudents = con.Query<Student>("spStudent", SetParameters(oStudent, operationType), commandType: CommandType.StoredProcedure);

                var enumerable = oStudents as Student[] ?? oStudents.ToArray();
                if (enumerable.Any())
                {
                    _oStudent = enumerable.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                if (_oStudent != null) _oStudent.Message = ex.Message;
            }

            return _oStudent;
        }

        public async Task<List<Student>> Gets()
        {
            _oStudents = new List<Student>();

            using IDbConnection con = new SqlConnection(Global.ConnectionString);

            if (con.State == ConnectionState.Closed) con.Open();

            var oStudents = await con.QueryAsync<Student>("select * from Student");

            if (!oStudents.Any()) return _oStudents;
            return oStudents as List<Student>;
        }

        public Student Get(int studentID)
        {
            _oStudent = new Student();

            using IDbConnection con = new SqlConnection(Global.ConnectionString);

            if (con.State == ConnectionState.Closed) con.Open();

            var oStudents = con.Query<Student>("select * from Student where StudentID =" + studentID).ToList();

            if (oStudents.Count > 0)
            {
                _oStudent = oStudents.SingleOrDefault();
            }

            return _oStudent;
        }

        public string Delete(int studentID)
        {
            var message = "";

            try
            {
                _oStudent = new Student() {StudentID = studentID};

                using IDbConnection con = new SqlConnection(Global.ConnectionString);

                var oStudents = con.Query<Student>("spStudent", SetParameters(_oStudent, (int) OperationType.Delete), commandType: CommandType.StoredProcedure);

                message = "Deleted";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }
    }
}