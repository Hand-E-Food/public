namespace Bots;
public sealed class DisposeAction(Action dispose) : IDisposable
{
    private readonly Action dispose = dispose;
    private bool isDisposed = false;

    public void Dispose()
    {
        if (!isDisposed)
        {
            dispose();
            isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
