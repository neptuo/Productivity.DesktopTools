using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace WinRun.Stickers.UI
{
    /// <summary>
    /// Interaction logic for StickVisualizationWindow.xaml
    /// </summary>
    public partial class StickVisualizationWindow : Window
    {
        public StickVisualizationWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            List<SolidColorBrush> colors = new List<SolidColorBrush>()
            {
                Brushes.Red,
                Brushes.LightGreen,
                Brushes.Purple,
                Brushes.LightPink,
                Brushes.Blue,
                Brushes.Brown,
                Brushes.Black,
            };

            cnvMain.Children.Clear();

            VisibleWindowStickPointProvider provider = new VisibleWindowStickPointProvider(0);
            int index = 0;
            foreach (IntPtr handle in provider.GetTopLevelWindows())
            {
                Win32.RECT info;
                if (Win32.GetWindowRect(handle, out info))
                {
                    Win32.RECT frame;
                    Win32.DwmGetWindowAttribute(handle, (int)Win32.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out frame, Marshal.SizeOf(typeof(Win32.RECT)));

                    info = frame;
                    //info.Left += frame.Left;
                    //info.Top += frame.Top;
                    //info.Right -= frame.Right;
                    //info.Bottom -= frame.Bottom;

                    Rectangle rect = new Rectangle
                    {
                        Width = info.Right - info.Left,
                        Height = info.Bottom - info.Top,
                        Stroke = colors[index++ % colors.Count],
                        StrokeThickness = 2,
                    };
                    rect.MouseDown += Rect_MouseDown;
                    rect.Tag = handle;

                    Canvas.SetLeft(rect, info.Left);
                    Canvas.SetTop(rect, info.Top);
                    cnvMain.Children.Add(rect);
                }
            }
        }

        private void Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            IntPtr handle = (IntPtr)rect.Tag;

            StringBuilder className = new StringBuilder(1024);
            Win32.GetClassName(handle, className, className.Capacity);
            string classNameLower = className.ToString().ToLowerInvariant();

            StringBuilder text = new StringBuilder(1024);
            Win32.GetWindowText(handle, text, text.Capacity);
            string textLower = text.ToString().ToLowerInvariant();

            MessageBox.Show(String.Format("Window: '{0}' -> '{1}'", className, text.ToString()));
        }
    }
}
