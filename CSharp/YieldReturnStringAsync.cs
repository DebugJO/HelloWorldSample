async Task Main()
{
	await ReadString();
}

private async Task ReadString()
{
	IAsyncEnumerable<string> lines = GetLines(101);

	await foreach (string item in lines)
	{
		item.Dump();
	}
}

private static async IAsyncEnumerable<string> GetLines(int countLine = 1)
{
	List<string> list = new();

	for (int i = 0; i < countLine; i++)
	{
		list.Add("테스트:" + i.ToString("D" + (countLine - 1).ToString().Length.ToString()));
	}

	foreach (string item in list)
	{
		await Task.Delay(100);
		yield return item;
	}
}
