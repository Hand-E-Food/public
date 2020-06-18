using System;

namespace PsiFi.Models.Mapping
{
    class Cell
    {
        public Annotation Annotation {
            get => annotation;
            set
            {
                if (annotation == value) return;
                annotation = value;
                OnChanged();
            }
        }
        private Annotation annotation;

        public Appearance Appearance => Annotation?.Appearance ?? Mob?.Appearance ?? Item?.Appearance ?? Terrain?.Appearance ?? Appearance.None;

        public bool HasChanged { get; set; } = true;

        public IItem Item {
            get => item;
            set
            {
                if (item == value) return;
                item = value;
                OnChanged();
            }
        }
        private IItem item;

        public IMob Mob 
        {
            get => mob;
            set
            {
                if (mob == value) return;
                mob.Cell = null;
                mob = value;
                mob.Cell = this;
                OnChanged();
            }
        }
        private IMob mob;

        public Terrain Terrain
        {
            get => terrain;
            set
            {
                if (terrain == value) return;
                terrain = value;
                OnChanged();
            }
        }
        private Terrain terrain;

        public int X { get; }

        public int Y { get; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        protected void OnChanged() => HasChanged = true;
    }
}
