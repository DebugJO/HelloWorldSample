void Main()
{
	{
		using DemoResource demo = new();
		demo.DoWork();
		//demo.StartWriting();
	}

	Console.WriteLine("I'm done running Program.cs");
}


public class DemoResource : IDisposable
{
	private bool disposedValue;
	//private StreamWriter _writer;
	//private Excel.Application _excel;

	//public void StartWriting()
	//{
	//	_writer = new StreamWriter("output.txt");
	//  _excel = new Excel.Application();
	//}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				//_writer.Dispose();
				Console.WriteLine("_writer.Dispose()");
			}

			//if (_excel != null)
			//{
			//	_excel.Quit();
			//	Marshal.ReleaseComObject(_excel);
			//	Console.WriteLine("Releasing Excel");
			//}

			disposedValue = true;
		}

		Console.WriteLine("Closing connection via Dispose");
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~DemoResource()
	{
		Dispose(false);
	}

	public void DoWork()
	{
		{
			Console.WriteLine("Opening Connection");
			Console.WriteLine("Doing Work...");
			//throw new Exception("I broke");
			//Console.WriteLine("Closing Connection");
		}
	}
}
