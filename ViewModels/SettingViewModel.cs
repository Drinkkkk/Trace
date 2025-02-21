using MQTTnet.Extensions.ManagedClient;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trace.Dto;
using MQTTnet.Client;
using MQTTnet.Packets;
using DryIoc;
using Trace.Common.Extensions;
using Trace.Common.Events;
using Newtonsoft.Json;

namespace Trace.ViewModels
{
    public class SettingViewModel: NavigationViewModel
    {
        private IManagedMqttClient mqttClient; //mqtt客户端
       // MqttFactory factory;

        public SettingViewModel(IContainerProvider container):base(container) 
        {
            Geo = new GeoLocation();
            Client =new MqttClientModel("broker.emqx.io", "1883","WPF_Cli1");
            // ConnectionCommand = new DelegateCommand(ConnectionAsync);
            ConnectionCommand = new AsyncDelegateCommand(ConnectionAsync);
            CloseCommand = new AsyncDelegateCommand(CloseAsync);
            SubscriteCommand = new AsyncDelegateCommand(SubscriteAsync);
            PublishCommand = new AsyncDelegateCommand(PublishAsync);
            
          var factory = new MqttFactory();
            mqttClient = factory.CreateManagedMqttClient();//创建客户端对象

            //绑定断开事件
            
            mqttClient.DisconnectedAsync+=(async ee =>
            {
                WriteLog(DateTime.Now.ToString() + "与服务器之间的连接断开了");
                // 等待 5s 时间
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"重新连接服务器失败:{ex}");
                }
            });

            //绑定接收事件
            mqttClient.ApplicationMessageReceivedAsync+= aa =>
            {
                try
                {
                //Latitude: 12.3456,Longitude: 23.4567,TripID: 9,TruckID: 38
                    string msg = aa.ApplicationMessage.ConvertPayloadToString();
                    /*  Geo.Latitude = 12.3456;
                      Geo.Longitude = 23.4567;
                      Geo.TripID = 9;
                      Geo.TruckID = 38;*/
                    var parts = msg.Split(',');
                    foreach (var part in parts)
                    {
                        var keyValue = part.Split(':');
                        if (keyValue.Length == 2)
                        {
                            switch (keyValue[0])
                            {
                                case "Latitude":
                                    Geo.Latitude = double.Parse(keyValue[1]);
                                    break;
                                case "Longitude":
                                    Geo.Longitude = double.Parse(keyValue[1]);
                                    break;
                                case "TripID":
                                    Geo.TripID = int.Parse(keyValue[1]);
                                    break;
                                case "TruckID":
                                    Geo.TruckID = int.Parse(keyValue[1]);
                                    break;
                            }
                        }
                    }
                    Geo.Latitude = 12.3456;
                    Geo.Longitude = 23.4567;
                    Geo.TripID = 9;
                    Geo.TruckID = 38;
                    Geo.Message = "MQTT";
                    Aggregator.Report(Geo);
                    WriteLog(">>> 消息：" + msg + "，QoS =" + aa.ApplicationMessage.QualityOfServiceLevel + "，客户端=" + aa.ClientId + "，主题：" + aa.ApplicationMessage.Topic);
                }
                catch (Exception ex)
                {
                    WriteLog($"+ 消息 = " + ex.Message);
                }

                return Task.CompletedTask;
            };

