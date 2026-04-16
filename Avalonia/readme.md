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

