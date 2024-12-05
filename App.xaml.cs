using System.Configuration;
using System.Data;
using System.Windows;
using Prism.DryIoc;
using Trace.Service;
using Trace.ViewModels;
using Trace.Views;
namespace Trace
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
           return Container.Resolve<MainView>();  
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //注册HttpRestClient实例
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "weburl"));
            containerRegistry.GetContainer().RegisterInstance(@"", serviceKey:"weburl");

            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<TruckView, TruckViewModel>();
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<MissionView, MissionViewModel>();
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>();
            containerRegistry.RegisterForNavigation<UserView, UserViewModel>();



            //Http服务           
            containerRegistry.Register<ITruckService, TruckService>();
        }
    }

}
