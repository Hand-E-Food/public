namespace ColourBlind
{
    /// <summary>
    /// Populates a template with polygons.
    /// </summary>
    public class CardFactory
    {
        private readonly string outDir;
        private readonly PolygonD[] polygons;
        private readonly Template template;

        public CardFactory(Template template, string outDir)
        {
            this.template = template;
            this.outDir = outDir;
            PolygonFactory polygonFactory = new(template.Radius);
            polygons = new[] {
                polygonFactory.CornersIcon,
                polygonFactory.GroupIcon,
                polygonFactory.AlignedIcon,
                polygonFactory.IsolatedIcon,
            };

        }
        
        public void CreateCards()
        {
            for (int i = 0; i < polygons.Length; i++)
                for (int j = 0; j < polygons.Length; j++)
                    CreateCard(i, j);
        }

        private void CreateCard(int i, int j)
        {
            var card = template.Clone();
            AddPolygon(card, polygons[i], PlayerId.A);
            AddPolygon(card, polygons[j], PlayerId.B);
            ColourTemplate(card);
            var path = Path.Join(outDir, $"card{i}{j}.svg");
            card.Svg.Write(path);
        }

        /// <summary>
        /// Adds a coloured polygon to a template.
        /// </summary>
        /// <param name="template">The template to which to add the polygon.</param>
        /// <param name="polygon">The polygon to add.</param>
        /// <param name="playerId">The player that needs to see this polygon.</param>
        private static void AddPolygon(Template template, PolygonD polygon, PlayerId playerId)
        {
            foreach(var circle in template.Circles)
                if (polygon.Contains(circle.Centre))
                    circle.PlayerId |= playerId;
        }

        /// <summary>
        /// Completes the template by applying the correct colour to each circle.
        /// </summary>
        /// <param name="template">The template to complete.</param>
        private static void ColourTemplate(Template template)
        {
            foreach (var circle in template.Circles)
            {
                switch (circle.PlayerId)
                {
                    case PlayerId.None:
                        circle.Hue = 240;
                        break;
                    case PlayerId.A:
                        circle.Hue = 300;
                        break;
                    case PlayerId.AB:
                        circle.Hue = 60;
                        break;
                    case PlayerId.B:
                        circle.Hue = 180;
                        break;
                }
            }
        }
    }
}
