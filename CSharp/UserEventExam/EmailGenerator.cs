using System;

namespace LambdaExam
{
    public class EmailGenerator
    {
        public void SendEmailToUser(object sender, UserArgs e)
        {
            if (sender != null && sender is DateTime)
            {
                sender = (DateTime) sender;
            }

            Console.WriteLine($"Email sent to User({e.Name}) and Age({e.Age}) and object({sender?.ToString()??"NULL"})");
        }
    }
}