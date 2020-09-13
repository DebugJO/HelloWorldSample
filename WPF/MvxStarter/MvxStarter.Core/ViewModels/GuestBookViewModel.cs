using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvxStarter.Core.Models;
using System.Collections.ObjectModel;

namespace MvxStarter.Core.ViewModels
{
    public class GuestBookViewModel : MvxViewModel
    {
        public GuestBookViewModel()
        {
            AddGuestCommand = new MvxCommand(AddGuest);
        }

        public IMvxCommand AddGuestCommand { get; set; }

        public bool CanAddGuest => FirstName?.Length > 0 && LastName?.Length > 0;

        private ObservableCollection<PersonModel> _people = new ObservableCollection<PersonModel>();
        private string _firstName;
        private string _lastName;

        public ObservableCollection<PersonModel> People
        {
            get => _people;
            set => SetProperty(ref _people, value);
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
                RaisePropertyChanged(() => FullName);
                RaisePropertyChanged(() => CanAddGuest);
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
                RaisePropertyChanged(() => FullName);
                RaisePropertyChanged(() => CanAddGuest);
            }
        }

        public string FullName => $"{FirstName} {LastName}";

        public void AddGuest()
        {
            var p = new PersonModel
            {
                FirstName = FirstName,
                LastName = LastName
            };

            FirstName = string.Empty;
            LastName = string.Empty;
            People.Add(p);
        }
    }
}