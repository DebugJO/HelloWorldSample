using MvvmCross;
using MvvmCross.ViewModels;
using MvxStarter.Core.ViewModels;

namespace MvxStarter.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton(new GuestBookViewModel());

            RegisterAppStart<GuestBookViewModel>();

            //CreatableTypes().EndingWith("Service").AsInterfaces().RegisterAsLazySingleton();
            //Mvx.IoCProvider.RegisterSingleton<IGuestBookViewModel>(new GuestBookViewModel());
            //Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IGuestBookViewModel, GuestBookViewModel>();
        }
    }
}