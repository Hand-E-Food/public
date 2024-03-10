using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Contains the framework for a map generator component that only performs a single step.
    /// </summary>
    public abstract class SingleStepMapGeneratorComponent : MapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="RandomVectorMap.Generation.SingleStepMapGeneratorComponent"> class.
        /// </summary>
        public SingleStepMapGeneratorComponent()
        {
        }

        #region Properties ...

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished { get { return IsInitialized && _IsFinished; } }
        private bool _IsFinished = false;

        #endregion

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            _IsFinished = true;
        }
    }
}
