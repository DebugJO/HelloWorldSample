using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyApp.ViewModels;
using System;

namespace MyApp;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
        {
            return null;
        }

        string vmName = param.GetType().FullName!;
        string viewName = vmName.Replace("ViewModels", "Views")
            .Replace("ViewModel", "View");
        Type? type = param.GetType().Assembly.GetType(viewName);

        if (type == null)
        {
            viewName = vmName.Replace("ViewModel", "View");
            type = param.GetType().Assembly.GetType(viewName);
        }

        if (type == null)
        {
            return new TextBlock { Text = "Not Found: " + viewName };
        }

        Control view = App.Services.GetService(type) as Control 
                       ?? (Control)Activator.CreateInstance(type)!;
            
        view.DataContext = param;
        return view;
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}