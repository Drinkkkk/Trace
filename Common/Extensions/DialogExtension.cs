using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common.Events;
using Trace.Views;

namespace Trace.Common.Extensions
{
    public static class DialogExtension
    {
        public static void UpdataLoding(this IEventAggregator aggregator,UpdataModel updataModel)
        {
            aggregator.GetEvent<UpdataLoadingEvent>().Publish(updataModel);
        }

        public static void Register(this IEventAggregator aggregator, Action<UpdataModel> action)
        {
            aggregator.GetEvent<UpdataLoadingEvent>().Subscribe(action);
        }

        public static async Task<IDialogResult> Question(this IDialogHostService Service,string Title,string Content,string DialogHostName="Root")
        {
            var param = new DialogParameters();
            param.Add("Title",Title );
            param.Add("Content", Content);
            param.Add("DialogHostName",DialogHostName);
            return await Service.ShowDialog("MsgView", param, DialogHostName);
        }
    }
}