            //绑定连接事件
            mqttClient.ConnectedAsync+= (ee) =>
            {
                WriteLog(">>> 连接到服务");
                return Task.CompletedTask;
                // return Task.CompletedTask;
            };
        }

        private async Task PublishAsync()
        {
            if (string.IsNullOrWhiteSpace(this.Topic))
            {
                WriteLog(">>> 请输入主题");
                return;
            }

            /* var message = new MqttApplicationMessageBuilder()
                 .WithTopic("Topic")
                 .WithPayload(Encoding.UTF8.GetBytes(Pubmsg))
                 .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                 .WithRetainFlag()
                 .Build();

            var result = await mqttClient.PublishAsync(message);*/
            /* var result = await mqttClient.PublishAsync(
                 this.Topic,
                 this.Pubmsg,
                 MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);//恰好一次， QoS 级别1 */

            var client = mqttClient.InternalClient;

            // 使用 IMqttClient 实例发布消息
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("test/topic") // 替换为你的主题
                .WithPayload("Hello MQTT") // 替换为你的消息内容
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            var result = await client.PublishAsync(message);
            WriteLog($">>> 主题：{Topic}，消息：{Pubmsg}，结果： {result.ReasonCode}");
        }

        private async Task SubscriteAsync()
        {
            if (string.IsNullOrWhiteSpace(Topic))
            {
                WriteLog(">>> 请输入主题");
                return;
            }
            var topicFilter = new MqttTopicFilterBuilder().WithTopic(Topic).WithAtLeastOnceQoS().Build();
            //在 MQTT 中有三种 QoS 级别： 
            //At most once(0) 最多一次
            //At least once(1) 至少一次
            //Exactly once(2) 恰好一次
            //await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(this.tbTopic.Text).WithAtMostOnceQoS().Build());//最多一次， QoS 级别0
            await mqttClient.SubscribeAsync(new[] { topicFilter });//恰好一次， QoS 级别1 
           // await mqttClient.SubscribeAsync("testtopic");
            WriteLog($">>> 成功订阅 {this.Topic}");
        }

        private async Task CloseAsync()
        {
            if (mqttClient != null)
            {
                if (mqttClient.IsStarted)
                {
                    await mqttClient.StopAsync();
                }
                mqttClient.Dispose();
                
            }
        }

        private async Task ConnectionAsync()
        {
          
            var mqttClientOptions = new MqttClientOptionsBuilder()
                             .WithClientId(Client.ID)
                             .WithTcpServer(this.Client.Ipaddress, int.Parse(this.Client.Port));
                             //.WithCredentials(this.Client.ServerName, this.Client.ServerPwd);

            var options = new ManagedMqttClientOptionsBuilder()
                        .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                        .WithClientOptions(mqttClientOptions.Build())
                        .Build();
            //开启
          //  var t = mqttClientOptions;
            await mqttClient.StartAsync(options);
        }

        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="message"></param> 
        public void WriteLog(string message)
        {
            Task.Run(() =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    ConnectWords += message + "\r";
                });
            });
        }

        #region 属性


        private GeoLocation geoLocation;

        public GeoLocation Geo
        {
            get { return geoLocation; }
            set { geoLocation = value; RaisePropertyChanged(); }
        }


        private MqttClientModel client;

        /// <summary>
        /// 连接对象
        /// </summary>
        public MqttClientModel Client
        {
            get { return client; }
            set
            {
                client = value;
                RaisePropertyChanged(); 
            }
        }

        private string connectWords = "";
        /// <summary>
        /// 连接状态
        /// </summary>
        public string ConnectWords
        {
            get { return connectWords; }
            set
            {
                connectWords = value;
                RaisePropertyChanged();
            }
        }

        private string topic = "shanghai";
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic
        {
            get { return topic; }
            set
            {
                topic = value;
                RaisePropertyChanged();
            }
        }

        private string pubmsg = "0103";
        /// <summary>
        /// 发布
        /// </summary>
        public string Pubmsg
        {
            get { return pubmsg; }
            set
            {
                pubmsg = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region 命令

        private AsyncDelegateCommand connectioncommand;

        public AsyncDelegateCommand ConnectionCommand
        {
            get { return connectioncommand; }
            set { connectioncommand = value; RaisePropertyChanged(); }
        }

        private AsyncDelegateCommand closecommand;

        public AsyncDelegateCommand CloseCommand
        {
            get { return closecommand; }
            set { closecommand = value; RaisePropertyChanged(); }
        }


        private AsyncDelegateCommand subscritecommand;

        public AsyncDelegateCommand SubscriteCommand
        {
            get { return subscritecommand; }
            set { subscritecommand = value; RaisePropertyChanged(); }
        }

        private AsyncDelegateCommand publishcommand;

        public AsyncDelegateCommand PublishCommand
        {
            get { return publishcommand; }
            set { publishcommand = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 连接命令
        /// </summary> 

        /*  public ICommand OpenCommand
          {
              get
              {
                  return new RelayCommand(async o =>
                  {
                      var mqttClientOptions = new MqttClientOptionsBuilder()
                               .WithClientId(this.Client.ClientId)
                               .WithTcpServer(this.Client.ServerIP, int.Parse(this.Client.ServerPort))
                               .WithCredentials(this.Client.ServerName, this.Client.ServerPwd);

                      var options = new ManagedMqttClientOptionsBuilder()
                                  .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                                  .WithClientOptions(mqttClientOptions.Build())
                                  .Build();
                      //开启
                      var t = mqttClientOptions;
                      await mqttClient.StartAsync(options);
                  });
              }
          }*/

        /// <summary>
        /// 断开命令
        /// </summary> 
       /* public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    if (mqttClient != null)
                    {
                        if (mqttClient.IsStarted)
                        {
                            await mqttClient.StopAsync();
                        }
                        mqttClient.Dispose();   
                    }
                });
            }
        }*/

        /// <summary>
        /// 订阅命令
     /*   /// </summary> 
        [Obsolete]
        public ICommand SubscriteCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    if (string.IsNullOrWhiteSpace(this.Topic))
                    {
                        WriteLog(">>> 请输入主题");
                        return;
                    }

                    //在 MQTT 中有三种 QoS 级别： 
                    //At most once(0) 最多一次
                    //At least once(1) 至少一次
                    //Exactly once(2) 恰好一次
                    //await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(this.tbTopic.Text).WithAtMostOnceQoS().Build());//最多一次， QoS 级别0
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(this.Topic).WithAtLeastOnceQoS().Build());//恰好一次， QoS 级别1 

                    WriteLog($">>> 成功订阅 {this.Topic}");
                });
            }
        }*/

        /// <summary>
        /// 发布命令
        /// </summary> 
   /*     public ICommand PublishCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    if (string.IsNullOrWhiteSpace(this.Topic))
                    {
                        WriteLog(">>> 请输入主题");
                        return;
                    }
                    var result = await mqttClient.PublishAsync(
                        this.Topic,
                        this.Pubmsg,
                        MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);//恰好一次， QoS 级别1 
                    WriteLog($">>> 主题：{this.Topic}，消息：{this.Pubmsg}，结果： {result.ReasonCode}");
                });
            }
        }*/
 
        #endregion

    }
}
