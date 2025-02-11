using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common.Extensions;

namespace Trace.ViewModels
{
    public class NavigationViewModel :BindableBase, INavigationAware
    {
        private readonly IContainerProvider container;
        public readonly IEventAggregator Aggregator;

        public NavigationViewModel(IContainerProvider container)
        {
            this.container = container;
            Aggregator = this.container.Resolve<IEventAggregator>();
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
          
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        
        }

        public void UpdataLoading(bool isopen)
        {
            Aggregator.UpdataLoding(new Common.Events.UpdataModel { IsOpen = isopen });
        }

    }
}
