using Neptuo.Productivity.DevTools.ViewModels;
using Neptuo.Productivity.DevTools.Views.Controls;
using Neptuo.Windows.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public DispatcherHelper DispatcherHelper { get; private set; }

        internal MainViewModel ViewModel
        {
            get { return (MainViewModel)DataContext; }
            set
            {
                DataContext = value;
                OnViewModelChanged(value);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            DispatcherHelper = new DispatcherHelper(Dispatcher);
            Background = new SolidColorBrush(Colors.Transparent);
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!ViewModel.IsExpanded && WindowDrag.TryMove(e))
            {
                DragMove();

                ViewModel.Settings.Vertical = GetVerticalOrientation();
                ViewModel.Settings.Horizontal = GetHorizontalOrientation();
            }
        }

        private void OnViewModelChanged(MainViewModel viewModel)
        {
            viewModel.Settings.PropertyChanged += OnViewModelSettingsChanged;
        }

        private async void OnViewModelSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainSettingsViewModel.Horizontal))
            {
                await Task.Delay(10);
                double diff = ActualWidth - brdMainButton.ActualWidth - 7;
                switch (ViewModel.Settings.Horizontal)
                {
                    case HorizontalOrientation.Left:
                        Left += diff;
                        break;
                    case HorizontalOrientation.Right:
                        Left -= diff;
                        break;
                }
            }
            else if(e.PropertyName == nameof(MainSettingsViewModel.Vertical))
            {
                await Task.Delay(10);
                double diff = ActualHeight - brdMainButton.ActualHeight + 5;
                switch (ViewModel.Settings.Vertical)
                {
                    case VerticalOrientation.Top:
                        Top += diff;
                        break;
                    case VerticalOrientation.Bottom:
                        Top -= diff;
                        break;
                }
            }
        }

        private VerticalOrientation GetVerticalOrientation()
        {
            WpfScreen screen = WpfScreen.GetScreenFrom(this);
            if (Top > (screen.DeviceBounds.Height / 2))
                return VerticalOrientation.Bottom;

            return VerticalOrientation.Top;
        }

        private HorizontalOrientation GetHorizontalOrientation()
        {
            WpfScreen screen = WpfScreen.GetScreenFrom(this);
            if (Left > (screen.DeviceBounds.Width / 2))
                return HorizontalOrientation.Right;

            return HorizontalOrientation.Left;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (ViewModel.IsExpanded)
                    ViewModel.IsExpanded = false;
                else
                    Close();
            }
        }
    }
}
