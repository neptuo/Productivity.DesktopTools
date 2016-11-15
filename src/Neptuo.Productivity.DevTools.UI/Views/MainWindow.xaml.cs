using Neptuo.Productivity.DevTools.ViewModels;
using Neptuo.Productivity.DevTools.Views.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Neptuo.Productivity.DevTools.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel
        {
            get { return (MainViewModel)DataContext; }
            set { DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowDrag.TryMove(e))
                DragMove();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
