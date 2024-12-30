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
    public class UserService : BaseService<UserDto>, IUserService
    {
        private readonly HttpRestClient client;

        public UserService(HttpRestClient client) : base(client,"User")
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

    }
}
