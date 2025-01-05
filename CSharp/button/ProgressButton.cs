public sealed class ProgressButton
{
    private static readonly Lazy<ProgressButton> _instance = new(() => new ProgressButton());

    private ProgressButton()
    {
    }

    public static ProgressButton Go
    {
        get => _instance.Value;
    }

    private readonly ConcurrentDictionary<Button, ButtonExecutionState> mButtonExecutionStates = new();

    private class ButtonExecutionState
    {
        public string OriginalContent { get; init; } = string.Empty;
        public string[] ProgressMessages { get; init; } = [];
        public int ProgressIndex { get; set; }
        public DispatcherTimer Timer { get; set; } = new();
        public bool IsExecuting { get; set; }
    }

    public async Task ExecuteAsync(Button? button, Func<Task> asyncAction, string[]? progressMessages = null)
    {
        if (button == null)
        {
            return;
        }

        if (mButtonExecutionStates.TryGetValue(button, out ButtonExecutionState? state) && state.IsExecuting)
        {
            ShowMessageOnButton(button, "실행 중...");
            return;
        }

        progressMessages ??= [" ⦁    ", " ⦁⦁   ", " ⦁⦁⦁  ", " ⦁⦁⦁⦁ ", " ⦁⦁⦁⦁⦁"];

        ButtonExecutionState newState = new()
        {
            OriginalContent = button.Content?.ToString() ?? string.Empty,
            ProgressMessages = progressMessages ?? throw new ArgumentNullException(nameof(progressMessages)),
            ProgressIndex = 0,
            IsExecuting = true
        };

        if (!mButtonExecutionStates.TryAdd(button, newState))
        {
            mButtonExecutionStates.TryUpdate(button, newState, mButtonExecutionStates[button]);
        }


        button.Content = newState.ProgressMessages[0];
        button.IsEnabled = false;

        newState.Timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        newState.Timer.Tick += (_, _) => Timer_Tick(button);
        newState.Timer.Start();

        try
        {
            await asyncAction();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"ExecuteAsync ERROR: {ex.Message}");
        }
        finally
        {
            newState.Timer.Stop();
            mButtonExecutionStates.TryRemove(button, out _);
            button.Content = newState.OriginalContent;
            button.IsEnabled = true;
            newState.IsExecuting = false;
        }
    }

    private void Timer_Tick(Button button)
    {
        if (!mButtonExecutionStates.TryGetValue(button, out var state) || !state.IsExecuting) return;

        button.Dispatcher.InvokeAsync(() =>
        {
            try
            {
                if (state.ProgressMessages.Length == 0)
                {
                    return;
                }

                state.ProgressIndex = (state.ProgressIndex + 1) % state.ProgressMessages.Length;
                button.Content = state.ProgressMessages[state.ProgressIndex];
            }
            catch (Exception ex)
            {
                mButtonExecutionStates.TryRemove(button, out _);
                state.Timer.Stop();
                LogHelper.Logger.Error($"Timer_Tick ERROR : {ex.Message}");
            }
        });
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
            LogHelper.Logger.Error($"ShowMessageOnButton ERROR : {ex.Message}");
        }
    }
}
