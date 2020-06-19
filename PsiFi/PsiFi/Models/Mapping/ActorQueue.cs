using System;
using System.Collections;
using System.Collections.Generic;

namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// A container of actors that is kept in order of <see cref="Actor.NextTimeIndex"/>.
    /// </summary>
    class ActorQueue : ICollection<Actor>
    {
        private readonly List<Actor> actors;

        /// <inheritdoc/>
        public int Count => actors.Count;

        /// <summary>
        /// The next actor in the queue.
        /// </summary>
        public Actor Next => actors[0];

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <summary>
        /// Initialises a new instance of the <see cref="ActorQueue"/> class.
        /// </summary>
        public ActorQueue()
        {
            actors = new List<Actor>();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ActorQueue"/> class with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">This collection's initial capacity.</param>
        public ActorQueue(int capacity)
        {
            actors = new List<Actor>(capacity);
        }

        /// <inheritdoc/>
        public void Add(Actor actor)
        {
            if (actors.Contains(actor))
                throw new InvalidOperationException("Duplicate actor added to queue.");
            actor.NextTimeIndexChanged += Actor_NextTimeIndexChanged;
            AddInternal(actor);
        }

        private void AddInternal(Actor actor)
        {
            int i = actors.Count - 1;
            while (i >= 0 && actors[i].NextTimeIndex > actor.NextTimeIndex)
                i--;
            actors.Insert(++i, actor);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (var actor in actors)
                actor.NextTimeIndexChanged -= Actor_NextTimeIndexChanged;
            actors.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(Actor actor) => actors.Contains(actor);

        /// <inheritdoc/>
        public void CopyTo(Actor[] array, int arrayIndex) => actors.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        /// <inheritdoc/>
        public IEnumerator<Actor> GetEnumerator() => actors.GetEnumerator();

        /// <inheritdoc/>
        public bool Remove(Actor actor)
        {
            var removed = actors.Remove(actor);
            if (removed)
                actor.NextTimeIndexChanged -= Actor_NextTimeIndexChanged;
            return removed;
        }

        private void Actor_NextTimeIndexChanged(object sender, ValueChangedEventArgs<int> e)
        {
            var actor = (Actor)sender;
            if (actors.Remove(actor))
                AddInternal(actor);
        }
    }
}
