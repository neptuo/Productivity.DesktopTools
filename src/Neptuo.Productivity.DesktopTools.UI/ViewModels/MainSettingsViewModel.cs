using Neptuo.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DesktopTools.ViewModels
{
    public class MainSettingsViewModel : ObservableObject
    {
        public BorderViewModel MainBorder { get; private set; }
        public BorderViewModel CommandBorder { get; private set; }

        private VerticalOrientation vertical;
        public VerticalOrientation Vertical
        {
            get { return vertical; }
            set
            {
                if (vertical != value)
                {
                    vertical = value;
                    RaisePropertyChanged();
                }
            }
        }

        private HorizontalOrientation horizontal;
        public HorizontalOrientation Horizontal
        {
            get { return horizontal; }
            set
            {
                if (horizontal != value)
                {
                    horizontal = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MainSettingsViewModel()
        {
            MainBorder = new BorderViewModel();
            CommandBorder = new BorderViewModel();
        }
    }
}
