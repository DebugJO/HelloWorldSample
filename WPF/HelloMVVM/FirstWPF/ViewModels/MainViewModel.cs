using FirstWPF.Helpers.CommandHelper;
using FirstWPF.Helpers.SystemHelper;
using FirstWPF.Services;
using FirstWPF.Views;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FirstWPF.ViewModels
{
    public class MainViewModel : NotifyObject
    {
        private readonly IDateTimeServices _dateTimeServices;
        private string _hello;
        private Window _contentControlMain;

        public MainViewModel(IDateTimeServices dateTimeServices)
        {
            _dateTimeServices = dateTimeServices;
            Hello = "헬로우월드";
            ContentControlMain = null;
        }

        public string CurrentTime => _dateTimeServices.GetDateTimeString();

        public string Hello
        {
            get => _hello;
            set
            {
                _hello = value;
                OnPropertyChanged();
            }
        }

        public Window ContentControlMain
        {
            get => _contentControlMain;
            set
            {
                _contentControlMain = value;
                OnPropertyChanged();
            }
        }

        public void ButtonFirst()
        {
            ContentControlMain = IoC.Get<WinFirstView>();
        }

        public void ButtonSecond()
        {
            ContentControlMain = IoC.Get<WinSecondView>();
        }

        public ICommand ButtonItemTest => new RelayCommand<object>(ButtonItemTestImp);

        private void ButtonItemTestImp(object obj)
        {
            ContentControlMain = IoC.Get<WinThirdView>();
        }


        public void ButtonTest1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(sender + " / " + e.RoutedEvent.Name);
        }

        public static ICommand WindowClosing
        {
            get
            {
                return new RelayCommand<CancelEventArgs>((e) =>
                {
                    if (MessageBox.Show("프로그램을 종료하시겠습니까?", "프로그램 종료",
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        for (int windowsCount = Application.Current.Windows.Count - 1; windowsCount > 0; windowsCount--)
                        {
                            Application.Current.Windows[windowsCount]?.Close();
                        }

                        Application.Current.Shutdown();
                    }
                });
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
            Application.Current.Windows.OfType<MainView>().First().Close();
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