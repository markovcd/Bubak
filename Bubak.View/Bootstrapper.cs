using Bubak.Client;
using Bubak.Shared.Misc;
using Bubak.ViewModel;
using Caliburn.Micro;
using Ninject;

namespace Bubak.View
{
    public static class Bootstrapper
    {
        public static ITorrentsViewModel Bootstrap()
        {
            IKernel kernel = new StandardKernel();

            kernel.Bind<ILogger>().To<DebugLogger>();
            kernel.Bind<IEventAggregator>().To<EventAggregator>();

            kernel.Bind<ITorrentClient>().To<TorrentClient>();
            kernel.Bind<ITorrentWrapper>().To<TorrentWrapper>();
            kernel.Bind<ITorrentWrapperFactory>().To<TorrentWrapperFactory>();
            kernel.Bind<ITorrentsViewModel>().To<TorrentsViewModel>();
          
           return kernel.Get<ITorrentsViewModel>();
        }
        
    }
}

