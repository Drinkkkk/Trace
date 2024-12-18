
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.PageListHelper;
using Trace.Service.HttpClient;
using QueryParameter = Trace.Service.HttpClient.QueryParameter;


namespace Trace.Service.IService
{
    public interface IBaseService<T> where T : class
    {
        Task<ApiResponse<T>> AddAsync(T entity);
        Task<ApiResponse<T>> UpdateAsync(T entity);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse<T>> GetFirstorDefaultAsync(int id);
        Task<ApiResponse<PagedList<T>>> GetPageListAsync (QueryParameter queryParameter );
    }
}
