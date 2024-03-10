using MarkRichardson.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MarkRichardson.SkillTreeTemplate
{

    /// <summary>
    /// Paints the skill tree.
    /// </summary>
    public class AnimalSkillTreePainter : SkillTreePainter
    {

        /// <summary>
        /// The field value to use for this output.
        /// </summary>
        public string Animal { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="PlayerSkillTreePainter"/> class.
        /// </summary>
        public AnimalSkillTreePainter(Animal animal)
        {
            Animal = animal == SkillTreeTemplate.Animal.Blank ? string.Empty : animal.ToString();
            SetCircleText();

            Tier1();
            Tier2();
            Tier3();
            Tier4();

            Labels.Add(Animal, new Label
            {
                Text = Animal,
                Centre = Vector.Polar(Circles[Id(Animal, 1, 1)], 180f, 0.20f),
                Rotation = 0f,
                Brush = _textBrush,
                Font = _labelFont,
            });

            CalculateLogicalBounds();
        }

        /// <summary>
        /// Sets the text to display in each circle.
        /// </summary>
        private void SetCircleText()
        {
            CircleText = new Dictionary<string, string> {
                { "Ant1.1", "Mand-\nibles" },
                { "Ant1.2", "Spit\nAcid" },
                { "Ant1.3", "Track\nScent" },
                { "Ant1.4", "Psi-\nShield" },
                { "Ant2.1", "Digest\nJuices" },
                { "Ant2.2", "Long\nSight" },
                { "Ant2.3", "Chitter" },
                { "Ant2.4", "Omni-\nSight" },
                { "Ant2.5", "" },
                { "Ant2.6", "Canabal" },
                { "Ant3.1", "Spiny\nCarapice" },
                { "Ant3.2", "Psi-\nChit" },
                { "Ant3.3", "Wings" },
                { "Ant4.1", "Queen" },
                { "1.1", "1" },
                { "1.2", "2" },
                { "1.3", "3" },
                { "1.4", "4" },
                { "2.1", "12" },
                { "2.2", "23" },
                { "2.3", "13" },
                { "2.4", "34" },
                { "2.5", "14" },
                { "2.6", "24" },
                { "3.1", "12+23" },
                { "3.2", "13+34" },
                { "3.3", "14+24" },
                { "4.1", "All" },
            };
        }

        /// <summary>
        /// Creates circles for tier 1.
        /// </summary>
        private void Tier1()
        {
            float deg;
            int i;
            var t1s1 = CreateCircle(Animal, 1, 1, new PointF(0, 0));
            for (deg = -60, i = 2; i <= 4; deg += 120, i++)
            {
                CreateCircle(Animal, 1, i, Vector.Polar(t1s1, deg, 0.1f));
            }
            Areas.Add(Id(Animal, 1), new Circle {
                Centre = t1s1,
                Radius = 0.140f,
                Pen = _areaPen,
            });
        }

        /// <summary>
        /// Creates circles and arrows for tier 2.
        /// </summary>
        private void Tier2()
        {
            float deg;
            int i;
            Circle t2, t1s1 = Circles[Id(Animal, 1, 1)];
            for (deg = 0, i = 2; i <= 4; deg += 120, i++)
            {
                t2 = CreateCircle(Animal, 2, i * 2 - 3, Vector.Polar(t1s1, deg - 15f, 0.22f));
                CreateArrow(t1s1, t2);
                CreateArrow(Circles[Id(Animal, 1, i)], t2);

                t2 = CreateCircle(Animal, 2, i * 2 - 2, Vector.Polar(t1s1, deg + 15f, 0.22f));
                CreateArrow(Circles[Id(Animal, 1, i)], t2);
                CreateArrow(Circles[Id(Animal, 1, i < 4 ? i + 1 : i - 2)], t2);
            }
            Areas.Add(Id(Animal, 2), new Circle {
                Centre = t1s1,
                Radius = 0.265f,
                Pen = _areaPen,
            });
        }

        /// <summary>
        /// Creates circles and arrows for tier 3.
        /// </summary>
        private void Tier3()
        {
            float deg;
            int i;
            Circle t3, t1s1 = Circles[Id(Animal, 1, 1)];
            for (deg = 0, i = 1; i <= 3; deg += 120, i++)
            {
                t3 = CreateCircle(Animal, 3, i, Vector.Polar(t1s1, deg, 0.42f));
                CreateArrow(Circles[Id(Animal, 2, i * 2 - 1)], t3);
                CreateArrow(Circles[Id(Animal, 2, i * 2 - 0)], t3);
            }
        }

        /// <summary>
        /// Creates circles and arrows for tier 4.
        /// </summary>
        private void Tier4()
        {
            var t1s1 = Circles[Id(Animal, 1, 1)];
            var t4s1 = CreateCircle(Animal, 4, 1, Vector.Polar(t1s1, 0f, 0.64f));
            for (int i = 1; i <= 3; i++)
            {
                CreateArrow(Circles[Id(Animal, 3, i)], t4s1);
            }
        }
    }
}
