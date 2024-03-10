using System;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Concretes common features of map generation components.
    /// </summary>
    public abstract class MapGeneratorComponent : IMapGenerator
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="MapGeneratorComponent"/> class.
        /// </summary>
        public MapGeneratorComponent()
        {
            IsInitialized = false;
        }

        #region Properties ...

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public abstract bool IsFinished { get; }

        /// <summary>
        /// Gets a value indicating whether this stepper has been initialised.
        /// </summary>
        /// <value>True if this stepper has been initialised; otherwise, false.</value>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// The map to generate.
        /// </summary>
        public Map Map { get; set; }

        /// <summary>
        /// Gets or sets this component's name.
        /// </summary>
        /// <value>This component's name.</value>
        public string Name { get; set; }

        /// <summary>
        /// The map to generate.
        /// </summary>
        public Random Random { get; set; }

        #endregion

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public virtual void Initialize() 
        {
            IsInitialized = true;
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public abstract void Step();
    }
}
