using MaterialDesignThemes.Wpf;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trace.Common;
using Trace.Common.Extensions;
using Trace.ViewModels;
using Trace.Views;

namespace Trace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly LoadingView loadingView;
        private readonly IDialogHostService service;

        public MainView(IEventAggregator eventAggregator, LoadingView loadingView,IDialogHostService Service)
        {
            InitializeComponent();
            service = Service;
            eventAggregator.Register(x =>
            {
                DialogHost.IsOpen = x.IsOpen;
                if (DialogHost.IsOpen)
                {
                    DialogHost.DialogContent = this.loadingView;
                }
            });
            eventAggregator.RegisterMessage(arg =>
            {
                Snackbar.MessageQueue.Enqueue(arg.Message);
            }, "Main");
            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };
            btnClose.Click += async (s, e) =>
            {
                 var dialogResult = await Service.Question("温馨提示", "确认退出系统?");
                 if (dialogResult.Result != ButtonResult.OK) return;

              
                Application.Current.Shutdown();


            };
            ColorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };

            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };

            menuBar.SelectionChanged += (s, e) => { drawerHost.IsLeftDrawerOpen = false; };
            this.loadingView = loadingView;
          
        }
    }
}