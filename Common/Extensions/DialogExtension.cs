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

        /// <summary>
        /// 注册提示消息 
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void RegisterMessage(this IEventAggregator aggregator,
            Action<MessageModel> action, string filterName = "Main")
        {
            aggregator.GetEvent<MessageEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }

        /// <summary>
        /// 发送提示消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="message"></param>
        public static void SendMessage(this IEventAggregator aggregator, string message, string filterName = "Main")
        {
            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel()
            {
                Filter = filterName,
                Message = message,
            });
        }


        public static void Report(this IEventAggregator aggregator, GeoLocation geoLocation)
        {
            aggregator.GetEvent<MQTTEvent>().Publish(geoLocation);
        }
        public static void ReceiveReport(this IEventAggregator aggregator, Action<GeoLocation> action)
        {
            aggregator.GetEvent<MQTTEvent>().Subscribe(action, ThreadOption.UIThread, false,
                              geoLocation => geoLocation.Message == "MQTT");
        }

        public static void SendCoordinates(this IEventAggregator aggregator,CoordinatesInTrip coordinates)
        {
            aggregator.GetEvent<CoordinatesInTripEvent>().Publish(coordinates);
        }

        public static void ReceiveCoordinates(this IEventAggregator aggregator, Action<CoordinatesInTrip> action)
        {
            aggregator.GetEvent<CoordinatesInTripEvent>().Subscribe(action, ThreadOption.UIThread, false,
                              geoLocation => geoLocation.Message == "Coordinates");
        }
        public static void ReceiveNewCoordinate(this IEventAggregator aggregator, Action<CoordinatesInTrip> action)
        {
            aggregator.GetEvent<CoordinatesInTripEvent>().Subscribe(action, ThreadOption.UIThread, false,
                              geoLocation => geoLocation.Message == "AddNew");
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
