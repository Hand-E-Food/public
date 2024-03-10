namespace PsiFi.ConsoleView
{
    /// <summary>
    /// Calls a method when this object is disposed.
    /// This is indended for use in <see langword="using"/> blocks to execute clean up code.
    /// </summary>
    internal class DisposeCallback : IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="DisposeCallback"/>.
        /// </summary>
        /// <param name="callback">The method to call when this object is disposed.</param>
        public DisposeCallback(Action callback)
        {
            this.callback = callback;
        }

        public void Dispose()
        {
            if (callback != null)
            {
                callback();
                callback = null;
            }
        }

        /// <summary>
        /// The method to call when this object is disposed.
        /// </summary>
        private Action? callback;
    }
}
