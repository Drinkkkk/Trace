using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Dto;
using Trace.PageListHelper;
using Trace.Service.HttpClient;

namespace Trace.Service.IService
{
    public interface IUserService:IBaseService<UserDto>
    {
        Task<ApiResponse<PagedList<UserDto>>> GetFliterAsync(FilterQuery query);
        Task<ApiResponse<UserDto>> LoginAsync(UserDto userDto);
        Task<ApiResponse<UserDto>> RegisterAsync(UserDto userDto);
    }
}
