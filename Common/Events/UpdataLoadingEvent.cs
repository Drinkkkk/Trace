using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Trace.Common.Events
{

    public class UpdataModel
    {
        public bool IsOpen { get; set; }
    }
    public class UpdataLoadingEvent:PubSubEvent<UpdataModel>
    {

    }
}
