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
