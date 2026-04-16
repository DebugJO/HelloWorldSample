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
