async Task Main()
{
	"Begin".Dump();

	List<Task> tasks = new() { Job1(), Job2(), Job3() };

	try
	{
		await Task.WhenAll(tasks);
	}
	catch (Exception ex)
	{
		List<Task> tasksThatFailed = tasks.Where(t => t.IsFaulted).ToList();

		foreach (Task task in tasksThatFailed)
		{
			Exception exception = task.Exception?.InnerException!;
			string methodName = exception?.Data["name"]?.ToString() ?? "";
			$"{methodName} ERROR".Dump();
		}
	}

	"End".Dump();
}

async Task Job1()
{
	await Task.Delay(1000);
	"1...".Dump();
}

async Task Job2()
{
	await Task.Delay(1000);
	"2...".Dump();
}

async Task Job3()
{
	try
	{
		await Task.Delay(1000);
		throw new Exception();
	}
	catch (Exception ex)
	{
		ex.Data["name"] = nameof(Job3);
		throw;
	}
}
