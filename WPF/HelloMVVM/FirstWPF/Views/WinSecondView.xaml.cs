using FirstWPF.ViewModels;

namespace FirstWPF.Views
{
    public partial class WinSecondView
    {
        public WinSecondView(WinSecondViewModel winSecondViewModel)
        {
            InitializeComponent();

            Grid.DataContext = winSecondViewModel;
            DataContext = Grid.DataContext;
        }
    }
}