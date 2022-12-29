using ConsoleForms;

namespace PsiFi.ConsoleView
{
    /// <summary>
    /// A view that manages a sequence of interactions.
    /// </summary>
    public abstract class View<TChildControl> where TChildControl : IChildControl, new()
    {
        private readonly Dictionary<Type, Action<Interaction>> interactByType = new();

        /// <summary>
        /// Creates a new <see cref="View"/>.
        /// </summary>
        protected View()
        {
            RegisterInteractions(this);
        }

        /// <summary>
        /// Adds <see cref="Interact(Interaction)"/> methods from the specified object as
        /// interaction handlers of this view. Previously registered interaction types will take
        /// precedence over new registrations.
        /// </summary>
        /// <param name="obj">The object containing the <see cref="Interact(Interaction)"/>
        /// methods.</param>
        protected void RegisterInteractions(object obj)
        {
            foreach (var pair in GetMethodsByType.Action<Interaction>(obj, nameof(Interact)))
                if (!interactByType.ContainsKey(pair.Key))
                    interactByType.Add(pair.Key, pair.Value);
        }

        /// <summary>
        /// Runs the game.
        /// </summary>
        /// <param name="canvas">The canvas to draw to.</param>
        /// <param name="interactions">The interactions to present.</param>
        public void Run(Canvas canvas, IEnumerable<Interaction> interactions)
        {
            var previousChild = canvas.Child;
            canvas.Child = TopLevelControl;
            BeginRun();
            foreach (var interaction in interactions)
                Interact(interaction);
            EndRun();
            canvas.Child = previousChild;
        }

        /// <summary>
        /// The top-level control to display when running this view.
        /// </summary>
        protected TChildControl TopLevelControl { get; } = new();

        /// <summary>
        /// When overridden, performs initialization when a run is starting.
        /// </summary>
        protected virtual void BeginRun() { }

        /// <summary>
        /// When overridden, performs clean up when a run is ending.
        /// </summary>
        protected virtual void EndRun() { }

        /// <summary>
        /// Manages an interaction.
        /// </summary>
        /// <param name="interaction">The interaction to manage.</param>
        protected void Interact(Interaction interaction) => interactByType[interaction.GetType()](interaction);
    }
}
