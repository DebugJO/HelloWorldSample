using Caliburn.Micro;

namespace CMTest80.ViewModels;

public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
{
    private string mTest;

    public ShellViewModel()
    {
        mTest = string.Empty;
    }

    public string Test
    {
        get => mTest;
        set
        {
            mTest = value;
            NotifyOfPropertyChange();
        }
    }
}