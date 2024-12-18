using RestSharp;
using Trace.PageListHelper;
using Trace.Service.HttpClient;
using Trace.Service.IService;

namespace Trace.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private HttpRestClient client;
        private string servicename;
        public BaseService(HttpRestClient client, string Servicename)
        {
            this.client = client;
            servicename = Servicename;
        }

        public async Task<ApiResponse<T>> AddAsync(T entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Post;
            request.Route = $"api/{servicename}/Add";
            request.Parameter = entity;
            return await client.ExecuteAsync<T>(request);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Delete;
            request.Route = $"api/{servicename}/Delete?id={id}";

            return await client.ExecuteAsync(request);
        }



        public async Task<ApiResponse<T>> GetFirstorDefaultAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Get;
            request.Route = $"api/{servicename}/Get?id={id}";

            return await client.ExecuteAsync<T>(request);
        }

        public async Task<ApiResponse<T>> UpdateAsync(T entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Post;
            request.Route = $"api/{servicename}/Update";
            request.Parameter = entity;

            return await client.ExecuteAsync<T>(request);
        }

       
        public async Task<ApiResponse<PagedList<T>>> GetPageListAsync(HttpClient.QueryParameter queryParameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Post;
            /* request.Route = $"api/{servicename}/GetAll?pageIndex={queryParameter.PageIndex}" +
                 $"&pageSize={queryParameter.PageSize}" + $"&search={queryParameter.Search}";*/


            request.Route = $"api/{servicename}/GetAll";

            request.Parameter = queryParameter;
            
            return await client.ExecuteAsync<PagedList<T>>(request);
        }
    }
}
