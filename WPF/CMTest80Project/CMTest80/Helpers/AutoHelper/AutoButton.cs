using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CMTest80.Helpers.AutoHelper;

public class AutoButton : IDisposable
{
    private Button? button;
    private bool mbIsDispose;

    public AutoButton(object sender)
    {
        if (sender is not Button btn)
        {
            return;
        }

        button = btn;
        button.IsEnabled = false;
    }

    ~AutoButton() => Dispose(false);

    protected virtual void Dispose(bool disposing)
    {
        if (mbIsDispose)
        {
            return;
        }

        if (disposing)
        {
            if (button != null)
            {
                button.IsEnabled = true;
                button = null;
            }
        }

        mbIsDispose = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

public class AutoButtonAsync : IAsyncDisposable
{
    private Button? button;
    private Action? action;
    private string? ButtonString;
    private IAsyncDisposable? disposable;

    public AutoButtonAsync(object sender, Action act)
    {
        disposable = new NoopAsyncDisposable();

        action = act;

        if (sender is not Button btn)
        {
            return;
        }

        ButtonString = btn.Content.ToString();
        button = btn;
        button.Content = "처리 중 ...";
        button.IsEnabled = false;
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (disposable is not null)
        {
            await disposable.DisposeAsync().ConfigureAwait(false);

            if (button != null)
            {
                await Task.Delay(200);
                action?.Invoke();
                button.Content = ButtonString;
                button.IsEnabled = true;
                button = null;
                action = null;
                ButtonString = null;
            }
        }

        disposable = null;
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    private sealed class NoopAsyncDisposable : IAsyncDisposable
    {
        ValueTask IAsyncDisposable.DisposeAsync() => ValueTask.CompletedTask;
    }
}
