using Neptuo.Productivity.DevTools.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DevTools.Views.DesignData
{
    internal class ViewModelLocator
    {
        private MainViewModel main;

        public MainViewModel Main
        {
            get
            {
                if (main == null)
                {
                    main = new MainViewModel();
                    main.IsExpanded = true;
                    main.Settings.Size = 60;
                }

                return main;
            }
        }
    }
}
