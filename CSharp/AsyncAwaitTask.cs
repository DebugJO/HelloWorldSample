class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Start............................");
        Console.WriteLine($"{ Calculate1()}");
        Console.WriteLine("---------------------------------");
        Console.WriteLine($"{ await Calculate2()}");
        Console.WriteLine("---------------------------------");
        await ExeOrder();
    }

    private static string GetOrder()
    {
        Thread.Sleep(3000);
        return "GetOrder OK............";
    }

    private static async Task ExeOrder()
    {
        Task<string> t = new(GetOrder);
        t.Start();
        Console.WriteLine("Waiting....");
        var message = await t;
        Console.WriteLine(message);
    }

    static string Calculate1()
    {
        var task1 = Task.Run(() => { return GetCalulate1(); });
        var task2 = Task.Run(() => GetCalulate2());

        var awaiter1 = task1.GetAwaiter();
        var awaiter2 = task2.GetAwaiter();

        var result1 = awaiter1.GetResult();
        var result2 = awaiter2.GetResult();

        Task.WaitAll(task1, task2);

        var ret1 = result1;
        var ret2 = result2;

        Console.WriteLine($"Result : {ret1 + ret2}");
        return "Calculate 1 OK...........";
    }

    static async Task<string> Calculate2()
    {
        var task1 = Task.Run(() => { return GetCalulate1(); });
        var task2 = Task.Run(() => { return GetCalulate2(); });

        var awaiter1 = task1.GetAwaiter();
        var awaiter2 = task2.GetAwaiter();

        var result1 = awaiter1.GetResult();
        var result2 = awaiter2.GetResult();

        await Task.WhenAll(task1, task2);

        var ret1 = result1;
        var ret2 = result2;

        Console.WriteLine($"Result : {ret1 + ret2}");
        return "Calculate 2 OK...........";
    }

    static int GetCalulate1()
    {
        Thread.Sleep(3000);
        Console.WriteLine("Calulate1..........1");
        return 100;
    }

    static int GetCalulate2()
    {
        Thread.Sleep(3000);
        Console.WriteLine("Calulate2..........2");
        return 200;
    }
}
