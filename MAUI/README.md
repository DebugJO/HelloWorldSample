### MAUI 기본 구조

Reference : Gemini 2.0

```cs
builder.Services.AddSingleton<MyViewModel>();
builder.Services.AddTransient<MyPage>();
```

```cs
// 싱글톤 뷰 모델
public class MyViewModel : INotifyPropertyChanged
{
    private string _sharedData;
    public string SharedData
    {
        get => _sharedData;
        set { _sharedData = value; OnPropertyChanged(); }
    }

    // ...
}
```

```xml
// 페이지에서 데이터 바인딩
<Label Text="{Binding SharedData}" />

<ShellContent
    Title="Home"
    ContentTemplate="{DataTemplate local:MyPage}"
    Route="MyPage" />
```

예시 (뷰 모델 싱글톤 + 페이지 Transient + ShellContent):

```cs
// MauiProgram.cs
builder.Services.AddSingleton<MyViewModel>();
builder.Services.AddTransient<MyPage>();

// AppShell.xaml
<ShellContent
    Title="Home"
    ContentTemplate="{DataTemplate local:MyPage}"
    Route="MyPage" />

// MyPage.xaml.cs (코드 비하인드에서 뷰 모델 설정)
public partial class MyPage : ContentPage
{
    public MyPage(MyViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

### 예제 시나리오

```cs
// MyData.cs
public class MyData
{
    public string UserName { get; set; }
}
```

```cs
// MyViewModel.cs : 뷰모델(Scoped)
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class MyViewModel : INotifyPropertyChanged
{
    private MyData _myData;

    public MyViewModel()
    {
        _myData = new MyData { UserName = "초기 사용자 이름" }; // 초기값 설정
    }

    public MyData MyData
    {
        get => _myData;
        set
        {
            if (_myData != value)
            {
                _myData = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

```cs
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage"
             xmlns:local="clr-namespace:MyMauiApp"
             x:DataType="local:MyViewModel">
    <VerticalStackLayout Padding="20">
        <Label Text="{Binding MyData.UserName}" FontSize="20" />
        <Entry Text="{Binding MyData.UserName}" />
        <Button Text="상세 페이지로 이동" Clicked="GoToDetailPage" />
    </VerticalStackLayout>
</ContentPage>

// MainPage.xaml.cs
public partial class MainPage : ContentPage
{
    public MainPage(MyViewModel viewModel) // 뷰 모델 주입
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void GoToDetailPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage());
    }
}

// DetailPage.xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.DetailPage"
             xmlns:local="clr-namespace:MyMauiApp"
             x:DataType="local:MyViewModel">
    <VerticalStackLayout Padding="20">
        <Label Text="{Binding MyData.UserName}" FontSize="20" />
    </VerticalStackLayout>
</ContentPage>

// DetailPage.xaml.cs
public partial class DetailPage : ContentPage
{
    public DetailPage(MyViewModel viewModel) // 뷰 모델 주입
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

```cs
// MauiProgram.cs
// ...
builder.Services.AddScoped<MyViewModel>(); // 뷰 모델 Scoped 등록
builder.Services.AddTransient<MainPage>(); // 페이지 Transient 등록
builder.Services.AddTransient<DetailPage>(); // 페이지 Transient 등록
// ...
```
