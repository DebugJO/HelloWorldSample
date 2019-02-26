### C# Thread 스레드 기초

최근에는 이러한 스레드/비동기 프로그램을 아주 쉽고 간단하게 코팅 할 수 있도록 개발언어에서 다양한 함수를 제공하고 있다. 대표적으로 델파이(Delphi)에서는 익명스레드 / CreateAnonymousThread를 사용할 있으며, C#에서는 기본적인 스레드 사용뿐 아니라 C# 5.0버전 이상에서는 Async, Await를 이용하여 좀더 편하고 직관적으로 구현이 가능하다.

##### 델파이 익명 스레드의 예
```delphi
procedure Button1Click(Sender: TObject);
var
  Th: TThread;
begin
  Th := TThread.CreateAnonymousThread( procedure begin
	// 구현 부분 (스레드로 동작함)
  end);
  Th.Start;
end;
```

