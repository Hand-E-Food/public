using MarkRichardson.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MarkRichardson.SkillTreeTemplate
{

    /// <summary>
    /// Paints the skill tree.
    /// </summary>
    public class PlayerSkillTreePainter : SkillTreePainter
    {

        /// <summary>
        /// The colour designated for each area.
        /// </summary>
        #region private Dictionary<string, Color> _areaColour = { ... }
        private Dictionary<string, Color> _areaColour = new Dictionary<string,Color> {
            { "A", Color.FromArgb(64, 000, 255, 000) },
            { "M", Color.FromArgb(64, 255, 000, 000) },
            { "P", Color.FromArgb(64, 000, 000, 255) },
            { "S", Color.FromArgb(64, 128, 128, 000) },
        };
        #endregion

        /// <summary>
        /// The text to display on each label.
        /// </summary>
        #region private static readonly Dictionary<string, string> _labelText = { ... }
        private static readonly Dictionary<string, string> _labelText = new Dictionary<string, string> {
            { "A", "Athlete" },
            { "M", "Military" },
            { "P", "Psion" },
            { "S", "Scientist" },
            { "AM", "Operative" },
            { "AP", "Elemental" },
            { "AS", "Quborg" },
            { "MP", "Phantasm" },
            { "MS", "Dreadnaught" },
            { "PS", "Divine" },
        };
        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="PlayerSkillTreePainter"/> class.
        /// </summary>
        public PlayerSkillTreePainter(bool blank = false)
        {
            if (blank)
                SetBlankCircleText();
            else
                SetCircleText();

            int i;
            float deg;
            var fields = new[] { "P", "S", "A", "M" };

            Field1(fields[0]);
            var origin = Circles[Id(fields[0], 1, 1)];
            for (i = 1, deg = 0; i <= 3; i++, deg += 120f)
                Tier12(fields[i], origin, deg);

            Tier34(Combine(fields, 0, 1), Circles[Id(fields[0], 2, 1)], Circles[Id(fields[0], 2, 2)], Circles[Id(fields[1], 2, 4)], Circles[Id(fields[1], 2, 3)], 0);
            Tier34(Combine(fields, 0, 2), Circles[Id(fields[0], 2, 3)], Circles[Id(fields[0], 2, 4)], Circles[Id(fields[2], 2, 4)], Circles[Id(fields[2], 2, 3)], 120);
            Tier34(Combine(fields, 0, 3), Circles[Id(fields[0], 2, 5)], Circles[Id(fields[0], 2, 6)], Circles[Id(fields[3], 2, 4)], Circles[Id(fields[3], 2, 3)], 240);
            Tier34(Combine(fields, 1, 2), Circles[Id(fields[1], 2, 1)], Circles[Id(fields[1], 2, 2)], Circles[Id(fields[2], 2, 6)], Circles[Id(fields[2], 2, 5)], 150);
            Tier34(Combine(fields, 1, 3), Circles[Id(fields[1], 2, 5)], Circles[Id(fields[1], 2, 6)], Circles[Id(fields[3], 2, 2)], Circles[Id(fields[3], 2, 1)], 210);
            Tier34(Combine(fields, 2, 3), Circles[Id(fields[2], 2, 1)], Circles[Id(fields[2], 2, 2)], Circles[Id(fields[3], 2, 6)], Circles[Id(fields[3], 2, 5)], 270);

            CalculateLogicalBounds();
        }

        /// <summary>
        /// Sets the text to display in each circle.
        /// </summary>
        private void SetCircleText()
        {
            CircleText = new Dictionary<string, string> {
                { "A1.1", "" }, // Cardio
                { "A1.2", "Run" },
                { "A1.3", "" }, // Jump
                { "A1.4", "" },
                { "A2.1", "" }, // Charge = Cardio + Run
                { "A2.2", "" }, // Parkour = Jump + Run
                { "A2.3", "" },
                { "A2.4", "" },
                { "A2.5", "" },
                { "A2.6", "" },
                { "M1.1", "Hand\n-gun" },
                { "M1.2", "Armour" },
                { "M1.3", "Long\n-arm" },
                { "M1.4", "Blade" },
                { "M2.1", "Gren\n-ade" },
                { "M2.2", "Ammo" },
                { "M2.3", "Cannon" },
                { "M2.4", "Bayo\n-net" },
                { "M2.5", "Ambi\n-dex" },
                { "M2.6", "Tool" },
                { "P1.1", "" }, // Quantum Flux
                { "P1.2", "" },
                { "P1.3", "Psi\nBlast" }, //?
                { "P1.4", "Tele-\nkinesis" },
                { "P2.1", "" },
                { "P2.2", "Psi\nShield" }, //?
                { "P2.3", "" },
                { "P2.4", "Throw\nElement" }, //?
                { "P2.5", "" },
                { "P2.6", "" },
                { "S1.1", "" },
                { "S1.2", "Robo\n-tics" },
                { "S1.3", "Biology" },
                { "S1.4", "" },
                { "S2.1", "" },
                { "S2.2", "Cyber\n-netics" },
                { "S2.3", "" },
                { "S2.4", "" },
                { "S2.5", "" },
                { "S2.6", "" },
                { "AM3.1", "Cloak" },
                { "AM3.2", "Run\n+\nGun" },
                { "AM4.1", "" },
                { "AP3.1", "" },
                { "AP3.2", "Throw\nMass" },
                { "AP4.1", "" },
                { "AS3.1", "" },
                { "AS3.2", "Data\nJack" },
                { "AS4.1", "Quantum\nSim" },
                { "MP3.1", "" },
                { "MP3.2", "Third\nHand" },
                { "MP4.1", "Reach" },
                { "MS3.1", "" },
                { "MS3.2", "" },
                { "MS4.1", "" },
                { "PS3.1", "" },
                { "PS3.2", "" },
                { "PS4.1", "Regen" },
            };
        }

        /// <summary>
        /// Sets the text to display in each circle.
        /// </summary>
        private void SetBlankCircleText()
        {
            CircleText = new Dictionary<string, string> {
                { "A1.1", "A1" },
                { "A1.2", "A2" },
                { "A1.3", "A3" },
                { "A1.4", "A4" },
                { "A2.1", "A12" },
                { "A2.2", "A23" },
                { "A2.3", "A13" },
                { "A2.4", "A34" },
                { "A2.5", "A14" },
                { "A2.6", "A24" },
                { "M1.1", "M1" },
                { "M1.2", "M2" },
                { "M1.3", "M3" },
                { "M1.4", "M4" },
                { "M2.1", "M12" },
                { "M2.2", "M23" },
                { "M2.3", "M13" },
                { "M2.4", "M34" },
                { "M2.5", "M14" },
                { "M2.6", "M24" },
                { "P1.1", "P1" },
                { "P1.2", "P2" },
                { "P1.3", "P3" },
                { "P1.4", "P4" },
                { "P2.1", "P12" },
                { "P2.2", "P23" },
                { "P2.3", "P13" },
                { "P2.4", "P34" },
                { "P2.5", "P14" },
                { "P2.6", "P24" },
                { "S1.1", "S1" },
                { "S1.2", "S2" },
                { "S1.3", "S3" },
                { "S1.4", "S4" },
                { "S2.1", "S12" },
                { "S2.2", "S23" },
                { "S2.3", "S13" },
                { "S2.4", "S34" },
                { "S2.5", "S14" },
                { "S2.6", "S24" },
                { "AM3.1", "A12\n+\nM24" },
                { "AM3.2", "A23\n+\nM14" },
                { "AM4.1", "AM\nMax" },
                { "AP3.1", "A34\n+\nP13" },
                { "AP3.2", "A13\n+\nP34" },
                { "AP4.1", "AP\nMax" },
                { "AS3.1", "A24\n+\nS12" },
                { "AS3.2", "A14\n+\nS23" },
                { "AS4.1", "AS\nMax" },
                { "MP3.1", "M34\n+\nP14" },
                { "MP3.2", "M13\n+\nP24" },
                { "MP4.1", "MP\nMax" },
                { "MS3.1", "M23\n+\nS14" },
                { "MS3.2", "M12\n+\nS24" },
                { "MS4.1", "MS\nMax" },
                { "PS3.1", "P12\n+\nS34" },
                { "PS3.2", "P23\n+\nS13" },
                { "PS4.1", "PS\nMax" },
            };
        }

        /// <summary>
        /// Creates circles and arrows for the central field.
        /// </summary>
        /// <param name="field">The skill's field.</param>
        private void Field1(string field)
        {
            float deg;
            int i;
            Circle t2, t1s1 = CreateCircle(field, 1, 1, new PointF(0, 0));
            for (deg = -60, i = 2; i <= 4; deg += 120, i++)
            {
                CreateCircle(field, 1, i, Vector.Polar(t1s1, deg, 0.1f));
            }
            // Skill 1 tier 2
            for (deg = 0, i = 2; i <= 4; deg += 120, i++)
            {
                t2 = CreateCircle(field, 2, i * 2 - 3, Vector.Polar(t1s1, deg - 15f, 0.22f));
                CreateArrow(t1s1, t2);
                CreateArrow(Circles[Id(field, 1, i)], t2);

                t2 = CreateCircle(field, 2, i * 2 - 2, Vector.Polar(t1s1, deg + 15f, 0.22f));
                CreateArrow(Circles[Id(field, 1, i)], t2);
                CreateArrow(Circles[Id(field, 1, i < 4 ? i + 1 : i - 2)], t2);
            }
            // Area
            Areas.Add(Id(field, 2), new Circle
            {
                Centre = t1s1,
                Radius = 0.265f,
                Brush = new SolidBrush(_areaColour[field]),
                Pen = _areaPen,
            });
            Areas.Add(Id(field, 1), new Circle
            {
                Centre = t1s1,
                Radius = 0.140f,
                Pen = _areaPen,
            });
            Labels.Add(field, new Label {
                Text = _labelText[field],
                Centre = Vector.Polar(t1s1, 180f, 0.20f),
                Rotation = 0f,
                Brush = _textBrush,
                Font = _labelFont,
            });
        }

        /// <summary>
        /// Creates circles and arrows for a primary field.
        /// </summary>
        /// <param name="field">The field identifier.</param>
        /// <param name="deg1">The field's direction.</param>
        private void Tier12(string field, PointF origin, float deg1)
        {
            var Root2 = (float)Math.Sqrt(2.0);
            int i;
            float deg2;
            // Tier 1
            Circle t2, t1s1 = CreateCircle(field, 1, 1, Vector.Polar(origin, deg1, 1.0f));
            for (i = 2, deg2 = deg1 + 157.5f; i <= 4; i++, deg2 += 30f)
            {
                CreateCircle(field, 1, i, Vector.Polar(t1s1, deg2, i > 2 ? 0.35f : 0.23f));
            }
            // Tier 2
            for (i = 2, deg2 = deg1 + 150f; i <= 4; i++, deg2 += 30f)
            {
                t2 = CreateCircle(field, 2, i * 2 - 3, Vector.Polar(t1s1, deg2 - 7.5f, 0.5f));
                CreateArrow(t1s1, t2);
                CreateArrow(Circles[Id(field, 1, i)], t2);

                t2 = CreateCircle(field, 2, i * 2 - 2, Vector.Polar(t1s1, deg2 + 7.5f, 0.5f));
                CreateArrow(Circles[Id(field, 1, i)], t2);
                CreateArrow(Circles[Id(field, 1, i < 4 ? i + 1 : i - 2)], t2);
            }
            // Area
            var areaCentre = Vector.Polar(t1s1, deg1, Radius[1] * Root2);
            Areas.Add(Id(field, 2), new Pie
            {
                Centre = areaCentre,
                Radius = 0.56f + Radius[1],
                StartAngle = deg1 + 135f,
                SweepAngle = 90f,
                Brush = new SolidBrush(_areaColour[field]),
                Pen = _areaPen,
            });
            Areas.Add(Id(field, 1), new Arc
            {
                Centre = areaCentre,
                Radius = 0.41f + Radius[1],
                StartAngle = deg1 + 135f,
                SweepAngle = 90f,
                Pen = _areaPen,
            });
            Labels.Add(field, new Label {
                Text = _labelText[field],
                Centre = Vector.Polar(t1s1, deg1 + 217.5f, 0.195f),
                Rotation = UprightDeg(deg1 + 127.5f),
                Brush = _textBrush,
                Font = _labelFont,
            });
        }

        /// <summary>
        /// Creates circles and arrows for a secondary field.
        /// </summary>
        /// <param name="f1s1">Field 1 skill 1.</param>
        /// <param name="f1s2">Field 1 skill 2.</param>
        /// <param name="f2s1">Field 2 skill 1.</param>
        /// <param name="f2s2">Field 2 skill 2.</param>
        private void Tier34(string field, Circle f1s1, Circle f1s2, Circle f2s1, Circle f2s2, float deg)
        {
            var t4s1 = CreateCircle(field, 4, 1, Vector.Average(f1s1, f1s2, f2s1, f2s2));
            var t3s1 = CreateCircle(field, 3, 1, Vector.Polar(t4s1, deg - 90, Radius[3] * 3f));
            var t3s2 = CreateCircle(field, 3, 2, Vector.Polar(t4s1, deg + 90, Radius[3] * 3f));
            CreateArrow(f1s1, t3s1);
            CreateArrow(f2s1, t3s1);
            CreateArrow(f1s2, t3s2);
            CreateArrow(f2s2, t3s2);
            CreateArrow(t3s1, t4s1);
            CreateArrow(t3s2, t4s1);
            // Area
            Labels.Add(field, new Label {
                Text = _labelText[field],
                Centre = Vector.Polar(t4s1, deg, Radius[4] + 0.02f),
                Rotation = UprightDeg(deg),
                Brush = _textBrush,
                Font = _labelFont,
            });
        }
    }
}
