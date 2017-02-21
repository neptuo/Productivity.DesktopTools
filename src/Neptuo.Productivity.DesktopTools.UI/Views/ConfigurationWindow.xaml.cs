using Neptuo.Productivity.DesktopTools.ViewModels;
using Neptuo.Productivity.DesktopTools.Views.Controls;
using System;
using System.Collections.Generic;
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

namespace Neptuo.Productivity.DesktopTools.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        internal ConfigurationViewModel ViewModel
        {
            get { return (ConfigurationViewModel)DataContext; }
            set { DataContext = value; }
        }

        public ConfigurationWindow()
        {
            InitializeComponent();
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowDrag.TryMove(e))
                DragMove();
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
