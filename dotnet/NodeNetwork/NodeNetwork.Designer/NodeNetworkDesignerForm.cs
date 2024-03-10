using System;
using System.IO;
using System.Windows.Forms;

namespace NodeNetwork.Designer
{
    public partial class NodeNetworkDesignerForm : Form
    {

        public NodeNetworkDesignerForm()
        {
            InitializeComponent();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            nodeNetworkDesignerPanel.Map = new Map();
            saveFileDialog.FileName = null;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog.FileName;
                var json = File.ReadAllText(path);
                nodeNetworkDesignerPanel.Map = NodeNetworkSerializer.Deserialize(json);
                saveFileDialog.FileName = openFileDialog.FileName;
            }
        }

        private void saveButon_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = saveFileDialog.FileName;
                var map = nodeNetworkDesignerPanel.Map;
                var json = NodeNetworkSerializer.Serialize(map);
                File.WriteAllText(path, json);
            }
        }
    }
}
