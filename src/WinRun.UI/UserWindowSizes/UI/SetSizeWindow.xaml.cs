﻿using System;
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
using WinRun.UserWindowSizes.UI.ViewModels;

namespace WinRun.UserWindowSizes.UI
{
    /// <summary>
    /// Interaction logic for SetSizeWindow.xaml
    /// </summary>
    public partial class SetSizeWindow : Window
    {
        public SetSizeViewModel ViewModel
        {
            get { return (SetSizeViewModel)DataContext; }
            set { DataContext = value; }
        }

        public SetSizeWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            tbxLeft.Focus();
        }
    }
}
