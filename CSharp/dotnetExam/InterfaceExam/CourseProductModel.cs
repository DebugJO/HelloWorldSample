using System;

namespace InterfaceExam
{
    public class CourseProductModel : IProductModel
    {
        public string Title { get; set; }

        public bool HasOrderBeenCompleted { get; private set; }

        public void ShipItem(CustomerModel customer)
        {
            if (HasOrderBeenCompleted == false)
            {
                Console.WriteLine($"Added th {Title} course to {customer.FirstName}'s account");
                HasOrderBeenCompleted = true;
            }
        }
    }
}