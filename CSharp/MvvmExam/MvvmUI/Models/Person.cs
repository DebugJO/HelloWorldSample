using System.ComponentModel;
using System.Windows;

namespace MvvmUI.Models
{
    public class Person : INotifyPropertyChanged
    {
        private string mName;
        private string mLastName;
        private string mFullName;

        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("FullName");
            }
        }

        public string LastName
        {
            get { return mLastName; }
            set
            {
                mLastName = value;
                OnPropertyChanged("LastName");
                OnPropertyChanged("FullName");
            }

        }

        public string FullName
        {
            get { return mName + " " + mLastName; }
            set
            {
                mFullName = value;
                OnPropertyChanged("FullName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Person()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Name = "가나";
                LastName = "닭";
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            //if (PropertyChanged != null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
