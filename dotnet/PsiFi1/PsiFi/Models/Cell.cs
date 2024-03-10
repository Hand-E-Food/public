using PsiFi.Geometry;
using System;

namespace PsiFi.Models
{
    delegate void CellEventHandler(Cell cell);

    class Cell
    {
        /// <summary>
        /// The cell's location.
        /// </summary>
        public Point Location { get; }

        /// <summary>
        /// The ell's X location.
        /// </summary>
        public int X => Location.X;

        /// <summary>
        /// The cell's Y location.
        /// </summary>
        public int Y => Location.Y;

        /// <summary>
        /// True if this cell has been explored. Otherwise, false.
        /// </summary>
        public bool Explored 
        {
            get => explored;
            set
            {
                if (explored == value) return;
                explored = value;
                OnAppearanceChanged();
            }
        }
        private bool explored = false;

        /// <summary>
        /// The cell's visibility by the player.
        /// </summary>
        public CellVisibility Visibility
        {
            get => visibility;
            set
            {
                if (visibility == value) return;
                visibility = value;
                OnAppearanceChanged();
            }
        }
        private CellVisibility visibility = CellVisibility.NotVisible;

        /// <summary>
        /// This cell's appearance, goverened by the top-most element of those in this cell.
        /// </summary>
        public Appearance Appearance
        {
            get
            {
                if (!Explored)
                    return Appearance.Empty;

                switch (Visibility)
                {
                    case CellVisibility.NotVisible:
                        if (Annotation != null)
                            return Annotation.Appearance;

                        var appearance = Occupant?.IsStructure == true
                            ? Occupant.Appearance
                            : Floor.Appearance;

                        appearance.ForegroundColor = appearance.ForegroundColor == ConsoleColor.Gray
                            ? ConsoleColor.DarkGray
                            : appearance.ForegroundColor & ConsoleColor.Gray;
                        appearance.BackgroundColor = ConsoleColor.Black;

                        return appearance;
                    
                    default:
                        return (Annotation ?? Occupant ?? Item ?? (IVisible)Floor).Appearance;
                }
            }
        }
        /// <summary>
        /// Raises the <see cref="AppearanceChanged"/> event.
        /// </summary>
        protected void OnAppearanceChanged() => AppearanceChanged?.Invoke(this);
        /// <summary>
        /// Raised after <see cref="Appearance"/> or <see cref="Explored"/> or <see cref="Visibility"/> is changed.
        /// </summary>
        public event CellEventHandler? AppearanceChanged;

        /// <summary>
        /// This cell's floor.
        /// </summary>
        public Floor Floor
        {
            get => floor;
            set
            {
                if (floor == value) return;
                
                var oldFloor = floor;
                floor = null!;
                if (oldFloor != null)
                    oldFloor.AppearanceChanged -= OnAppearanceChanged;

                floor = value;
                if (floor != null)
                    floor.AppearanceChanged += OnAppearanceChanged;
            
                OnAppearanceChanged();
            }
        }
        private Floor floor = Terrain.DefaultFloor;

        /// <summary>
        /// The item in this cell.
        /// </summary>
        public Item? Item
        {
            get => item;
            set
            {
                if (item == value) return;
                
                var oldItem = item;
                item = null;
                if (oldItem != null)
                    oldItem.AppearanceChanged -= OnAppearanceChanged;

                item = value;
                if (item != null)
                    item.AppearanceChanged += OnAppearanceChanged;

                OnAppearanceChanged();
            }
        }
        private Item? item = null;

        /// <summary>
        /// The occupant in this cell.
        /// </summary>
        public Occupant? Occupant
        {
            get => occupant;
            set
            {
                if (occupant == value) return;
                
                var oldOccupant = occupant;
                occupant = null;
                if (oldOccupant != null)
                {
                    if (oldOccupant is Mob mob) mob.Cell = null;
                    oldOccupant.AppearanceChanged -= OnAppearanceChanged;
                }

                occupant = value;
                if (occupant != null)
                {
                    if (occupant is Mob mob) mob.Cell = this;
                    occupant.AppearanceChanged += OnAppearanceChanged;
                }

                OnAppearanceChanged();
            }
        }
        private Occupant? occupant = null;

        /// <summary>
        /// The annotation drawn over this cell.
        /// </summary>
        public Annotation? Annotation
        {
            get => annotation;
            set
            {
                if (annotation == value) return;

                var oldAnnotation = annotation;
                annotation = null;
                if (oldAnnotation != null)
                    oldAnnotation.AppearanceChanged -= OnAppearanceChanged;

                annotation = value;
                if (annotation != null)
                    annotation.AppearanceChanged += OnAppearanceChanged;

                OnAppearanceChanged();
            }
        }
        private Annotation? annotation = null;

        /// <summary>
        /// Initialises a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="x">This cell's X location.</param>
        /// <param name="y">This cell's Y location.</param>
        public Cell(int x, int y)
        {
            Location = new Point(x, y);
        }
    }
}