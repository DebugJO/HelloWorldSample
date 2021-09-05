using System;

namespace ExamHelloDI.Services
{
    public class DateTimeServices : IDateTimeServices
    {
        public string GetDateTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}