using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRun.Stickers
{
    public class StickPointProviderCollection : IStickPointProvider
    {
        private readonly List<IStickPointProvider> providers = new List<IStickPointProvider>();
        private readonly List<IStickPointDecorator> decorators = new List<IStickPointDecorator>();

        public StickPointProviderCollection AddProvider(IStickPointProvider provider)
        {
            Ensure.NotNull(provider, "provider");
            providers.Add(provider);
            return this;
        }

        public StickPointProviderCollection AddDecorator(IStickPointDecorator decorator)
        {
            Ensure.NotNull(decorator, "decorator");
            decorators.Add(decorator);
            return this;
        }

        private StickPoint DecorateTopInternal(StickPoint point)
        {
            foreach (IStickPointDecorator decorator in decorators)
                point = decorator.DecorateTop(point);

            return point;
        }

        private StickPoint DecorateBottomInternal(StickPoint point)
        {
            foreach (IStickPointDecorator decorator in decorators)
                point = decorator.DecorateBottom(point);

            return point;
        }

        private StickPoint DecorateLeftInternal(StickPoint point)
        {
            foreach (IStickPointDecorator decorator in decorators)
                point = decorator.DecorateLeft(point);

            return point;
        }

        private StickPoint DecorateRightInternal(StickPoint point)
        {
            foreach (IStickPointDecorator decorator in decorators)
                point = decorator.DecorateRight(point);

            return point;
        }

        public IEnumerable<StickPoint> ForTop()
        {
            return providers.SelectMany(p => p.ForTop()).Select(DecorateTopInternal);
        }

        public IEnumerable<StickPoint> ForBottom()
        {
            return providers.SelectMany(p => p.ForBottom()).Select(DecorateBottomInternal);
        }

        public IEnumerable<StickPoint> ForLeft()
        {
            return providers.SelectMany(p => p.ForLeft()).Select(DecorateLeftInternal);
        }

        public IEnumerable<StickPoint> ForRight()
        {
            return providers.SelectMany(p => p.ForRight()).Select(DecorateRightInternal);
        }
    }
}
