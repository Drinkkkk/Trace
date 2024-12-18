using DryIoc;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Trace.Common
{
    //自定义对话主机服务
    public class DialogHostService : DialogService, IDialogHostService
    {
        private readonly IContainerExtension containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameter, string dialogHostname = "")
        {
            parameter ??= new DialogParameters();

            //从容器中拿出窗口实例
            var content = containerExtension.Resolve<object>(name);

            //验证实例有效性
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");
            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                ViewModelLocator.SetAutoWireViewModel(view, true);

            }
            if (!(dialogContent.DataContext is IDialogHostAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");


            viewModel.DialogHostName = dialogHostname;

            //↓位于md中
            DialogOpenedEventHandler handler = (sender, args) =>
            {
                if(viewModel is IDialogHostAware aware)
                {
                    aware.OnDialogOpen(parameter);
                }
                args.Session.UpdateContent(content);
            };
            return (IDialogResult)await DialogHost.Show(dialogContent, viewModel.DialogHostName, handler);
        }
    }
}
