using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Trace.Dto
{
    public class MqttClientModel:BaseDto
    {
		private string ipaddress;

		public string Ipaddress
        {
			get { return ipaddress; }
			set { ipaddress = value; OnPropertyChanged(); }
		}
		private string port;

		public string Port
		{
			get { return port; }
			set { port = value;OnPropertyChanged(); }
		}
		private string id;

		public string ID
		{
			get { return id; }
			set { id = value; OnPropertyChanged(); }
		}
        public MqttClientModel(string ip,string port,string id)
        {
            Ipaddress = ip;
			Port = port;
			ID = id;
        }
    }
}
