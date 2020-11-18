using System;

namespace AbstractDemo
{
    public class SqlDataAccess : DataAccess
    {
        public override string LoadConnectionString(string name)
        {
            string output = base.LoadConnectionString(name);
            output += " (from SQL Server)";
            return output; 
        }

        public override void LoadData(string sql)
        {
            Console.WriteLine("Loading SQL Server");
        }

        public override void SaveData(string sql)
        {
            Console.WriteLine("Saving SQL Server");
        }
    }
}