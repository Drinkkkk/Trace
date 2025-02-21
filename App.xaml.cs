using System.Configuration;
using System.Data;
using System.Windows;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using NLog;
using Prism.DryIoc;
using Prism.Ioc;
using Trace.Common;
using Trace.Service;
using Trace.Service.HttpClient;
using Trace.Service.IService;
using Trace.ViewModels;
using Trace.ViewModels.DialogViewModels;
using Trace.Views;
using Trace.Views.DialogViews;
namespace Trace
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }
        public static void LoginOut(IContainerProvider containerProvider)
        {
            Current.MainWindow.Hide();
            _logger.Info($"{AppSession.UserName}登出了");
            var dialog = containerProvider.Resolve<IDialogService>();

            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }

                Current.MainWindow.Show();
            });
        }
        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", callback =>
            {
                if(callback.Result!=ButtonResult.OK)
                {
                    Application.Current.Shutdown();
                    return;
                }

                var service = App.Current.MainWindow.DataContext as IConfigureService;
                if (service != null)
                {
                    service.Configure();
                }
                base.OnInitialized();
            });

          
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //注册HttpRestClient实例
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "weburl"));
            containerRegistry.GetContainer().RegisterInstance(@"http://localhost:5888/", serviceKey: "weburl");

            //Http服务      
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<ITruckService, TruckService>();
            containerRegistry.Register<ITripService, TripService>();
            containerRegistry.Register<ICoordinateService, CoordinateService>();

            //V-VM
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<TruckView, TruckViewModel>();
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<MissionView, MissionViewModel>();
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>();
            containerRegistry.RegisterForNavigation<UserView, UserViewModel>();
           


            //单例  
            containerRegistry.RegisterSingleton<LoadingView>();

            //自定义弹窗服务
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.RegisterForNavigation<AddTruckView, AddTruckViewModel>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<AddTripView, AddTripViewModel>();
            containerRegistry.RegisterForNavigation<testView, ViewModels.DialogViewModels.testViewModel>();
            //官方弹窗
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();
        }
    }

}
