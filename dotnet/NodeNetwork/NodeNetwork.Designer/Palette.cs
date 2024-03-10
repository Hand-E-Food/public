using System;
using System.Drawing;

namespace NodeNetwork.Designer
{
    internal class Palette : IDisposable
    {
        public static readonly Color LabelColor = Color.Black;

        public readonly Pen BridgeCreate = new Pen(Color.Yellow, 5f);

        public readonly Pen BridgeNormal = new Pen(Color.White, 5f);

        public readonly Brush NodeHover = new SolidBrush(Color.Yellow);

        public readonly Brush NodeNormal = new SolidBrush(Color.White);

        public const float NodeRadius = 25f;

        public readonly SizeF NodeSize = new SizeF(NodeRadius * 2f, NodeRadius * 2f);

        public TextStyle Label = new TextStyle(
            new Font("Arial", 10f),
            new SolidBrush(LabelColor),
            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
        );

        public void Dispose()
        {
            BridgeCreate.Dispose();
            BridgeNormal.Dispose();
            NodeHover.Dispose();
            NodeNormal.Dispose();
            Label.Dispose();
        }
    }
}