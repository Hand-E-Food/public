using System;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Generates a map.
    /// </summary>
    public partial class MapGenerator : IStepper
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="MapGenerator"/> class.
        /// </summary>
        public MapGenerator()
        {
            Map = new Map();
            Random = new Random();
            Stepper = new StepperCollection();
        }

        #region Properties ...

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value> 
        public bool IsFinished
        {
            get { return Stepper.IsFinished; }
        }

        /// <summary>
        /// Gets a value indicating whether this stepper has been initialised.
        /// </summary>
        /// <value>True if this stepper has been initialised; otherwise, false.</value>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets the map being generated.
        /// </summary>
        /// <value>The map being generated.</value>
        public Map Map { get; private set; }

        /// <summary>
        /// Gets or sets this stepper's name.
        /// </summary>
        /// <value>This stepper's name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the random number generator to use.
        /// </summary>
        public Random Random { get; set; }

        /// <summary>
        /// The stepper controller.
        /// </summary>
        private StepperCollection Stepper { get; set; }

        /// <summary>
        /// Gets the name of the current task.
        /// </summary>
        /// <value>The current task's name.</value>
        public string TaskName
        {
            get
            {
                var task = Stepper.Tasks.FirstOrDefault((t) => !t.IsFinished);
                if (task != null)
                    return task.Name;
                else
                    return null;
            }
        }

        #endregion

        /// <summary>
        /// Adds a task to the generator.
        /// </summary>
        /// <param name="task">The task to add.</param>
        public void AddTask(IMapGenerator task)
        {
            task.Map = Map;
            task.Random = Random;
            Stepper.Tasks.Add(task);
        }

        /// <summary>
        /// Finishes the current task.
        /// </summary>
        public void FinishTask()
        {
            Map.ClearDebug();
            Stepper.FinishTask();
        }

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public virtual void Initialize()
        {
            Stepper.Initialize();
            IsInitialized = true;
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public void Step()
        {
            Map.ClearDebug();
            Stepper.Step();
        }
    }
}
