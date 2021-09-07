using System.Windows;
using System.Windows.Input;
using FirstWPF.Helpers.CommandHelper;
using FirstWPF.Helpers.SystemHelper;
using FirstWPF.Services;

namespace FirstWPF.ViewModels
{
    public class MainViewModel : NotifyObject
    {
        private readonly IDateTimeServices _dateTimeServices;

        public MainViewModel()
        {
            _dateTimeServices = new DateTimeServices();
            Hello = "헬로우월드";
        }

        public string CurrentTime => _dateTimeServices.GetDateTimeString();

        private string _hello;

        public string Hello
        {
            get => _hello;
            set
            {
                _hello = value;
                OnPropertyChanged();
            }
        }

        public static ICommand DragMoveWindow => new RelayCommand<object>(DragMoveWindowImp);

        private static void DragMoveWindowImp(object obj)
        {
            WindowHelper.DragMoveWindow();
        }

        public static ICommand CloseWindow => new RelayCommand<object>(CloseWindowImp);

        private static void CloseWindowImp(object obj)
        {
            Application.Current.Shutdown();
        }

        public void ButtonTest1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(sender + " / " + e.RoutedEvent.Name);
        }

        public void ButtonTest2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(sender + " / " + e.RoutedEvent.Name);
        }

        public static ICommand ButtonTest3 => new RelayCommand<object>(ButtonTest3Imp);

        private static void ButtonTest3Imp(object obj)
        {
            MessageBox.Show(obj.ToString());
        }
    }
}