using System.Collections.ObjectModel;

namespace MyApp.StateModels;

public class MainState
{
    public string StatusMessage { get; set; } = string.Empty;
    public ObservableCollection<string> Items { get; } = [];
}