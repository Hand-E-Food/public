using System.Collections;
using System.Collections.Generic;

namespace PsiFi.Models
{
    class ActorQueue : ICollection<IActor>
    {
        private List<IActor> actors = new List<IActor>();

        /// <inheritdoc/>
        public int Count => actors.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets the first actor.
        /// </summary>
        public IActor First => actors[0];

        /// <inheritdoc/>
        public void Add(IActor item)
        {
            if (Contains(item)) return;
            Insert(item);
            item.ActTimeChanged += Item_ActTimeChanged;
        }

        /// <inheritdoc/>
        public void Clear() => actors.Clear();

        /// <inheritdoc/>
        public bool Contains(IActor item) => actors.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(IActor[] array, int arrayIndex) => actors.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public IEnumerator<IActor> GetEnumerator() => actors.GetEnumerator();
        
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Insert(IActor item)
        {
            var newActTime = item.ActTime;
            var i = actors.FindIndex(actor => actor.ActTime > newActTime);
            if (i >= 0)
                actors.Insert(i, item);
            else
                actors.Add(item);
        }

        /// <inheritdoc/>
        public bool Remove(IActor item)
        {
            if (!actors.Remove(item)) return false;
            item.ActTimeChanged -= Item_ActTimeChanged;
            return true;
        }

        private void Item_ActTimeChanged(IActor actor)
        {
            if (actors.Remove(actor))
                Insert(actor);
        }
    }
}
