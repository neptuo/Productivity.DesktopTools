using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WinRun.UI
{
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();

            if (ApplicationDeployment.IsNetworkDeployed)
                tblVersion.Text = "v" + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(3);
            else
                tblVersion.Text = "DEBUG";
        }
    }
}
