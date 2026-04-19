### Avalonia 바인딩

|대상|문법 (Avalonia)|용도 및 특징|
|---|---|---|
|**자기 자신**|`{Binding $self}`|버튼 등 컨트롤 자체를 넘길 때|
|**이름으로 찾기**|`{Binding #ControlName}`|같은 화면 내 특정 컨트롤을 지칭 (WPF의 ElementName)|
|**속성 추출**|`{Binding #Name.Text}`|특정 컨트롤의 속성(Text, Value 등)을 바로 사용|
|**최상위 루트**|`{Binding $root}`|Window나 UserControl의 최상위 객체|
|**부모 타입**|`{Binding $parent[Window]}`|가장 가까운 해당 타입의 부모 찾기|
|**부모 단계**|`{Binding $parent[Grid, 1]}`|0은 자기 자신(가장 가까운), 1은 그 위 부모|
|**부모 데이터**|`{Binding $parent[Type].DataContext}`|리스트 안에서 **ViewModel**에 접근할 때 필수|
|**리스트 선택값**|`{Binding $parent[ListBox].SelectedItem}`|리스트박스에서 현재 선택된 행의 데이터|
|**현재 행 데이터**|`{Binding}`|리스트 아이템 자기 자신의 데이터 (객체)|

### Avalonia Command

```xml
<Interaction.Behaviors>
    <EventTriggerBehavior EventName="Loaded">
        <InvokeCommandAction Command="{Binding AppStartCommand}" />
    </EventTriggerBehavior>

    <EventTriggerBehavior EventName="Closing">
        <InvokeCommandAction Command="{Binding AppClosingCommand}"
                             PassEventArgsToCommand="True" />
    </EventTriggerBehavior>
</Interaction.Behaviors>

<Interaction.Behaviors>
    <EventTriggerBehavior EventName="PointerPressed">
        <InvokeCommandAction Command="{Binding TitleBarActionCommand}"
                             PassEventArgsToCommand="True" />
    </EventTriggerBehavior>
</Interaction.Behaviors>

<Button Content="테마 테스트"
        Command="{Binding ThemeChangeCommand}"
        CommandParameter="{Binding $self}"
        IsEnabled="{Binding !ThemeChangeCommand.IsRunning}" />

<Button Classes="HasIcon CaptionButton"
        ex:AttachedButton.Icon="ChromeMinimize" ex:AttachedButton.IconSize="12"
        Command="{Binding MinimizeWindowCommand}"
        CommandParameter="{Binding $parent[Window]}" />
```

```text
이름으로 찾기 : {Binding #ControlName}
부모 윈도우/타입으로 찾기 : {Binding $parent[Window]}
버튼 자신 : {Binding $self} 
부모 데이터 : {Binding $parent[ListBox].DataContext}"
태그나 네임 : {Binding $parent[StackPanel].Tag}"
최상위 루트 : {Binding $root}
부모 단계 : {Binding $parent[Grid]} -> {Binding $parent[Grid, 1]} -> {Binding $parent[Grid, 2]}
자식/다른요소 이름 기반 : {Binding #UserNameInput.Text}"
아이템 소스에서 Selected Item 
    Command="{Binding $parent[ListBox].DataContext.ProcessCommand}"
    CommandParameter="{Binding $parent[ListBox].SelectedItem}" 
	또는
    Command="{Binding $parent[ListBox].DataContext.DeleteCommand}"
    CommandParameter="{Binding}"
	
=============================================================================	

<StackPanel>
    //3초 뒤에 자동으로 텍스트가 바뀝니다
    <TextBlock Text="{Binding DelayedText^}" FontSize="20" />
    // 로딩 중 표시 (FallbackValue 활용)
    <TextBlock Text="{Binding DelayedText^, FallbackValue='로딩 중...'}" />
</StackPanel>

public Task<string> DelayedText => GetDelayedTextAsync();
private async Task<string> GetDelayedTextAsync()
{
    await Task.Delay(3000); // 3초 대기
    return "비동기 데이터 로드 완료!";
}
```

### Avalonia UI 컨트롤 계층 및 이벤트

