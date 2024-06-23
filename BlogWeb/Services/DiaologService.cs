public class DialogService
{
    public event Action<string, string>? ShowDialog;
    public event Action? HideDialog;

    public void Show(string title, string message)
    {
        ShowDialog?.Invoke(title, message);
    }

    public void Hide()
    {
        HideDialog?.Invoke();
    }
}
