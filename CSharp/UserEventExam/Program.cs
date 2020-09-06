using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace LambdaExam
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            IdCardGenerator id = new IdCardGenerator();
            EmailGenerator eg = new EmailGenerator();

            UserProcessor.UserProcessorEvent += db.SaveToDB;
            UserProcessor.UserProcessorEvent += id.GenerateIdCard;
            UserProcessor.UserProcessorEvent += eg.SendEmailToUser;

            Console.WriteLine("Comands Accepted:");
            Console.WriteLine("1. new : Add new User");
            Console.WriteLine("2. exit : Close the Application");

            while (true)
            {
                Console.Write("\nEnter Command: ");
                var cmdInput = Console.ReadLine();

                if (cmdInput.Equals("exit"))
                    break;
                else if (cmdInput.Equals("new"))
                {
                    Console.Write("\nEnter User Name: ");
                    var name = Console.ReadLine();

                    Console.Write("\nEnter User Age: ");
                    int age = int.TryParse(Console.ReadLine(), out age) ? age : 0;

                    Console.Write("\nEmail sending needed? (y/n) ");
                    var emailNeeded = Console.ReadLine();

                    if (emailNeeded.Equals("n"))
                        UserProcessor.UserProcessorEvent -= eg.SendEmailToUser;

                    // UserProcessor.ProcessorUser(eg, name, age);
                    UserProcessor.ProcessorUser(DateTime.Now.ToLocalTime(), new UserArgs { Name = name, Age = age });
                }
                else
                    Console.WriteLine("Invalid Command!");
            }
        }
    }
}