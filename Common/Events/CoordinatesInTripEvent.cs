using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Dto;

namespace Trace.Common.Events
{
    public class CoordinatesInTrip
    {
        private List<CoordinateDto> coordinates;

        public List<CoordinateDto> Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }
        public string Message { get; set; } = "Coordinates";
    }
    public class CoordinatesInTripEvent:PubSubEvent<CoordinatesInTrip>
    {
    }
}
