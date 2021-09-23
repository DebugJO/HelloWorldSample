using System.Collections.Generic;
using ExamHelloDI.Services;

namespace ExamHelloDI
{
    public partial class MainWindow
    {
        private readonly IDateTimeServices _dateTimeServices;

        public MainWindow(IDateTimeServices dateTimeServices)
        {
            InitializeComponent();

            _dateTimeServices = dateTimeServices;
            DataContext = this;
        }

        public string DateTime => _dateTimeServices.GetDateTimeString();
        // set { } Mode=OneWay
    }

    // Employee Model
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Item> ListItem { get; set; } = new()
        {
            new Item { Data = 100 },
            new Item { Data = 120 },
            new Item { Data = 150 }
        };
    }

    public class Item
    {
        public int Data { get; set; }
    }
}