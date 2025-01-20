```xml
<ListView ItemsSource="{Binding MyDataList}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <Label Text="{Binding Name}" />
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

```cs
using System.Collections.ObjectModel;

public class MyViewModel
{
    public ObservableCollection<MyData> MyDataList { get; set; }

    public MyViewModel()
    {
        MyDataList = new ObservableCollection<MyData>();
        MyDataList.Add(new MyData { Name = "데이터 1" });
        MyDataList.Add(new MyData { Name = "데이터 2" });
        // ...
    }
}

public class MyData
{
    public string Name { get; set; }
}
```

```xml
<ListView ItemsSource="{Binding MyDataList}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <Grid Padding="10">
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="Auto" />
                        <ColumnDefinition Width="*" />
                    </Grid.ColumnDefinitions>
                    <Image Source="{Binding Image}" WidthRequest="50" HeightRequest="50" />
                    <Label Grid.Column="1" Text="{Binding Name}" VerticalOptions="Center" />
                </Grid>
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

```xml
<ListView ItemsSource="{Binding MyDataList}" CachingStrategy="RecycleElement">
</ListView>
```

* Normal: 기본 상태
* PointerOver: 마우스 포인터가 컨트롤 위에 있을 때
* Pressed: 컨트롤을 누르고 있을 때
* Disabled: 컨트롤이 비활성화되었을 때

```xml
<Button Text="Button">
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup x:Name="CommonStates">
            <VisualState x:Name="Normal">
                <VisualState.Setters>
                    <Setter Property="BackgroundColor" Value="LightGray" />
                </VisualState.Setters>
            </VisualState>
            <VisualState x:Name="PointerOver">
                <VisualState.Setters>
                    <Setter Property="BackgroundColor" Value="LightBlue" />
                </VisualState.Setters>
            </VisualState>
            <VisualState x:Name="Pressed">
                <VisualState.Setters>
                    <Setter Property="BackgroundColor" Value="DarkBlue" />
                    <Setter Property="Scale" Value="0.95" />
                </VisualState.Setters>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
</Button>
```

CommunityToolkit.Maui

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
             x:Class="MyMauiApp.MainPage">

    <Button Text="Button">
      <Button.Behaviors>
          <toolkit:TouchBehavior
              BackgroundColor="LightGreen"
              Command="{Binding MyCommand}"
              Opacity="0.7"
              Scale="1.1" />
      </Button.Behaviors>
    </Button>
</ContentPage>
```

```xml
<Button Text="Button">
    <Button.Triggers>
        <EventTrigger Event="Pressed">
            <BeginStoryboard>
                <Storyboard>
                    <DoubleAnimation
                        Storyboard.TargetProperty="ScaleX"
                        To="0.9"
                        Duration="0:0:0.1" />
                    <DoubleAnimation
                        Storyboard.TargetProperty="ScaleY"
                        To="0.9"
                        Duration="0:0:0.1" />
                </Storyboard>
            </BeginStoryboard>
        </EventTrigger>
        <EventTrigger Event="Released">
            <BeginStoryboard>
                <Storyboard>
                    <DoubleAnimation
                        Storyboard.TargetProperty="ScaleX"
                        To="1"
                        Duration="0:0:0.1" />
                    <DoubleAnimation
                        Storyboard.TargetProperty="ScaleY"
                        To="1"
                        Duration="0:0:0.1" />
                </Storyboard>
            </BeginStoryboard>
        </EventTrigger>
    </Button.Triggers>
</Button>
```
