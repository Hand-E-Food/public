using System.Collections;

namespace ConsoleForms
{
    public class ChildControls : IList<IChildControl>
    {
        /// <summary>
        /// Creates a new <see cref="ChildControls"/>
        /// </summary>
        /// <param name="parent">The parent of the controls in this collection.</param>
        public ChildControls(IParentControl parent)
        {
            this.parent = parent;
        }

        public bool Contains(IChildControl item) => children.Contains(item);
        public void CopyTo(IChildControl[] array, int arrayIndex) => children.CopyTo(array, arrayIndex);
        public int Count => children.Count;
        public IEnumerator<IChildControl> GetEnumerator() => children.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => children.GetEnumerator();
        public int IndexOf(IChildControl item) => children.IndexOf(item);
        public bool IsReadOnly => false;

        public IChildControl this[int index]
        {
            get => children[index];
            set
            {
                var item = children[index];
                if (item == value) return;
                AssertOrphan(item);
                item.Parent = null;
                children[index] = value;
                value.Parent = parent;
                parent.InvalidateLayout();
            }
        }

        public void Add(IChildControl item)
        {
            AssertOrphan(item);
            children.Add(item);
            item.Parent = parent;
            parent.InvalidateLayout();
        }

        public void Clear()
        {
            if (children.Count == 0) return;
            foreach (var item in children)
                item.Parent = null;
            children.Clear();
            parent.InvalidateLayout();
        }

        public void Insert(int index, IChildControl item)
        {
            AssertOrphan(item);
            children.Insert(index, item);
            item.Parent = parent;
            parent.InvalidateLayout();
        }

        public bool Remove(IChildControl item)
        {
            var removed = children.Remove(item);
            if (!removed) return false;
            item.Parent = null;
            parent.InvalidateLayout();
            return true;
        }

        public void RemoveAt(int index)
        {
            var item = children[index];
            children.RemoveAt(index);
            item.Parent = null;
            parent.InvalidateLayout();
        }

        /// <summary>
        /// Replaces the specified child with another.
        /// </summary>
        /// <param name="original">The child to replace.</param>
        /// <param name="replacement">The child to replace it with.</param>
        /// <exception cref="ArgumentException"><paramref name="original"/> is not in this
        /// collection.</exception>
        public void Replace(IChildControl original, IChildControl replacement)
        {
            var index = children.IndexOf(original);
            if (index < 0) throw new ArgumentException($"{nameof(original)} is not in this collection.");
            children[index] = replacement;
        }

        /// <summary>
        /// The parent of the controls in this collection.
        /// </summary>
        private readonly IParentControl parent;

        /// <summary>
        /// The child controls.
        /// </summary>
        private readonly List<IChildControl> children = new();

        private static void AssertOrphan(IChildControl item)
        {
            if (item.Parent != null) throw new InvalidOperationException("Cannot add a control that already has a parent.");
        }
    }
}