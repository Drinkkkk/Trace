using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Dto;
using Trace.Models;
using Trace.PageListHelper;
using Trace.Service.HttpClient;
using Trace.Service.IService;

namespace Trace.Service
{
    public class TruckService : BaseService<TruckDto>, ITruckService
    {
        private readonly HttpRestClient client;

        public TruckService(HttpRestClient client) : base(client, "Truck")
        {
            this.client = client;
        }

        public async Task<ApiResponse<PagedList<TruckDto>>> GetFliterAsync(FilterQuery query)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Post;
            /* request.Route = $"api/{servicename}/GetAll?pageIndex={queryParameter.PageIndex}" +
                 $"&pageSize={queryParameter.PageSize}" + $"&search={queryParameter.Search}";*/


            request.Route = $"api/Truck/GetFilter";

            request.Parameter = query;

            return await client.ExecuteAsync<PagedList<TruckDto>>(request);
        }

        public async Task<ApiResponse<SummaryDto>> GetSummaryAsync()
        {
           BaseRequest request= new BaseRequest();
            request.Method = Method.Post;
            request.Route = $"api/Truck/Summary";
            return  await client.ExecuteAsync<SummaryDto>(request);
        }
    }
}
