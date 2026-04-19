### C# 코딩스타일
1. 클래스 및 구조체에 파스칼 케이스 사용
2. 지역 변수 이름과 함수 매개 변수에 카멜 케이스 사용
3. 모든 메소드 이름은 파스칼 케이스 사용
4. 네임스페이스는 파스칼 케이스 사용
5. boolean 변수 앞에 b붙이고, 프로퍼트에 앞에 is 붙임
6. 인터페이스 앞에는 I를 붙임
7. enum 앞에는 E붙임
8. private 멤버 변수 앞에는 m붙이고, 나머지 파스칼 케이스 사용

http://lonpeach.com/2017/12/24/CSharp-Coding-Standard/ 
https://namu.wiki/w/%EC%BD%94%EB%94%A9%20%EC%8A%A4%ED%83%80%EC%9D%BC

카멜 표기법과 파스칼 표기법을 적절하게 조합하여,
1. 변수명이나 함수명은 카멜 표기법을 따르고 클래스명은 파스칼 표기법을 따르는 작성 스타일, 
2. 혹은 변수명은 카멜로 표기하고 함수와 클래스명은 파스칼로 표기하는 작성 스타일이 대세이다. 
3. 전자는 Java, 후자는 C++(C#)에서 주로 볼 수 있는 스타일이다.

### LINQ Tutorial
* [LINQ Tutorial-kudvenkat](https://www.youtube.com/playlist?list=PL6n9fhu94yhWi8K02Eqxp3Xyh_OmQ0Rp6)

### Visual Studio Code 사용법
```
// vscode
솔루션 파일 생성
dotnet new sln -n "HelloSln"

콘솔 프로젝트 생성
dotnet new console -n "HelloUI"
 
클래스 라이브러리 생성
dotnet new classlib -n "HelloLib"

프로젝트 등록
bash : dotnet sln HelloSln.sln add **/*.csproj
windows : dotnet sln HelloSln.sln add .\HelloUI\HelloUI.csproj

프로젝트에 라이브러리 참조 추가
dotnet add HelloUI\HelloUI.csproj reference HelloLib\HelloLib.csproj

클래스 라이브러리에 패키지 추가(클래스라이브러리 폴더에서)
dotnet add package dapper
또는 패키지 매니저 사용

SSL reset
dotnet dev-certs https --clean
dotnet dev-certs https --trust

1.루트 폴더 생성 및 이동
mkdir MySolution
cd MySolution

2.솔루션 파일 생성 (현재 폴더 이름으로 생성됨)
dotnet new sln

3.프로젝트(앱) 생성
dotnet new console -o MyApp

4.클래스 라이브러리(Library) 생성
dotnet new classlib -o MyLibrary

5.솔루션에 프로젝트 추가
dotnet sln add MyApp/MyApp.csproj
dotnet sln add MyLibrary/MyLibrary.csproj

6.앱 프로젝트 폴더로 이동 후 라이브러리 참조 추가
cd MyApp
dotnet add reference ../MyLibrary/MyLibrary.csproj

7.전체 솔루션 빌드
dotnet build
콘솔 앱 실행
dotnet run --project MyApp/MyApp.csproj
또는 프로젝트 폴더에서 dotnet build / run

싱글 파일 배포
dotnet publish -c Release -r win-x6
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
* win-x64, win-x86, linux-x64, linux-arm, linux-arm64, osx-x64
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true
```

### .NET 경로 API

|API 종류|SingleFile 배포 시 결과|신뢰도|특징 및 한계|
|---|---|---|---|
|`AppContext.BaseDirectory`|정상 경로 (O)|최상|.NET Core 이후 표준. 어떤 환경에서도 앱이 설치된 "그 폴더"를 가리킴.|
|`AppDomain.CurrentDomain.BaseDirectory`|정상 경로 (O)|안전|구형 .NET Framework부터 있던 방식. 결과는 위와 같으나 코드가 조금 더 김.|
|`Assembly.Location`|빈 문자열 ("") (X)|절대 금지|"파일"의 위치를 묻는 것인데, SingleFile은 파일이 .exe 안에 숨어버려 위치가 없다고 판단함.|
|`Directory.GetCurrentDirectory()`|가변 경로 (??)|위험|앱 설치 폴더가 아니라, 사용자가 "지금 서 있는 폴더"임. (예: C:에서 실행하면 C:가 나옴)|

### C# NULL 가이드

`null`일 수 있는 값을 체크 없이 사용하면 빌드가 실패

`Nullable`은 경고만

* CS8600 (Null 리터럴을 Null 비허용 형식으로 변환)
	* `string name = null;` 처럼 `?`가 없는 변수에 대놓고 `null`을 넣을 때 발생
* CS8602 (Null 가능 참조의 역참조)
	* `string? name` 변수를 `if (name != null)` 체크 없이 `name.Length`처럼 바로 사용할 때 발생
* CS8603 (Null 가능 참조를 Null 비허용 형식의 반환값으로 사용
	* 반환 타입은 `string`인데 `null`일 수도 있는 값을 리턴하려고 할 때 발생

```xml
<PropertyGroup>
  <Nullable>enable</Nullable>
  <WarningsAsErrors>CS8600;CS8602;CS8603</WarningsAsErrors>
</PropertyGroup>

```

OneOf 라이브러리 사용

```cs
using OneOf;

public OneOf<string, None> GetUsername(int id)
{
    if (id == 0) return new None();
    return "John Doe";
}

var result = GetUsername(0);

string message = result.Match(
    name => $"사용자: {name}",
    none => "사용자를 찾을 수 없습니다."
);
```

C# 11 `required` 키워드

```cs
public class User
{
    public required string Name { get; init; } // 초기화 필수
    public string? Email { get; init; }        // 선택 사항
}

// 에러: Name을 넣지 않으면 빌드 안 됨
// var user = new User(); 

var user = new User { Name = "Alice" }; // OK
```

Null 가드 연산자 (Throw If Null)

```cs
// 코드 실행 초기 단계에서 null을 차단
public void ProcessData(string? input)
{
    ArgumentNullException.ThrowIfNull(input);
    // 이후부터는 input이 절대 null이 아님을 보장함
    Console.WriteLine(input.Length);
}
```

Value Object 패턴 (값 객체화)

```cs
public readonly struct Email
{
    private readonly string _value;
    public string Value => _value ?? throw new InvalidOperationException();

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email cannot be empty");
        _value = value;
    }
}
```

### throw 대표적인 예외

1.매개변수(인자) 값이 잘못되었을 때

* ArgumentNullException: 전달된 인자가 null이면 안 될 때
* ArgumentOutOfRangeException: 숫자가 범위를 벗어났을 때
* ArgumentException: 그 외에 값이 잘못되었을 때 (포괄)

2.객체의 상태가 올바르지 않을 때

* InvalidOperationException: 메서드를 호출하기 위한 전제 조건이 맞지 않을 때
* ObjectDisposedException: 리소스가 해제(Dispose)된 객체를 다시 사용하려고 할 때

3.아직 구현하지 않았거나 지원하지 않을 때

* NotImplementedException: 메서드 틀만 있고 코드는 없을 때(메모용)
* NotSupportedException: 해당 기능이 지원되지 않을 때

**Debug.Assert vs if (throw)  차이**

|구분|Debug.Assert|if (tagList.Length == 0) throw|
|---|---|---|
|**작동 환경**|Debug 모드에서만 작동|Debug/Release 모두 작동|
|**실행 결과**|개발 중 팝업창이 뜨거나 로그가 찍힘|프로그램이 예외를 발생시키며 중단됨|
|**용도**|개발자의 실수를 잡을 때 (버그 방지)|사용자의 입력값이나 데이터가 잘못되었을 때|

### C#의 핵심 예외(Exception) 클래스

1. 인자(Parameter) 관련 예외
메서드에 전달된 값이 잘못되었을 때

- **`ArgumentNullException`**: 인자가 `null`이어서는 안 될 때.
    - _예:_ `ArgumentNullException.ThrowIfNull(input);`
- **`ArgumentException`**: 인자가 `null`은 아니지만, 유효하지 않은 값일 때. (예: 빈 문자열, 형식 불일치)
    - _예:_ `if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("이름을 입력하세요.");`
- **`ArgumentOutOfRangeException`**: 인자가 허용된 숫자 범위를 벗어났을 때.
    - _예:_ `ArgumentOutOfRangeException.ThrowIfNegative(age);`

2. 상태(State) 및 조작 관련 예외

객체의 현재 상태가 해당 동작을 수행하기에 적절하지 않을 때 사용합니다.

- **`InvalidOperationException`**: 객체의 현재 상태에서 메서드 호출이 불가능할 때.
    - _예:_ 데이터베이스 연결이 닫혀 있는데 데이터를 읽으려 하는 경우.
- **`NotSupportedException`**: 메서드 자체는 존재하지만, 특정 구현체에서 해당 기능을 지원하지 않을 때.
    - _예:_ 읽기 전용 스트림에 쓰기를 시도할 때.
- **`NotImplementedException`**: 아직 개발 중이라 기능이 구현되지 않았을 때. (나중에 채워 넣어야 할 때 사용)

3. 데이터 및 연산 관련 예외

계산이나 데이터 처리 과정에서 발생합니다.

- **`DivideByZeroException`**: 0으로 나누기를 시도할 때.
- **`IndexOutOfRangeException`**: 배열이나 리스트의 인덱스 범위를 벗어날 때.
- **`KeyNotFoundException`**: 딕셔너리(`Dictionary`)에서 존재하지 않는 키를 찾으려 할 때.
- **`FormatException`**: 문자열을 숫자로 변환하는 등의 형 변환이 실패할 때.

4. 시스템 및 자원 관련 예외

시스템 리소스나 인프라 문제 시 발생합니다.

- **`IOException`**: 파일 입출력 중 오류가 발생할 때.
- **`TimeoutException`**: 작업이 지정된 시간을 초과했을 때.
- **`UnauthorizedAccessException`**: 권한이 없어 접근이 거부될 때. (파일 접근 권한 등)

|상황|추천 예외 클래스|
|---|---|
|**값이 `null`인가?**|`ArgumentNullException`|
|**값이 너무 크거나 작은가?**|`ArgumentOutOfRangeException`|
|**그 외 인자 값이 이상한가?**|`ArgumentException`|
|**실행 순서나 객체 상태가 꼬였나?**|`InvalidOperationException`|
|**아직 코드를 안 짰나?**|`NotImplementedException`|
|**이 기능은 못 쓰는 기능인가?**|`NotSupportedException`|

5. 커스텀 예제

```cs
public class InsufficientBalanceException : Exception
{
    public decimal CurrentBalance { get; }

    // 기본 메시지만 전달
    public InsufficientBalanceException(string message) : base(message) { }

    // 추가 정보(잔액 등)를 함께 전달하고 싶을 때
    public InsufficientBalanceException(string message, decimal balance) : base(message)
    {
        CurrentBalance = balance;
    }
}
```

```cs
public class BankAccount
{
    public decimal Balance { get; private set; } = 1000m;

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("출금액은 0보다 커야 합니다.");

        if (amount > Balance)
        {
            // 비즈니스 규칙 위반 시 우리가 만든 예외 던지기
            throw new InsufficientBalanceException("잔액이 부족합니다.", Balance);
        }

        Balance -= amount;
    }
}
```

```cs
var account = new BankAccount();

try
{
    account.Withdraw(5000m); // 잔액보다 큰 금액 시도
}
catch (InsufficientBalanceException ex)
{
    // 우리가 만든 전용 예외만 골라서 처리 가능
    Console.WriteLine($"[비즈니스 오류] {ex.Message}");
    Console.WriteLine($"현재 잔액: {ex.CurrentBalance}원");
}
catch (Exception ex)
{
    // 시스템 에러 등 기타 예외 처리
    Console.WriteLine($"[알 수 없는 오류] {ex.Message}");
}
```

예외(Exception)와 버그(Bug)의 구분

- **버그 (Bug)**: 개발자의 실수. 코드를 고쳐서 해결해야 함.
    - `NullReferenceException`, `IndexOutOfRangeException`, `ArgumentException`.
    - **대응**: `try-catch`로 잡지 말고, 프로그램이 죽게 놔둔 뒤 로그를 보고 **코드를 수정**해야 합니다.
- **불가항력적 예외 (Exceptional Circumstances)**: 코드 문제가 아니라 환경 문제.
    - `HttpRequestException`(네트워크 단절), `IOException`(디스크 꽉 참), `SqlException`(DB 서버 다운).
    - **대응**: 이건 코드를 수정한다고 해결되지 않으므로, **재시도(Retry)**하거나 사용자에게 **오류 상황을 알리는 처리(Graceful degradation)**를 해야 합니다.

예외 처리는 디비접속실패라던지,  프로그램 내에서 처리하지 못하는 상황을 처리하는 것이고, 나머진 버그 처리해야 함 `rgumentNullException.ThrowIfNull(input);`. 그러면 최종 개발 사용자(최종 단계에서 사용하는 로직)에서 판단하여 버그 수정요청을 할 것인지 아니면 여기에서 예외 처리를 할 것인지 정해야 함.

```cs
public void ProcessPayment(Order order)
{
    // 1. 사전 조건 검사 (Assertion 성격)
    // 여기서 터지면 호출한 쪽(개발자)이 데이터를 잘못 보낸 것이므로 '버그'임.
    ArgumentNullException.ThrowIfNull(order);

    // 2. 비즈니스 규칙 검사
    // 도메인 논리상 이 단계까지 오면 안 되는데 왔다면 시스템 설계 오류임.
    if (order.IsAlreadyProcessed)
        throw new MyBusinessException("이미 처리된 주문입니다. (로직 설계 오류)");

    // 3. 불가항력적인 상황 (여기서만 try-catch 고려)
    try {
        _paymentGateway.Send(order);
    } catch (NetworkException ex) {
        // 이건 개발자가 고칠 수 없으니 로그 남기고 사용자에게 안내
    }
}
```

**"예외 처리는 항복 선언이지, 해결책이 아니다"**
