public sealed class ProgressButtonAlone
{
    private static readonly Lazy<ProgressButtonAlone> _instance = new(() => new ProgressButtonAlone());

    private string? mOriginalContent;
    private string[]? mProgressMessages;
    private int mProgressIndex;
    private DispatcherTimer? mTimer;
    private Button? mCurrentButton;
    private Button? mCurrentExecutingButton;

    private ProgressButtonAlone()
    {
    }

    public static ProgressButtonAlone Go
    {
        get => _instance.Value;
    }

    public async Task ExecuteAsync(Button? button, Func<Task> asyncAction, string[]? progressMessages = null)
    {
        if (button == null)
        {
            return;
        }

        if (mCurrentExecutingButton != null)
        {
            ShowMessageOnButton(button, "사용 중...");
            return;
        }

        mCurrentExecutingButton = button;

        if (mTimer is { IsEnabled: true })
        {
            return;
        }

        mOriginalContent = button.Content?.ToString() ?? string.Empty;
        button.Content = " ⦁⦁⦁⦁⦁";
        progressMessages ??= [" ⦁    ", " ⦁⦁   ", " ⦁⦁⦁  ", " ⦁⦁⦁⦁ ", " ⦁⦁⦁⦁⦁"];
        mCurrentButton = button;
        mCurrentButton.Loaded += OnButtonLoaded;
        mProgressMessages = progressMessages ?? throw new ArgumentNullException(nameof(progressMessages));
        mProgressIndex = 0;
        button.IsEnabled = false;

        mTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        mTimer.Tick += Timer_Tick;
        mTimer.Start();

        try
        {
            await asyncAction();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"ProgressButton(ExecuteAsync) ERROR : {ex.Message}");
        }
        finally
        {
            mTimer.Stop();
            mTimer = null;

            if (mCurrentButton != null)
            {
                mCurrentButton.Content = mOriginalContent;
                mCurrentButton.IsEnabled = true;
                mCurrentButton = null;
            }

            mCurrentExecutingButton = null;
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (mCurrentButton == null || mProgressMessages == null)
        {
            return;
        }

        mCurrentButton.Content = mProgressMessages[mProgressIndex];
        mProgressIndex = (mProgressIndex + 1) % mProgressMessages.Length;
    }

    private void OnButtonLoaded(object sender, RoutedEventArgs e)
    {
        if (mCurrentButton == null || mProgressMessages == null)
        {
            return;
        }

        mCurrentButton.Loaded -= OnButtonLoaded;
        mCurrentButton.Dispatcher.InvokeAsync(() => mCurrentButton.Content = mProgressMessages[0]);
    }

    private static async void ShowMessageOnButton(Button button, string message)
    {
        try
        {
            string originalContent = button.Content?.ToString() ?? string.Empty;
            button.Content = message;
            button.IsEnabled = false;

            await Task.Delay(500);

            button.Content = originalContent;
            button.IsEnabled = true;
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"ProgressButton(ShowMessageOnButton) ERROR : {ex.Message}");
        }
    }
}
