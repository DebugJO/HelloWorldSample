using System;
using System.Collections.Generic;

namespace AbstractDemo
{
    // https://www.youtube.com/watch?v=jRkmPRk5j2E
    class Program
    {
        static void Main(string[] args)
        {
            List<DataAccess> databases = new List<DataAccess>()
            {
                new SqlDataAccess(),
                new SqliteDataAccess()
            };

            foreach (var db in databases)
            {
                Console.WriteLine(db.LoadConnectionString("demo"));
                db.LoadData("select * from table");
                db.SaveData("insert into table");
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}