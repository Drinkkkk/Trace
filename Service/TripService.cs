using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Dto;
using Trace.Service.HttpClient;
using Trace.Service.IService;

namespace Trace.Service
{
    public class TripService : BaseService<TripDto>, ITripService
    {
        public TripService(HttpRestClient client) : base(client, "Trip")
        {
        }
    }
}
