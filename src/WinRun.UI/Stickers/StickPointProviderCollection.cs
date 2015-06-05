using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.UI.Stickers
{
    public class StickPointProviderCollection : IStickPointProvider
    {
        private readonly List<IStickPointProvider> providers = new List<IStickPointProvider>();

        public StickPointProviderCollection AddProvider(IStickPointProvider provider)
        {
            providers.Add(provider);
            return this;
        }

        public IEnumerable<StickPoint> ForTop()
        {
            return providers.SelectMany(p => p.ForTop());
        }

        public IEnumerable<StickPoint> ForBottom()
        {
            return providers.SelectMany(p => p.ForBottom());
        }

        public IEnumerable<StickPoint> ForLeft()
        {
            return providers.SelectMany(p => p.ForLeft());
        }

        public IEnumerable<StickPoint> ForRight()
        {
            return providers.SelectMany(p => p.ForRight());
        }
    }
}
