class YourViewModel : Conductor<Screen>.Collection.AllActive
{
    // Menu Side Bar
    private MenuUCViewModel _menuUC;
    public MenuUCViewModel MenuUC
    {
        get { return _menuUC; }
        set { _menuUC = value; NotifyOfPropertyChange(() => MenuUC); }
    }

    // Panel
    private Screen _panelUC;
    public Screen PanelUC
    {
        get { return _panelUC; }
        set { _panelUC = value; NotifyOfPropertyChange(() => PanelUC); }
    }

    // Constructor
    public YourViewModel()
    {
        MenuUC = new MenuUCViewModel();
        ActivateItem(MenuUC);

        PanelUC = new FirstPanelUCViewModel();
        // PanelUC = IoC.Get<FirstPanelUCViewModel>();
        ActivateItem(PanelUC);
    }

    // Some method that changes PanelUC (previously FirstPanelUCViewModel) to SecondPanelUCViewModel
    public void ChangePanels()
    {
        DeactivateItem(PanelUC);
        PanelUC = new SecondPanelUCViewModel();
        ActivateItem(PanelUC);
    }
}
