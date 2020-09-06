using System;

namespace LambdaExam
{
    public class UserProcessor
    {
        public static event EventHandler<UserArgs> UserProcessorEvent;

        public static void ProcessorUser(object sender, UserArgs args)
        {
            UserArgs _args = new UserArgs();

            _args.Name = args.Name;
            _args.Age = args.Age;

            UserProcessorEvent?.Invoke(sender, _args);
        }
    }
}