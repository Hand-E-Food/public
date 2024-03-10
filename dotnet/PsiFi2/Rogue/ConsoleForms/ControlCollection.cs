using System.Collections;
using System.Collections.Generic;

namespace Rogue.ConsoleForms
{
    public class ControlCollection : IList<Control>
    {
        private List<Control> controls = new List<Control>();

        public Control this[int index]
        {
            get => controls[index];
            set => controls[index] = value;
        }

        public int Count => controls.Count;

        public bool IsReadOnly => false;

        public void Add(Control control)
        {
            control.Invalidate();
            controls.Add(control);
        }

        public void AddRange(params Control[] controls)
        {
            int capacity = this.controls.Capacity;
            int totalCount = Count + controls.Length;
            if (capacity < totalCount)
                capacity = totalCount;
            this.controls.Capacity = capacity;

            foreach (var control in controls)
                Add(control);
        }

        public void Clear()
        {
            controls.Reverse();
            foreach (var control in controls)
                control.Clear();
            controls.Clear();
        }

        public bool Contains(Control control) => controls.Contains(control);

        public void CopyTo(Control[] array, int arrayIndex) => controls.CopyTo(array, arrayIndex);

        public IEnumerator<Control> GetEnumerator() => controls.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => controls.GetEnumerator();

        public int IndexOf(Control control) => controls.IndexOf(control);

        public void Insert(int index, Control control)
        {
            control.Invalidate();
            controls.Insert(index, control);
        }

        public bool Remove(Control control)
        {
            if (!controls.Remove(control)) return false;
            control.Clear();
            return true;
        }

        public void RemoveAt(int index)
        {
            controls[index].Clear();
            controls.RemoveAt(index);
        }
    }
}
