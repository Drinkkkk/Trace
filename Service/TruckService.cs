using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Models;

namespace Trace.Service
{
    public class TruckService : BaseService<TruckDto>,ITruckService
    {
        public TruckService(HttpRestClient client, string Servicename) : base(client, Servicename)
        {
        }
    }
}
