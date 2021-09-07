using System.Collections.ObjectModel;
using System.Linq;

namespace FirstWPF.Models
{
    internal class ContactModel
    {
        public string UserName { get; set; }
        public string ImageSource { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public string LastMessage => Messages.Last().Message;
    }
}