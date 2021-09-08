using FirstWPF.Helpers.CommandHelper;
using FirstWPF.Services;

namespace FirstWPF.ViewModels
{
    public class WinSecondViewModel : NotifyObject
    {
        private readonly IDateTimeServices _dateTimeServices;

        public WinSecondViewModel(IDateTimeServices dateTimeServices)
        {
            _dateTimeServices = dateTimeServices;
        }
    }
}