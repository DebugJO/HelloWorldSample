using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FirstWPF.Helpers.CommandHelper;
using FirstWPF.Models;
using FirstWPF.Services;

namespace FirstWPF.ViewModels
{
    public class WinThirdViewModel : NotifyObject
    {
        private readonly IDateTimeServices _dateTimeServices;
        private readonly IPersonServices _personServices;
        private ObservableCollection<PersonModel> _personModels;

        public WinThirdViewModel(IDateTimeServices dateTimeServices, IPersonServices personServices)
        {
            _dateTimeServices = dateTimeServices;
            _personServices = personServices;

            PersonModels = new ObservableCollection<PersonModel>(_personServices.ClearItmes());
        }

        public string CurrentTime => _dateTimeServices.GetDateTimeString();

        public ObservableCollection<PersonModel> PersonModels
        {
            get => _personModels;
            set
            {
                _personModels = value;
                OnPropertyChanged();
            }
        }

        public ICommand ButtonGetList => new RelayCommand<object>(ButtonGetListImp);

        private void ButtonGetListImp(object obj)
        {
            PersonModels = new ObservableCollection<PersonModel>(_personServices.AddItmes());
        }

        public ICommand ButtonClearList => new RelayCommand<object>(ButtonClearListImp);

        private void ButtonClearListImp(object obj)
        {
            PersonModels = new ObservableCollection<PersonModel>(_personServices.ClearItmes());
        }

        public ICommand ButtonSelectItem => new RelayCommand<object>(ButtonSelectItemImp);

        private void ButtonSelectItemImp(object obj)
        {
            var p = PersonModels.First(x => x.Id == obj.ToString());
            MessageBox.Show("버튼선택 : " + p.Id + " / " + p.Name + " / " + p.Address);
        }

        public ICommand ListViewSelectItem => new RelayCommand<object>(ListViewSelectItemImp);

        private void ListViewSelectItemImp(object obj)
        {
            if (obj is PersonModel p && !string.IsNullOrEmpty(p.Id))
                MessageBox.Show("Row선택 : " + p.Id + " / " + p.Name + " / " + p.Address);
        }
    }
}