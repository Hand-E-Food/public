using System.Collections.Generic;
using System.Linq;

namespace RandomVectorMap
{

    /// <summary>
    /// A collection of steppable tasks.
    /// </summary>
    public class StepperCollection : IStepper
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="StepperCollection"/> class.
        /// </summary>
        public StepperCollection()
        {
            IsFinished = false;
            IsInitialized = false;
            Tasks = new List<IStepper>();
        }

        #region Properties ...

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value> 
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this stepper has been initialised.
        /// </summary>
        /// <value>True if this stepper has been initialised; otherwise, false.</value>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets or sets this stepper's name.
        /// </summary>
        /// <value>This stepper's name.</value>
        public string Name { get; set; }
 
        /// <summary>
        /// Gets the collection of steppable tasks to perform.
        /// </summary>
        /// <value>A collection of steppable tasks.</value>
        public List<IStepper> Tasks { get; private set; }

        #endregion

        /// <summary>
        /// Finishes all tasks.
        /// </summary>
        public void Finish()
        {
            while (!IsFinished)
            {
                FinishTask();
            }
        }

        /// <summary>
        /// Finishes the current task.
        /// </summary>
        public void FinishTask()
        {
            var task = Tasks.FirstOrDefault((t) => !t.IsFinished);
            if (task != null)
            {
                if (!task.IsInitialized) task.Initialize();
                while (!task.IsFinished)
                    task.Step();
            }
            else
            {
                IsFinished = true;
            }
        }

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public virtual void Initialize()
        {
            IsInitialized = true;
        }

        /// <summary>
        /// Performs a single step of its task.
        /// </summary>
        public void Step()
        {
            var task = Tasks.FirstOrDefault((t) => !t.IsFinished);
            if (task != null)
            {
                if (!task.IsInitialized) task.Initialize();
                if (!task.IsFinished) task.Step();
            }
            else
            {
                IsFinished = true;
            }
        }
    }
}

