using System.Threading;

namespace WordleSolver
{
    public sealed class Progress
    {
        private int batch = 0;

        /// <summary>
        /// The current number of items processed processed. Thread safe.
        /// </summary>
        public int Count 
        {
            get => count;
            set => OnChanged(batch, count = value);
        }
        private int count = 0;

        /// <summary>
        /// The total number of items to process.
        /// </summary>
        private readonly int total;

        /// <summary>
        /// Raised when the value of <see cref="Count"/> changes.
        /// </summary>
        public event ChangedEventHandler Changed;

        private void OnChanged(int batch, int count) => Changed?.Invoke(batch, total, count);

        /// <summary>
        /// The signature of the <see cref="Changed"/> event.
        /// </summary>
        /// <param name="batch">The batch currently being processed.</param>
        /// <param name="total">The total number of items to process in this batch.</param>
        /// <param name="count">The current number of items processed in this batch.</param>
        public delegate void ChangedEventHandler(int batch, int total, int count);

        /// <summary>
        /// Initialises a new instance of the <see cref="Progress"/> class.
        /// </summary>
        /// <param name="total">The total number of items to process.</param>
        public Progress(int total)
        {
            this.total = total;
        }

        /// <summary>
        /// Adds the specified value to <see cref="Count"/> and raises the <see cref="Changed"/> event. Thread safe.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void Add(int value)
        {
            if (value != 0) OnChanged(batch, Interlocked.Add(ref count, value));
        }

        /// <summary>
        /// Decrements <see cref="Count"/> and raises the <see cref="Changed"/> event. Thread safe.
        /// </summary>
        public void Decrement() => OnChanged(batch, Interlocked.Decrement(ref count));

        /// <summary>
        /// Increments <see cref="Count"/> and raises the <see cref="Changed"/> event. Thread safe.
        /// </summary>
        public void Increment() => OnChanged(batch, Interlocked.Increment(ref count));

        /// <summary>
        /// Increments the batch number and resets <see cref="Count"/> to 0.
        /// </summary>
        public void NextBatch() => OnChanged(Interlocked.Increment(ref batch), count = 0);
    }
}
