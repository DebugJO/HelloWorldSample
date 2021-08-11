using System;
using System.Collections.ObjectModel;
using WPFApp1.Core;
using WPFApp1.Model;

namespace WPFApp1.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }

        public RelayCommand SendCommand { get; set; }

        private ContactModel _selectedContact;
        public ContactModel SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();

            SendCommand = new RelayCommand(o =>
            {
                Messages.Add(new MessageModel
                {
                    UserName = "MSJO",
                    UserNameColor = "#409AFF",
                    ImageSource = "https://pbs.twimg.com/media/ElXTrHcXEAACIZE.jpg",
                    Message = Message,
                    Time = DateTime.Now,
                    IsNativeOrigin = true
                });
                Message = "";
            });

            Messages.Add(new MessageModel
            {
                UserName = "가나닭",
                UserNameColor = "#409AFF",
                ImageSource = "https://t1.daumcdn.net/daumtop_chanel/op/20200723055344399.png",
                Message = "메시지를 날려봅시다!",
                Time = DateTime.Now,
                IsNativeOrigin = false,
                FirstMessage = true
            });

            for (int i = 0; i < 3; i++)
            {
                Messages.Add(new MessageModel
                {
                    UserName = "홍길동",
                    UserNameColor = "#409AFF",
                    ImageSource = "https://t1.daumcdn.net/daumtop_chanel/op/20200723055344399.png",
                    Message = $"Test Message {i}...",
                    Time = DateTime.Now,
                    IsNativeOrigin = true
                });
            }

            Messages.Add(new MessageModel
            {
                UserName = "홍길동",
                UserNameColor = "#409AFF",
                ImageSource = "https://t1.daumcdn.net/daumtop_chanel/op/20200723055344399.png",
                Message = "Last Message",
                Time = DateTime.Now,
                IsNativeOrigin = true
            });

            for (int i = 0; i < 5; i++)
            {
                Contacts.Add(new ContactModel
                {
                    UserName = $"홍길동 {i}",
                    ImageSource = "https://t1.daumcdn.net/daumtop_chanel/op/20200723055344399.png",
                    Messages = Messages
                });
            }
        }
    }
}
