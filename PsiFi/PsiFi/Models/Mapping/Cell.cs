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

        public Mob Mob 
        {
            get => mob;
            set
            {
                if (mob == value) return;
                if (mob != null)
                    mob.Cell = null;
                mob = value;
                if (mob != null)
                    mob.Cell = this;
                OnChanged();
            }
        }
        private Mob mob;

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
