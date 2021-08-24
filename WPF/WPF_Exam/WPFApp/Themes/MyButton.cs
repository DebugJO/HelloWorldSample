using System.Windows;
using System.Windows.Controls;

namespace WPFApp.Themes
{
    public class MyButton : Button
    {
        public static readonly DependencyProperty P1 = DependencyProperty.Register(nameof(Icon), typeof(string), typeof(MyButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Icon
        {
            get => (string)GetValue(P1);
            set => SetValue(P1, value);
        }

        public static readonly DependencyProperty P2 = DependencyProperty.Register(nameof(Radius), typeof(string), typeof(MyButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Radius
        {
            get => (string)GetValue(P2);
            set => SetValue(P2, value);
        }

        public static readonly DependencyProperty P3 = DependencyProperty.Register(nameof(IconMargin), typeof(string), typeof(MyButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string IconMargin
        {
            get => (string)GetValue(P3);
            set => SetValue(P3, value);
        }
    }
}