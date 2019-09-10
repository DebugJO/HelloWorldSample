using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    internal class Program
    {
        static void Main()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            string[] conStrings = { "서울특별시", "대전", "수원시", "부산", "섬" };

            // Aggregate Functions
            Console.WriteLine(numbers.Min());
            Console.WriteLine(numbers.Where(x => x % 2 == 0).Min());
            Console.WriteLine(numbers.Max());
            Console.WriteLine(numbers.Sum());
            Console.WriteLine(numbers.Count(x => x % 2 == 0));
            Console.WriteLine(numbers.Average());
            Console.WriteLine(numbers.Aggregate((a, b) => a * b));
            Console.WriteLine(Enumerable.Range(1, 10).Aggregate((a, b) => a + b));
            Enumerable.Repeat("Hello", 10).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(conStrings.Min(x => x.Length));
            Console.WriteLine(conStrings.First(x => x.Length == 2).ToString());
            Console.WriteLine(conStrings.OrderByDescending(s => s).First(s => s.Contains("시")).ToString());
            Console.WriteLine(conStrings.Aggregate((a, b) => a + ", " + b));
            conStrings.ToList().Where(x => x.Contains("시")).ToList()
                .ForEach(c => Console.WriteLine(c.ToString()));
            Console.WriteLine("-----------------------------------");

            // Restriction Operators
            var result = numbers.ToList()
                .Select((num, index) => new { Numbers = num, Index = index })
                .Where(x => x.Numbers % 2 != 0); //.Select(x => x.Index);
            foreach (var item in result)
            {
                Console.WriteLine($"{item} : {item.Numbers} {item.Index}");
            }

            var resultEmp = Employee.GetAllEmployees()
                .Where(x => x.Salary >= 2000)
                .Select(e => new { e.ID, e.Name, e.Gender, MonthlySalary = e.Salary / 12 });
            foreach (var item in resultEmp)
            {
                Console.WriteLine($"{item.ID} {item.Name} {item.Gender} {item.MonthlySalary}");
            }

            Console.WriteLine("-----------------------------------");

            // SelectMany Operator
            string[] stringArray = { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "0123456789" };
            var resultChar = stringArray.SelectMany(s => s);
            foreach (var c in resultChar)
            {
                Console.WriteLine(c);
            }

            // var sj = from student in Student.GetAllStudents()
            //          from subject in student.Subjects
            //          select subject;
            var subjects = Student.GetAllStudents().SelectMany(s => s.Subjects).Distinct();
            foreach (var item in subjects)
            {
                Console.WriteLine($"{item}");
            }

            var subjects1 = Student.GetAllStudents()
                            .SelectMany(s => s.Subjects, (student, subject) => new { StudentName = student.Name, SubjectName = subject });
            foreach (var item in subjects1)
            {
                Console.WriteLine($"{item.StudentName} {item.SubjectName}");
            }

            Console.WriteLine("-----------------------------------");

            // Ordering Operators
            var r = Employee.GetAllEmployees()
                   .OrderByDescending(s => s.Name)
                   .OrderBy(s => s.Gender)
                   .ThenBy(s => s.Salary)
                   .Reverse();
            foreach (var student in r)
            {
                Console.WriteLine(student.Name);
            }

            Console.WriteLine("-----------------------------------");

            // Partitioning Operators
            var c1 = conStrings.Take(3);
            foreach (var i in c1) Console.WriteLine("c1: " + i);

            var c2 = conStrings.Skip(3);
            foreach (var i in c2) Console.WriteLine("c2: " + i);

            var c3 = conStrings.TakeWhile(s => s.Length >= 2);
            foreach (var i in c3) Console.WriteLine("c3: " + i);

            var c4 = conStrings.SkipWhile(s => s.Length > 2);
            foreach (var i in c4) Console.WriteLine("c4: " + i);

            // pagiing
            //students.Skip((pageNumber - 1) * pageSize).Take(pageSize)

            Console.WriteLine("-----------------------------------");

            // Cast and OfType operators
            // ToList:Cast, ToArray:OfType, ToDictionary:AsEnumerable, ToLookup:AsQueryable
            ArrayList list = new ArrayList { 1, 2, 3, "ABC", "DEF" };
            IEnumerable<int> r1 = list.OfType<int>();
            foreach (int i in r1) Console.WriteLine(i);

            Console.WriteLine("-----------------------------------");

            // GroupBy
            var empGroup = Employee.GetAllEmployees().GroupBy(x => x.Gender);
            foreach (var i in empGroup)
            {
                Console.WriteLine(i.Key + " c: " + i.Count());
                Console.WriteLine(i.Key + " x: " + i.Count(x => x.Gender == "M"));
                Console.WriteLine(i.Key + " m: " + i.Max(x => x.Salary));
                Console.WriteLine(i.Key + " s: " + i.Sum(x => x.Salary));
            }

            Console.WriteLine("-----------------------------------");

            // Group by multiple
            var e1 = Employee.GetAllEmployees()
                     .GroupBy(x => new { x.Department, x.Gender })
                     .OrderBy(g => g.Key.Department).ThenBy(g => g.Key.Gender)
                     .Select(g => new
                     {
                         Dept = g.Key.Department,
                         g.Key.Gender,
                         Employees = g.OrderBy(x => x.Name)
                     });
            foreach (var i in e1)
                Console.WriteLine(i.Dept + "/" + i.Gender + "/" + i.Employees.Count());

            Console.WriteLine("-----------------------------------");

            // Element Operations
            int[] num = { };
            //Console.WriteLine(num.First()); 
            Console.WriteLine(num.FirstOrDefault());

            Console.WriteLine(numbers.First(x => x % 2 == 0));
            Console.WriteLine(numbers.ElementAt(1));
            Console.WriteLine(numbers.Where(x => x == 1).Single()); //Single:sequence
            var r2 = num.DefaultIfEmpty(100);
            foreach (var i in r2) Console.WriteLine(i);

            Console.WriteLine("-----------------------------------");

            // Group Join
            // var query = dept.Join(emp, d => d.DeptNO, e => e.Department.DeptNO, (d, e) => 
            //             new {d.DeptNO, d.DeptName, e.EmpNO, e.EmpName});
            /*
             var result = Employee.GetAllEmployees()
                          .GroupJoin(Department.GetAllDepartments(),
                                     e => e.DepartmnetID
                                     d => d.ID,
                                     (emp, depts) => new {emp, depts})
                          .SelectMany(z => z.depts.DefaultIfEmpty(),
                                      (a, b) => new
                                      {
                                        EmployeeName = a.emp.Name,
                                        DepartmentName = b == null ? "No Department" : b.Name
                                      }
            */
             

            // Set operators
            string[] con = { "KR", "kr", "KO", "KO", "ko" };
            con.Distinct().ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("-----------------------------------");
            con.Distinct(StringComparer.OrdinalIgnoreCase).ToList().ForEach(x => Console.WriteLine(x));

            int[] num1 = { 1, 2, 3, 7 };
            int[] num2 = { 4, 5, 6, 7 };
            num1.Concat(num2).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("-----------------------------------");
            num1.Union(num2).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("-----------------------------------");
            num1.Intersect(num2).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine("-----------------------------------");
            num1.Except(num2).ToList().ForEach(x => Console.WriteLine(x));

            // SequenceEqual Operator
            string[] con1 = {"ABC", "DF"};
            string[] con2 = {"abc", "df"};
            Console.WriteLine(con1.SequenceEqual(con2).ToString());
            Console.WriteLine(con1.SequenceEqual(con2, StringComparer.OrdinalIgnoreCase).ToString());
        }
    }

    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Salary { get; set; }
        public string Department { get; set; }

        public static IList<Employee> GetAllEmployees()
        {
            return new List<Employee>
            {
                new Employee {ID = 101, Name = "A101", Gender = "M", Salary = 2000, Department = "111"},
                new Employee {ID = 102, Name = "A102", Gender = "F", Salary = 3000, Department = "111"},
                new Employee {ID = 103, Name = "A103", Gender = "M", Salary = 1000, Department = "222"},
                new Employee {ID = 104, Name = "A104", Gender = "F", Salary = 5000, Department = "222"},
                new Employee {ID = 105, Name = "A105", Gender = "M", Salary = 4000, Department = "222"},
            };
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public List<string> Subjects { get; set; }

        public static List<Student> GetAllStudents()
        {
            return new List<Student>
            {
                new Student {Name = "B101", Gender = "F", Subjects = new List<string> {"국어", "영어", "수학"}},
                new Student {Name = "B102", Gender = "M", Subjects = new List<string> {"국어", "수학", "체육"}},
                new Student {Name = "B103", Gender = "F", Subjects = new List<string> {"국어", "국사", "음악"}},
                new Student {Name = "B104", Gender = "M", Subjects = new List<string> {"국어", "윤리", "과학"}},
                new Student {Name = "B105", Gender = "F", Subjects = new List<string> {"국어", "수학", "미술"}},
            };
        }
    }
}