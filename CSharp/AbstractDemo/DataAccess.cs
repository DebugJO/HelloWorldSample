using System;

namespace AbstractDemo
{
    public abstract class DataAccess
    {
        public virtual string LoadConnectionString(string name)
        {
            Console.WriteLine("Load Connection String");
            return "testConnectionoString";
        }

        public abstract void LoadData(string sql);

        public abstract void SaveData(string sql);
    }
}