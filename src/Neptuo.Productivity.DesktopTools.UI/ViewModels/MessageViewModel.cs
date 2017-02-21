using Neptuo.Logging;
using Neptuo.Logging.Serialization;
using Neptuo.Logging.Serialization.Formatters;
using Neptuo.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DesktopTools.ViewModels
{
    public class MessageViewModel : ObservableObject, ILogSerializer
    {
        private string last;
        public string Last
        {
            get { return last; }
            set
            {
                if (last != value)
                {
                    last = value;
                    RaisePropertyChanged();
                }
            }
        }

        private readonly DefaultLogFormatter logFormatter = new DefaultLogFormatter();

        public async void Append(string scopeName, LogLevel level, object model)
        {
            // TODO: Add support for paralel processing.
            Last = logFormatter.Format(scopeName, level, model);
            await Task.Delay(3000);
            Last = null;
        }

        public bool IsEnabled(string scopeName, LogLevel level)
        {
            return true;
        }
    }
}
