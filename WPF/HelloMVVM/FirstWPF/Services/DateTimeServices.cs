using System;

namespace FirstWPF.Services
{
    public class DateTimeServices : IDateTimeServices
    {
        public string GetDateTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}