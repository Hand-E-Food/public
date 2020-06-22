using PsiFi.Models.Mapping.Geometry;
using System;
using System.Diagnostics;

namespace PsiFi.Models.Mapping
{
    [DebuggerDisplay("{Location}")]
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
                if (value?.Cell != null && value.Cell != this) throw new InvalidOperationException("Mob is already in a cell.");

                if (mob != null)
                {
                    var old = mob;
                    mob = null;
                    old.Cell = null;
                }
                if (value != null)
                    value.Cell = this;
                mob = value;
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

        public Location Location { get; }

        public Cell(Location location)
        {
            Location = location;
        }

        protected void OnChanged() => HasChanged = true;
    }
}
