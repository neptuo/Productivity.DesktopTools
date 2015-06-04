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

        public IEnumerable<StickInfo> ForTop()
        {
            return providers.SelectMany(p => p.ForTop());
        }

        public IEnumerable<StickInfo> ForBottom()
        {
            return providers.SelectMany(p => p.ForBottom());
        }

        public IEnumerable<StickInfo> ForLeft()
        {
            return providers.SelectMany(p => p.ForLeft());
        }

        public IEnumerable<StickInfo> ForRight()
        {
            return providers.SelectMany(p => p.ForRight());
        }
    }
}
