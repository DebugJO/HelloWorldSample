using System;
using System.Collections.Generic;

namespace InterfaceExam
{
    internal class Program
    {
        private static void Main()
        {
            List<IProductModel> cart = AddSampleData();
            CustomerModel customer = GetCustomer();

            foreach (IProductModel prod in cart)
            {
                prod.ShipItem(customer);

                if (prod is IDigitalProductModel digital)
                {
                    Console.WriteLine($"For the {digital.Title} you have {digital.TotalDownloadsLeft} downloads left.");
                }
            }

            Console.ReadLine();
        }

        private static CustomerModel GetCustomer()
        {
            return new CustomerModel
            {
                FirstName = "길동",
                LastName = "홍",
                City = "Seoul",
                EmailAddress = "gdh@이메일",
                PhoneNumber = "1234-5678-9000"
            };
        }

        private static List<IProductModel> AddSampleData()
        {
            List<IProductModel> output = new List<IProductModel>
            {
                new PhysicalProductModel { Title = "AAA-Product1" },
                new PhysicalProductModel { Title = "BBB-Product2" },
                new PhysicalProductModel { Title = "CCC-Product3" },
                new DigitalProductModel {Title = "ESD-Lecture1"},
                new CourseProductModel {Title = ".NET 5.0 Start to Finish"}
            };

            return output;
        }
    }
}