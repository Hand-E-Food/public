using System.Collections;

namespace ConsoleForms
{
    /// <summary>
    /// A control that contains other controls.
    /// </summary>
    public class Container : Control, IList<Control>
    {
        protected List<Control> Children { get; } = new();

        public bool Contains(Control item) => Children.Contains(item);
        public int Count => Children.Count;
        public void CopyTo(Control[] array, int arrayIndex) => Children.CopyTo(array, arrayIndex);
        public IEnumerator<Control> GetEnumerator() => Children.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int IndexOf(Control item) => Children.IndexOf(item);
        public bool IsReadOnly => false;

        public Control this[int index]
        {
            get => Children[index];
            set
            {
                var oldControl = Children[index];
                AddControl(value);
                RemoveControl(oldControl);
                Children[index] = value;
            }
        }

        public void Add(Control control)
        {
            Children.Add(control);
            AddControl(control);
        }

        public void Insert(int index, Control control)
        {
            Children.Insert(index, control);
            AddControl(control);
            while (++index < Children.Count)
                Children[index].Invalidate();
        }

        public void Clear()
        {
            foreach (var control in Children) RemoveControl(control);
            Children.Clear();
        }

        public bool Remove(Control control)
        {
            var removed = Children.Remove(control);
            if (removed) RemoveControl(control);
            return removed;
        }

        public void RemoveAt(int index)
        {
            RemoveControl(Children[index]);
            Children.RemoveAt(index);
        }

        /// <summary>
        /// Configures the control as a child of this control.
        /// </summary>
        /// <param name="control">The control being added.</param>
        /// <exception cref="ArgumentException">The control already has a parent.</exception>
        protected virtual void AddControl(Control control)
        {
            if (control.Parent != null) throw new ArgumentException("The control being added already has a parent.");
            control.Parent = this;
            PerformLayout();
            control.Invalidate();
        }

        /// <summary>
        /// Configures the control as no longer being a child of this control.
        /// </summary>
        /// <param name="control">The control being removed.</param>
        protected virtual void RemoveControl(Control control)
        {
            control.Invalidate();
            control.Parent = null;
            PerformLayout();
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            foreach (var child in Children.Where(child => child.IsVisible && child.Bounds.Intersects(graphics.ClipRegion)))
                child.Draw(graphics.CreateClipRegion(child.Bounds));
        }

        /// <summary>
        /// Positions all of this control's children.
        /// </summary>
        /// <remarks>When overridden, call this implementation last.</remarks>
        public override void PerformLayout()
        {
            foreach (var child in Children)
                child.PerformLayout();
        }
    }
}
