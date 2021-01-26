void Main()
{
    MyLambda l = new MyLambda();
    if (l.myDelegate == null)
	{
		MyDelegate d = new MyDelegate(l, typeof(MyLambda).GetMethod("Main"));
		l.myDelegate = d;
	}
	
	l.myDelegate.Invoke().Dump();
}

public class MyLambda
{
	public MyDelegate myDelegate;
	public int Main() => 123;
}

public class MyDelegate
{
	object _obj;
	MethodInfo _info;

	public MyDelegate(object obj, MethodInfo info)
	{
		_obj = obj;
		_info = info;
	}

	public int Invoke() => (int)_info.Invoke(_obj, null);
}

// [Reference] Raw Coding, "C# Delegates Explained +Func +Action +Closure", https://www.youtube.com/watch?v=KaxNwGA9fiY
