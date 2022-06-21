async Task Main()
{
	await ReadString();
}

private async Task ReadString()
{
	IAsyncEnumerable<string> lines = GetLines();

	await foreach (string item in lines)
	{
		item.Dump();
	}
}

private static async IAsyncEnumerable<string> GetLines()
{
	List<string> list = new();

	for (int i = 0; i < 10; i++)
	{
		list.Add("테스트:" + i.ToString());
	}

	foreach (string item in list)
	{
		await Task.Delay(100);
		yield return item;
	}
}
