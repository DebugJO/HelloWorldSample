### 네비게이션 사용하지 않고 shell에서 page불러 사용하기

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">
    <Label Text="Main Page" HorizontalOptions="Center" VerticalOptions="Center" />
</ContentPage>

<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.Page1">
    <Label Text="Page 1" HorizontalOptions="Center" VerticalOptions="Center" />
</ContentPage>

<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.Page2">
    <Label Text="Page 2" HorizontalOptions="Center" VerticalOptions="Center" />
</ContentPage>
```

```cs
// AppShellViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class AppShellViewModel : ObservableObject
{
    [ObservableProperty]
    private ContentPage _currentPage;

    public IRelayCommand<Type> ChangePageCommand { get; }

    public AppShellViewModel()
    {
        // 초기 페이지 설정
        CurrentPage = new MainPage();
        ChangePageCommand = new RelayCommand<Type>(ChangePage);
    }

    private void ChangePage(Type pageType)
    {
        CurrentPage = (ContentPage)Activator.CreateInstance(pageType);
    }
}
```

```xml
<Shell
    x:Class="MyMauiApp.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:MyMauiApp"
    xmlns:viewmodels="clr-namespace:MyMauiApp.ViewModels"
    x:DataType="viewmodels:AppShellViewModel">

    <FlyoutItem Title="Menu">
        <MenuFlyoutItem Text="Main Page" Command="{Binding ChangePageCommand}" CommandParameter="{x:Type local:MainPage}" />
        <MenuFlyoutItem Text="Page 1" Command="{Binding ChangePageCommand}" CommandParameter="{x:Type local:Page1}" />
        <MenuFlyoutItem Text="Page 2" Command="{Binding ChangePageCommand}" CommandParameter="{x:Type local:Page2}" />
    </FlyoutItem>

    <ShellContent Content="{Binding CurrentPage}" />

</Shell>
```

```cs
// MauiProgram.cs
// ...
builder.Services.AddScoped<AppShellViewModel>();
builder.Services.AddTransient<MainPage>();
builder.Services.AddTransient<Page1>();
builder.Services.AddTransient<Page2>();
// ...
```
