### C#, Delphi 에서 Lambda, LINQ, 익명메소드 사용

##### C# 에서 Array, List, Generic, LINQ, Lambda 예제
```
class Program
{
	static void Main(string[] args)
	{
		var numberList = Enumerable.Range(0, 10).ToList();
		List<int> numbers = new List<int>();
		numberList.ForEach(i => numbers.Add(i));
		// List<int> numbers = new List<int>(){0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
	
		// 1. List<T> Class 사용
		List<int> mc1 = new List<int>();
		foreach (int num in numbers)
		{
			if (num % 2 == 0) { mc1.Add(num); }
		}
		foreach (int m1 in mc1) { Console.WriteLine(m1); }

		// 2. LINQ Query Expressions 사용
		var mc2 = from num in numbers
				where num % 2 == 0
				select num;
		foreach (int m2 in mc2) { Console.WriteLine(m2); }

		// 3. Lambda Expressions 사용
		foreach (int m3 in numbers.Where(n => n % 2 == 0))
		{
			Console.WriteLine(m3);
		}
	}
}
```

##### Delphi 에서 익명 메소스 사용하기
```
type
	TForm1 = class(TForm)
	end;

	TSimpleProcedure = reference to procedure;
	TSimpleFunction = reference to function(x: string): Integer;
	TProc = reference to procedure;
	TFunc = reference to function(x: Integer): string;
var
implementation

{ http://blog.hjf.pe.kr/269 -- 프로시저 (?)초 지연 }
procedure DelayProc(ADelay: Integer; AProc: TProc);
begin
	Sleep(ADelay);
	AProc;
end;

{ https://en.m.wikipedia.org/wiki/Anonymous_function }
procedure TForm1.Button2Click(Sender: TObject);
var
	x1: TSimpleProcedure;
	y1: TSimpleFunction;
begin
	x1 := procedure
	begin
		ShowMessage('Hello World');
	end;
	x1;
	y1 := function(x: string): Integer 
	begin
		Result := Length(x);
	end;
	ShowMessage(IntToStr(y1('bar')));
end;

{docwiki.embarcadero.com/RADStudio/XE7/en/Anonymous_Methods_in_Delphi}
procedure TForm1.Button1Click(Sender: TObject);
var
	myProc: TProc;
	myFunc: TFunc;
begin
	myFunc := function(x: Integer): string
	begin
		Result := IntToStr(x * x);
	end;
	myProc := procedure
	begin
		ShowMessage(myFunc(2));
	end;
	(Sender as TButton).Caption := '3초 기다림';
	// TThread.CreateAnonymousThread (procedure begin
		DelayProc(3 * 1000, myProc);
	// end).Start;
end;
```
