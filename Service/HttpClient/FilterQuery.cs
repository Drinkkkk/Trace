using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace.Service.HttpClient
{
   public class FilterQuery:QueryParameter
    {
        public string? Filter { get; set; }
    }
}
