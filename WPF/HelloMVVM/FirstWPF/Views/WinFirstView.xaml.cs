using FirstWPF.ViewModels;

namespace FirstWPF.Views
{
    public partial class WinFirstView
    {
        public WinFirstView(WinFirstViewModel winFirstViewModel)
        {
            InitializeComponent();

            Grid.DataContext = winFirstViewModel;
            DataContext = Grid.DataContext;
        }
    }
}