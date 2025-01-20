```cs
// MyData.cs
public class MyData
{
    public string UserName { get; set; }
}
```

```cs
// MyViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;

public partial class MyViewModel : ObservableObject
{
    [ObservableProperty]
    private MyData _myData = new MyData { UserName = "초기 사용자 이름" }; // 초기값 설정
}
```

```cs
// 페이지(Transient)
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage"
             xmlns:local="clr-namespace:MyMauiApp"
             x:DataType="local:MyViewModel">
    <VerticalStackLayout Padding="20">
        <Label Text="{Binding MyData.UserName}" FontSize="20" />
        <Entry Text="{Binding MyData.UserName}" />
        <Button Text="상세 페이지로 이동" Command="{Binding GoToDetailPageCommand}" />
    </VerticalStackLayout>
</ContentPage>

// MainPage.xaml.cs
using CommunityToolkit.Mvvm.Input;

public partial class MainPage : ContentPage
{
    public MainPage(MyViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

//DetailPage.xaml
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
    public DetailPage(MyViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

```cs
// 뷰 모델에 Command 추가
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class MyViewModel : ObservableObject
{
    [ObservableProperty]
    private MyData _myData = new MyData { UserName = "초기 사용자 이름" };

    public IRelayCommand GoToDetailPageCommand { get; }

    public MyViewModel()
    {
        GoToDetailPageCommand = new RelayCommand(GoToDetailPage);
    }

    private async void GoToDetailPage()
    {
       await Shell.Current.GoToAsync(nameof(DetailPage));
    }
}
```

```cs
// MauiProgram.cs
// ...
builder.Services.AddScoped<MyViewModel>();
builder.Services.AddTransient<MainPage>();
builder.Services.AddTransient<DetailPage>();
Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
// ...
```

```cs
// Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage)); 의미
// 뷰 모델에서 페이지 이동
private async void GoToDetailPage()
{
    await Shell.Current.GoToAsync(nameof(DetailPage)); // 등록된 경로 사용
    // 또는
    await Shell.Current.GoToAsync("DetailPage"); // 직접 문자열 경로 사용 (nameof 사용 권장)
}
```
