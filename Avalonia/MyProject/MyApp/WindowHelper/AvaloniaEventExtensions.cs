using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace MyApp.WindowHelper;

public static class AvaloniaEventExtensions
{
    extension(RoutedEventArgs e)
    {
        //  Source를 원하는 타입으로 변환 (e.GetSourceAs<DockPanel>())
        public T? GetSourceAs<T>() where T : class
        {
            return e.Source as T;
        }

        // 윈도우 찾기 (e.GetWindow())
        public Window? GetWindow()
        {
            return (e.Source as Visual)?.GetVisualRoot() as Window;
        }

        // 조상 찾기 (e.FindParent<Grid>())
        public T? FindParent<T>() where T : class
        {
            return (e.Source as Visual)?.FindAncestorOfType<T>();
        }

        // 윈도우 내 컨트롤 찾기 (e.FindInWindow<Grid>("Name"))
        public T? FindInWindow<T>(string name) where T : Control
        {
            return e.GetWindow()?.FindControl<T>(name);
        }
    }
}

// [RelayCommand]
// private void TitleBarAction(PointerPressedEventArgs e)
// {
//     // e에서 바로 윈도우 찾기
//     var window = e.GetWindow();
//     
//     // e에서 바로 특정 이름의 Grid 찾기
//     var targetGrid = e.FindInWindow<Grid>("MyTargetGrid");
//
//     //  만약 Source(DockPanel)가 필요하다면
//     var dockPanel = e.GetSourceAs<DockPanel>();
//
//     // 로직 실행
//     if (window != null && e.ClickCount == 2)
//     {
//         window.WindowState = window.WindowState == WindowState.Maximized 
//             ? WindowState.Normal : WindowState.Maximized;
//     }
//     else
//     {
//         window?.BeginMoveDrag(e);
//     }
// }

// 확장메소드 없이 사용하기
// [RelayCommand]
// private void TitleBarAction(PointerPressedEventArgs e)
// {
//     if (e.Source is not Visual visual || visual.GetVisualRoot() is not Window window)
//     {
//         return;
//     }
//
//
//     PointerPointProperties pointerProps = e.GetCurrentPoint(window).Properties;
//
//     if (!pointerProps.IsLeftButtonPressed)
//     {
//         return;
//     }
//
//     if (e.ClickCount == 2)
//     {
//         WindowState = WindowState == WindowState.Maximized
//             ? WindowState.Normal
//             : WindowState.Maximized;
//
//         window.WindowState = WindowState;
//     }
//     else
//     {
//         window.BeginMoveDrag(e);
//     }
// }	
