public class Program
{
	public static void Main()
	{
		object[] arguments = new object[1];
		MethodInfo method1 = typeof(Program).GetMethod(nameof(SampleMethod1));
		method1?.Invoke(null, arguments);
		Console.WriteLine(arguments[0]);

		MethodInfo method2 = typeof(Program).GetMethod(nameof(SampleMethod2));
		var a = method2?.Invoke(new Program(), new[] { "SampleMethod2" });
		Console.WriteLine(a);
	}

	public static void SampleMethod1(out string text)
	{
		text = "Hello " + "SampleMethod1";
	}

	public string SampleMethod2(string text)
	{
		return "Hello " + text;
	}
}
