using System;

namespace AbstractDemo
{
    public class SqliteDataAccess : DataAccess
    {
        public override string LoadConnectionString(string name)
        {
            string output = base.LoadConnectionString(name);
            output += " (from SQLite)";
            return output; 
        }

        public override void LoadData(string sql)
        {
            Console.WriteLine("Loading SQLite");
        }

        public override void SaveData(string sql)
        {
            Console.WriteLine("Saving SQLite");
        }
    }
}