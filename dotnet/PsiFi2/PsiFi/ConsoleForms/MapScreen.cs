using PsiFi.Mapping;
using Rogue;
using Rogue.ConsoleForms;
using System;

namespace PsiFi.ConsoleForms
{
    public class MapScreen : Screen
    {
        private SingleLineText health;
        private MultiLineText log;
        private MapView mapView;
        private SingleLineText maximumHealth;
        private SingleLineText title;

        /// <summary>
        /// The displayed health.
        /// </summary>
        public int Health
        {
            get => int.Parse(health.Text);
            set => health.Text = value.ToString();
        }

        /// <summary>
        /// The displayed map.
        /// </summary>
        public Map Map
        {
            get => (Map)mapView.Map;
            set => mapView.Map = value;
        }

        /// <summary>
        /// The displayed maximum health.
        /// </summary>
        public int MaximumHealth
        {
            get => int.Parse(maximumHealth.Text);
            set => maximumHealth.Text = value.ToString();
        }

        /// <summary>
        /// The displayed title.
        /// </summary>
        public string Title
        {
            get => title.Text;
            set => title.Text = value;
        }

        /// <summary>
        /// Initialises a new instance of the map screen.
        /// </summary>
        public MapScreen()
        {
            title = new SingleLineText
            {
                Bounds = new Rectangle(0, 0, 80, 1),
                ForeColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            mapView = new MapView
            {
                Bounds = new Rectangle(80 - 41 - 1, 2, 41, 41),
            };

            log = new MultiLineText
            {
                Bounds = Rectangle.FromLTRB(0, mapView.Bounds.Top, mapView.Bounds.Left - 1, mapView.Bounds.Bottom),
                VerticalAlignment = VerticalAlignment.Bottom,
            };

            int controlPanelTop = mapView.Bounds.Bottom + 1;

            var healthLabel = new SingleLineText
            {
                Bounds = new Rectangle(0, controlPanelTop, 8, 1),
                Text = "Health: ",
            };

            health = new SingleLineText
            {
                Bounds = new Rectangle(healthLabel.Bounds.Right, healthLabel.Bounds.Top, 3, 1),
                ForeColor = Color.Red,
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            var healthDivider = new SingleLineText
            {
                Bounds = new Rectangle(health.Bounds.Right, health.Bounds.Top, 1, 1),
                Text = "/",
            };

            maximumHealth = new SingleLineText
            {
                Bounds = new Rectangle(healthDivider.Bounds.Right, healthDivider.Bounds.Top, 3, 1),
                ForeColor = Color.Red,
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            Controls.AddRange(title, mapView, log,
                healthLabel, health, healthDivider, maximumHealth);
        }

        /// <summary>
        /// Invalidates the map view for the next update.
        /// </summary>
        public void InvalidateMapView() => mapView.Invalidate();

        /// <summary>
        /// Adds a message to the log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message) =>
            log.Text += Environment.NewLine + message;
    }
}
