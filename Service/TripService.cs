using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Dto;
using Trace.PageListHelper;
using Trace.Service.HttpClient;
using Trace.Service.IService;

namespace Trace.Service
{
    public class TripService : BaseService<TripDto>, ITripService
    {
        private readonly HttpRestClient client;

        public TripService(HttpRestClient client) : base(client, "Trip")
        {
            this.client = client;
        }

        public async Task<ApiResponse<PagedList<TripDto>>> GetFliterAsync(FilterQuery query)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Post;
            /* request.Route = $"api/{servicename}/GetAll?pageIndex={queryParameter.PageIndex}" +
                 $"&pageSize={queryParameter.PageSize}" + $"&search={queryParameter.Search}";*/


            request.Route = $"api/Trip/GetFilter";

            request.Parameter = query;

            return await client.ExecuteAsync<PagedList<TripDto>>(request);
        }
    }
}
