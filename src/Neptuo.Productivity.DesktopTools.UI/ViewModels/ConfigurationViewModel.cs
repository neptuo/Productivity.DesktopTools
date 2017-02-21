using Neptuo.Observables;
using Neptuo.Productivity.DesktopTools.Services.StartupShortcuts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DesktopTools.ViewModels
{
    public class ConfigurationViewModel : ObservableObject
    {
        private readonly ShortcutService service;

        private bool isAutoStartup;
        public bool IsAutoStartup
        {
            get { return isAutoStartup; }
            set
            {
                if (isAutoStartup != value)
                {
                    isAutoStartup = value;
                    RaisePropertyChanged();

                    if (value)
                        service.Create(Environment.SpecialFolder.Startup);
                    else
                        service.Delete(Environment.SpecialFolder.Startup);
                }
            }
        }

        public ConfigurationViewModel(ShortcutService service)
        {
            this.service = service;
            isAutoStartup = service.Exists(Environment.SpecialFolder.Startup);
        }
    }
}
