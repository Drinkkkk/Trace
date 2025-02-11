using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace.Common.Events
{

    public class GeoLocation
    {
        public int TruckID;
        public int TripID;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Message { get; set; } = "MQTT";
    }
    public class MQTTEvent:PubSubEvent<GeoLocation>
    {
    }
}
