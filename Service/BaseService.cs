using RestSharp;
using Trace.Service;

namespace Trace.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private HttpRestClient client;
        private string servicename;
        public BaseService(HttpRestClient client, string Servicename) {
        this.client = client;
        this.servicename = Servicename;
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
            request.Method = Method.Post;
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

      /*  public async Task<ApiResponse<PagedList<T>>> GetAllAsync(QueryPara Parameter)
         {
             BaseRequest request = new BaseRequest();
             request.Method = Method.Post;
            request.Route = $"api/{servicename}/GetAll?Pageindex={Parameter.Pageindex}" +
                $"&PageSize={Parameter.PageSize}"
           // request.Route= "http://localhost:32169/api/ToDo/GetAll?Pageindex=0&PageSize=5"
                 +$"&search={Parameter.Search}";
             return await client.ExecuteAsync<PagedList<T>>(request);
         }*/

       
    }
}
