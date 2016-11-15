﻿using Neptuo.Observables;
using Neptuo.Observables.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DevTools.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        public MainSettingsViewModel Settings { get; private set; }
        public CommandObservableCollection Commands { get; private set; }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
            Settings = new MainSettingsViewModel();
            Commands = new CommandObservableCollection();
        }
    }
}