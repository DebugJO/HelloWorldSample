using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFApp.Themes
{
    public partial class MsJoDatePicker : UserControl
    {
        public MsJoDatePicker()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ButtonToday_OnClick(object sender, RoutedEventArgs e)
        {
            Picker.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Picker.IsDropDownOpen = false;
        }

        public new Brush BorderBrush { get; set; }
        public new double Width { get; set; }
        public new double Height { get; set; }
        public new Brush Background { get; set; }
        public new Brush Foreground { get; set; }
    }
}