using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;

namespace MvxStarter.WPF
{
    public partial class App
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWpfSetup<Core.App>>();
        }
    }
}