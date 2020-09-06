using System;

namespace LambdaExam
{
    public class UserArgs : EventArgs
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}