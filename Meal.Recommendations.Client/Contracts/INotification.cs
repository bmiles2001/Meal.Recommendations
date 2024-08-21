using MudBlazor;

namespace Meal.Recommendations.Client.Contracts;

public interface INotification
{
    ISnackbar Toast { get; }
    Snackbar Show(string message, Severity severity) => Toast.Add(message, severity);
    Snackbar ShowSuccess(string message) => Toast.Add(message, Severity.Success);
    Snackbar ShowError(string message) => Toast.Add(message, Severity.Error);
    Snackbar ShowInformation(string message) => Toast.Add(message, Severity.Info);
}

public class Notification(ISnackbar snackbar) : INotification, IDisposable
{
    private bool disposedValue;

    public ISnackbar Toast => snackbar;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Toast.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}