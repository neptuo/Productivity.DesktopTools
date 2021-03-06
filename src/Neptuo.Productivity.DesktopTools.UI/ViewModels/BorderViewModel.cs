﻿using Neptuo.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DesktopTools.ViewModels
{
    public class BorderViewModel : ObservableObject
    {
        private int size;
        public int Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CorderRadius));
                }
            }
        }

        public int CorderRadius
        {
            get { return Size / 2; }
        }
    }
}
