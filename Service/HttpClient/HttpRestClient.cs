using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Trace.Service.HttpClient
{
    public class HttpRestClient
    {
        private string url;
        protected readonly RestClient Client;
        public HttpRestClient(string Url)
        {
            url = Url;
            Client = new RestClient();
        }


        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {

            var request = new RestRequest(new Uri(url + baseRequest.Route), baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Method == Method.Post)
            {
                request.AddHeader("Accept", "text/plain");
            }
            if (baseRequest.Parameter != null)
            {
               
                    request.AddParameter("application/json", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            }
               
            
            var response = await Client.ExecuteAsync(request);


            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            else
                return new ApiResponse<T>()
                {
                    Status = false,
                    Result = default,
                    Message = response.ErrorMessage
                };
        }


        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(new Uri(url + baseRequest.Route), baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            var response = await Client.ExecuteAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            else
                return new ApiResponse()
                {
                    Status = false,
                    Result = null,
                    Message = response.ErrorMessage
                };
        }

    }
}
