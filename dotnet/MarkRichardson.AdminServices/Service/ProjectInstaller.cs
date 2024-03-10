using System.ComponentModel;
using System.Configuration.Install;

namespace MarkRichardson.AdminServices
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
