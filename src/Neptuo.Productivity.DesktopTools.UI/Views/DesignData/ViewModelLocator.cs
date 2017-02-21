using FontAwesome.WPF;
using Neptuo.Productivity.DesktopTools.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DesktopTools.Views.DesignData
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
                    //main.Settings.Horizontal = HorizontalOrientation.Right;
                    main.Settings.MainBorder.Size = 60;
                    main.Settings.CommandBorder.Size = 40;

                    main.Commands.Add(new CommandViewModel(FontAwesomeIcon.Cog, "Application settings", new Command()));
                    main.Commands.Add(new CommandViewModel(FontAwesomeIcon.AlignLeft, "Log", new Command()));
                }

                return main;
            }
        }
    }
}
