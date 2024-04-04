namespace Bots.Forms;
public static class ControlExtensions
{
    public static IDisposable SuspendedLayout(this Control control)
    {
        control.SuspendLayout();
        return new DisposeAction(control.ResumeLayout);
    }
}
