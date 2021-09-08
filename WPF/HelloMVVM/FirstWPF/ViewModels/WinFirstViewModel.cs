using FirstWPF.Helpers.CommandHelper;
using FirstWPF.Services;
using System.Windows.Input;

namespace FirstWPF.ViewModels
{
    public class WinFirstViewModel : NotifyObject
    {
        private readonly IDateTimeServices _dateTimeServices;
        private string _helloFirst;
        private string _helloResult;

        public WinFirstViewModel(IDateTimeServices dateTimeServices)
        {
            _dateTimeServices = dateTimeServices;
            Hello = "홍길동";
            HelloResult = "";
        }

        public string CurrentTime => _dateTimeServices.GetDateTimeString();

        public string Hello
        {
            get => _helloFirst;
            set
            {
                _helloFirst = value;
                OnPropertyChanged();
            }
        }

        public string HelloResult
        {
            get => _helloResult;
            set
            {
                _helloResult = value;
                OnPropertyChanged();
            }
        }

        public ICommand ButtonTest => new RelayCommand<object>(ButtonTestImp);

        private void ButtonTestImp(object obj)
        {
            HelloResult = Hello + ", 안녕하세요!";
        }
    }
}