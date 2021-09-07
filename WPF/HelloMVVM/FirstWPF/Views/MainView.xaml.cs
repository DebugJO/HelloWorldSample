using FirstWPF.ViewModels;

namespace FirstWPF.Views
{
    public partial class MainView
    {
        public MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            DataContext = mainViewModel;
        }
    }
}