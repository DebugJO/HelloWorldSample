using FirstWPF.ViewModels;

namespace FirstWPF.Views
{
    public partial class WinThirdView
    {
        public WinThirdView(WinThirdViewModel winThirdViewModel)
        {
            InitializeComponent();

            Grid.DataContext = winThirdViewModel;
            DataContext = Grid.DataContext;
        }
    }
}