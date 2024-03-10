using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NodeNetwork.Designer
{
    public partial class NodeNetworkDesignerPanel : UserControl
    {
        private const float ZoomIncrement = 1.1f;

        private readonly Palette Palette = new Palette();

        private LabelEditor labelEditor;

        private Mode _mode = Mode.None;

        private Point _mouseAnchor;

        private Node SelectedNode
        {
            get => _selectedNode;
            set => _selectedNode = value;
        }
        private Node _selectedNode = null;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Map Map
        {
            get => _map;
            set
            {
                _map = value ?? new Map();
                Invalidate();
            }
        }
        private Map _map = new Map();

        private PointF MouseLocation
        {
            get
            {
                var viewPort = ViewPort;
                var pixelLocation = PointToClient(MousePosition);
                var viewPortLocation = new PointF(
                    (pixelLocation.X - ClientRectangle.X) / (float)ClientRectangle.Width * viewPort.Width + viewPort.X,
                    (pixelLocation.Y - ClientRectangle.Y) / (float)ClientRectangle.Height * viewPort.Height + viewPort.Y
                );
                return viewPortLocation;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointF Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                Invalidate();
            }
        }
        private PointF _origin = new PointF(0f, 0f);

        public RectangleF ViewPort => CenteredRectangleF(Origin, new SizeF(ClientSize.Width / Zoom, ClientSize.Height / Zoom));

        [DefaultValue(DefaultZoom)]
        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                Invalidate();
            }
        }
        private float _zoom = DefaultZoom;
        const float DefaultZoom = 1f;

        public NodeNetworkDesignerPanel()
        {
            InitializeComponent();

            labelEditor = new LabelEditor { Visible = false };
            labelEditor.Validated += LabelEditor_Validated;
            Controls.Add(labelEditor);

            MouseWheel += NodeNetworkDesignerPanel_MouseWheel;
        }

        private void NodeNetworkDesignerPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var clientRectangle = new RectangleF(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            var viewPort = ViewPort;
            var graphicsContainer = g.BeginContainer(clientRectangle, viewPort, GraphicsUnit.Pixel);

            var labels = new List<TextLabel>();

            foreach (var bridge in Map.Bridges)
            {
                var points = bridge.Nodes
                    .Select(node => node.Location)
                    .ToArray();

                g.DrawLine(Palette.BridgeNormal, points[0], points[1]);

                labels.Add(new TextLabel
                {
                    Label = bridge.Label,
                    Location = new PointF(points.Average(point => point.X), points.Average(point => point.Y)),
                });
            }

            if (_mode == Mode.CreateBridge)
                g.DrawLine(Palette.BridgeCreate, _selectedNode.Location, MouseLocation);

            var hoverNode = GetNodeAt(MouseLocation);
            foreach (var node in Map.Nodes)
            {
                var bounds = CenteredRectangleF(node.Location, Palette.NodeSize);
                if (bounds.IntersectsWith(viewPort))
                {
                    Brush brush = node == hoverNode
                        ? Palette.NodeHover
                        : Palette.NodeNormal;

                    g.FillEllipse(brush, bounds);
                }

                labels.Add(new TextLabel
                {
                    Label = node.Label,
                    Location = node.Location,
                });
            }

            foreach (var label in labels)
            {
                if (CenteredRectangleF(label.Location, g.MeasureString(label.Label, Palette.Label)).IntersectsWith(viewPort))
                    g.DrawString(label.Label, Palette.Label, label.Location);
            }

            g.EndContainer(graphicsContainer);
        }

        private RectangleF CenteredRectangleF(PointF location, SizeF size) => new RectangleF(location.X - size.Width / 2f, location.Y - size.Height / 2f, size.Width, size.Height);

        private void NodeNetworkDesignerPanel_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void NodeNetworkDesignerPanel_MouseDown(object sender, MouseEventArgs e)
        {
            switch (_mode)
            {
                case Mode.EditNodeLabel:
                case Mode.EditBridgeLabel:
                    EndEditLabelMode();
                    break;

                default:
                    switch (MouseButtons)
                    {
                        case MouseButtons.Left:
                            LeftMouseDown();
                            break;
                        case MouseButtons.Right:
                            StartPanCanvasMode();
                            break;
                        case MouseButtons.Left | MouseButtons.Right:
                            StartMoveNodeMode();
                            break;
                        default:
                            ClearMode();
                            break;
                    }
                    break;
            }
        }

        private void LeftMouseDown()
        {
            var mouseLocation = MouseLocation;
            var hoverNode = GetNodeAt(mouseLocation);

            if (hoverNode != null)
                StartCreateBridgeMode(hoverNode);
            else
                CreateNode(mouseLocation);
        }

        private void StartCreateBridgeMode(Node hoverNode)
        {
            SelectedNode = hoverNode;
            _mode = Mode.CreateBridge;
        }

        private void StartPanCanvasMode()
        {
            _mouseAnchor = MousePosition;
            _mode = Mode.PanCanvas;
        }

        private void StartMoveNodeMode()
        {
            var hoverNode = GetNodeAt(MouseLocation);
            if (hoverNode != null && (SelectedNode == null || SelectedNode == hoverNode))
            {
                _mode = Mode.MoveNode;
                SelectedNode = hoverNode;
            }
            else
            {
                ClearMode();
            }
        }

        private void NodeNetworkDesignerPanel_MouseMove(object sender, MouseEventArgs e)
        {
            switch (_mode)
            {
                case Mode.PanCanvas:
                    ActionPanCanvasMode();
                    break;
                case Mode.MoveNode:
                    ActionMoveNodeMode();
                    break;
            }
            Invalidate();
        }

        private void ActionPanCanvasMode()
        {
            Origin = new PointF(
                Origin.X - (MousePosition.X - _mouseAnchor.X) / Zoom,
                Origin.Y - (MousePosition.Y - _mouseAnchor.Y) / Zoom
            );

            _mouseAnchor = MousePosition;
        }

        private void ActionMoveNodeMode()
        {
            SelectedNode.Location = MouseLocation;
        }

        private void NodeNetworkDesignerPanel_MouseUp(object sender, MouseEventArgs e)
        {
            switch (_mode)
            {
                case Mode.CreateBridge:
                    EndCreateBridgeMode(e.Location);
                    break;
                case Mode.PanCanvas:
                    ClearMode();
                    break;
                case Mode.MoveNode:
                    ClearMode();
                    break;
            }
        }

        private void EndCreateBridgeMode(Point position)
        {
            var hoverNode = GetNodeAt(MouseLocation);
            if (hoverNode == null)
                ClearMode();
            else if (hoverNode == SelectedNode)
                EditNodeLabel(position);
            else
                CreateBridge(hoverNode);

            Invalidate();
        }

        private void ClearMode()
        {
            SelectedNode = null;
            _mode = Mode.None;
        }

        private void CreateNode(PointF location)
        {
            var node = new Node { Location = location };
            Map.Nodes.Add(node);
            Invalidate();
        }

        private void CreateBridge(Node hoverNode)
        {
            var bridge = new Bridge();
            bridge.LinkNodes(SelectedNode, hoverNode);

            ClearMode();
        }

        private void EditNodeLabel(Point location)
        {
            labelEditor.Text = SelectedNode.Label;
            SelectedNode.Label = null;
            labelEditor.Center = location;
            labelEditor.Show();
            labelEditor.SelectAll();
            labelEditor.Focus();

            _mode = Mode.EditNodeLabel;
        }

        private Node GetNodeAt(PointF location)
        {
            foreach (var node in Map.Nodes)
            {
                var dx = node.Location.X - location.X;
                var dy = node.Location.Y - location.Y;
                if (dx * dx + dy * dy <= Palette.NodeRadius * Palette.NodeRadius)
                    return node;
            }
            return null;
        }

        private void NodeNetworkDesignerPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            Zoom = Zoom * (float)Math.Pow(ZoomIncrement, e.Delta / 120.0);
        }

        private void LabelEditor_Validated(object sender, EventArgs e)
        {
            EndEditLabelMode();
        }

        private void EndEditLabelMode()
        {
            switch (_mode)
            {
                case Mode.EditNodeLabel:
                    _selectedNode.Label = labelEditor.Text == "" ? null : labelEditor.Text;
                    break;
                case Mode.EditBridgeLabel:
                    _selectedNode.Label = labelEditor.Text == "" ? null : labelEditor.Text;
                    break;
            }

            labelEditor.Hide();
            ClearMode();
        }

        private void NodeNetworkDesignerPanel_Disposed(object sender, EventArgs e)
        {
            Palette.Dispose();
        }

        private enum Mode
        {
            None,
            PanCanvas,
            CreateBridge,
            MoveNode,
            EditNodeLabel,
            EditBridgeLabel,
        }
    }
}