|**계층 (상속 수준)**|**대상 컨트롤 (예시)**|**핵심 이벤트 (EventName)**|**용도 및 상세 설명**|
|---|---|---|---|
|**최상위 (Window)**|`Window`|**Closing**|창 닫기 시도 시 (저장 여부 확인, `e.Cancel` 가능)|
|||**Closed**|창이 완전히 파괴된 후 (리소스 정리)|
|||**Activated / Deactivated**|앱 창이 활성화되거나 포커스를 잃었을 때|
|**공통 (Control)**|**모든 UI 요소**|**Loaded**|컨트롤이 비주얼 트리에 붙었을 때 (초기화 권장 시점)|
||(Button, Grid, 등)|**Unloaded**|컨트롤이 트리에서 제거될 때 (이벤트 구독 해제)|
|||**SizeChanged**|가로/세로 크기 변경 시 (반응형 UI 제어)|
|||**DataContextChanged**|ViewModel 객체가 교체될 때|
|**입력 (Input)**|**모든 입력 요소**|**PointerPressed**|마우스 클릭 시작 (좌표, 버튼 종류 확인 가능)|
||(Border, Image 등)|**PointerReleased**|마우스 버튼에서 손을 뗄 때|
|||**PointerMoved**|마우스가 영역 안에서 움직일 때 (드래그 구현)|
|||**PointerEntered / Exited**|마우스 호버(Hover) 효과 시작과 끝|
|||**PointerWheelChanged**|마우스 휠 스크롤 조작 시|
|||**KeyDown / KeyUp**|키보드 키 입력 (단축키 처리 등)|
|||**GotFocus / LostFocus**|포커스가 들어오거나 나갈 때 (입력 가이드 표시)|
|**액션 (Button)**|`Button`, `MenuItem`|**Click**|마우스/키보드에 의한 논리적 클릭 (가장 많이 사용)|
|**텍스트 (Text)**|`TextBox`|**TextChanged**|글자가 바뀔 때마다 (실시간 검색, 유효성 검사)|
|||**Copying / Pasting**|클립보드 복사/붙여넣기 시 가로채기|
|**목록 (Items)**|`ListBox`, `ComboBox`|**SelectionChanged**|선택 항목이 변경되었을 때 (상세 보기 전환)|
||`DataGrid`|**CellEditEnded**|셀 수정 완료 후 (DB 즉시 반영 시점)|
|||**SelectionChanged**|행(Row) 선택이 바뀌었을 때|
|**토글 (Toggle)**|`CheckBox`, `Switch`|**IsCheckedChanged**|체크/언체크 상태가 변할 때|
|**범위 (Range)**|`Slider`, `Progress`|**ValueChanged**|수치가 변경될 때 (볼륨, 진행률 제어)|
|**스크롤 (Scroll)**|`ScrollViewer`|**ScrollChanged**|스크롤 위치 변경 (무한 스크롤, 헤더 고정)|

### ResourceDictionary와 Styles 차이

|구분|ResourceDictionary|Styles|
|---|---|---|
|**핵심 목적**|개별 데이터/객체 공유|컨트롤 외형 및 상태 변화 정의|
|**참조 방식**|`x:Key`를 이용한 직접 참조|`Selector`를 통한 규칙 적용|
|**구조**|순서 없는 Dictionary|순서가 있는 리스트 (CSS 방식)|
|**데이터 예시**|`#FF0000`, `24px`, 이미지|"모든 버튼의 배경을 빨갛게"|
|**파일 위치**|주로 `.axaml`의 `<Resources>`|주로 `.axaml`의 `<Styles>`|

### ResourceDictionary

디자인에서 반복적으로 사용되는 **값**이나 **객체**를 저장하는 곳

- **저장 내용**: `Color`, `SolidColorBrush`, `Thickness` (여백), `StaticResource`로 참조할 수 있는 모든 데이터
- **특징**:
    - **Key-Value** 구조로, 고유한 이름(`x:Key`)을 통해 데이터를 찾는다
    - 중복되는 색상 값이나 폰트 크기 등을 한곳에서 관리하여 유지보수를 쉽게한다

    ```xml
    <ResourceDictionary>
        <Color x:Key="BrandBlue">#007ACC</Color>
        <SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource BrandBlue}"/>
    </ResourceDictionary>
    ```
- 앱 전체에서 동일하게 쓰일 **색상 팔레트**를 정의할 때
- **공통 여백**(Margin/Padding) 수치를 정의할 때
- **FontFamily**나 고정된 텍스트 리소스를 관리할 때

### Styles

컨트롤이 어떻게 보여야 하는지 정의하는 **규칙의 집합**,  웹의 **CSS**와 매우 유사

- **저장 내용**: `Selector`(대응되는 대상)와 `Setter`(속성 값 설정)로 구성
- **특징**:
    - **순서가 중요**합니다. 나중에 정의된 스타일이 이전 스타일을 덮어쓸 수 있다.
    - `Selector`를 사용해 "모든 버튼" 또는 "Blue 클래스를 가진 텍스트"처럼 특정 대상을 지정한다.
    - **Pseudo-classes**(의사 클래스)를 지원하여 마우스 오버(`:pointerover`), 클릭(`:pressed`) 시의 변화를 정의할 수 있다.

    ```xml
    <Style Selector="Button.Modern">
        <Setter Property="Background" Value="{DynamicResource PrimaryBrush}"/>
        <Setter Property="Foreground" Value="White"/>
    </Style>
    ```

- 우리 앱만의 **커스텀 버튼 모양**을 만들 때
- 마우스를 올렸을 때 색상이 변하는 **애니메이션/효과**를 줄 때
- 컨트롤의 특정 **상태**(Error, Disabled 등)에 따른 디자인을 정의할 때

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
