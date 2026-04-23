using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace MyApp.WindowHelper;

public static class VisualExtensions
{
    extension(Visual? visual)
    {
        /// <summary>
        /// 최상위 윈도우 찾기
        /// </summary>
        public Window? GetWindow()
        {
            return visual?.GetVisualRoot() as Window;
        }

        /// <summary>
        /// 특정 타입의 조상 찾기
        /// </summary>
        public T? FindParent<T>(bool includeSelf = false) where T : class
        {
            return visual?.FindAncestorOfType<T>(includeSelf);
        }

        /// <summary>
        /// 바로 위 직계 부모 가져오기
        /// </summary>
        public T? GetVisualParent<T>() where T : class
        {
            return visual?.GetVisualParent() as T;
        }

        /// <summary>
        /// 윈도우 내에서 이름으로 컨트롤 찾기
        /// </summary>
        public T? FindInWindow<T>(string name) where T : Control
        {
            return visual.GetWindow()?.FindControl<T>(name);
        }

        /// <summary>
        /// 이름과 타입으로 조상 찾기
        /// </summary>
        public T? FindParentByName<T>(string name) where T : Control
        {
            if (visual == null)
            {
                return null;
            }

            foreach (Visual ancestor in visual.GetVisualAncestors())
            {
                if (ancestor is T target && target.Name == name)
                {
                    return target;
                }
            }

            return null;
        }
    }
}

// [RelayCommand]
// private void TitleBarAction(PointerPressedEventArgs e)
// {
//     var source = e.Source as Visual;
//
//     // 윈도우 찾기
//     var window = source.GetWindow(); 
//     
//     // 이름으로 특정 Grid 찾기
//     var targetGrid = source.FindInWindow<Grid>("MyTargetGrid");
//
//     // 가장 가까운 부모 Border 찾기
//     var outerBorder = source.FindParent<Border>();
//
//     // 로직 수행
//     if (window != null && e.ClickCount == 2)
//     {
//         window.WindowState = window.WindowState == WindowState.Maximized 
//             ? WindowState.Normal : WindowState.Maximized;
//     }
// }