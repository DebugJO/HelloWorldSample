```xml
<Shell
    x:Class="MyMauiApp.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:MyMauiApp"
    xmlns:viewmodels="clr-namespace:MyMauiApp.ViewModels"
    x:DataType="viewmodels:AppShellViewModel">

    <ShellContent
        Title="Main"
        ContentTemplate="{Binding CurrentPageTemplate}" />

</Shell>
```

```cs
// AppShellViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;

public partial class AppShellViewModel : ObservableObject
{
    [ObservableProperty]
    private DataTemplate _currentPageTemplate;

    public AppShellViewModel()
    {
        // 초기 페이지 설정
        CurrentPageTemplate = new DataTemplate(typeof(MainPage));
    }

    public void ChangePage(Type pageType)
    {
        CurrentPageTemplate = new DataTemplate(pageType);
    }
}
```

```cs
// AppShell.xaml.cs
public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

```cs
// 예시: 메뉴 클릭 이벤트 핸들러
private void OnMenuItemClicked(object sender, EventArgs e)
{
    // ... 메뉴 항목에 따라 pageType 설정
    Type pageType = typeof(NewPage);
    ((AppShellViewModel)BindingContext).ChangePage(pageType);
}
```

```cs
builder.Services.AddTransient<MainPage>();
builder.Services.AddTransient<NewPage>();
builder.Services.AddScoped<AppShellViewModel>();
```
