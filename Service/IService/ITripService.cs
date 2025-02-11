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
    public interface ITripService:IBaseService<TripDto>
    {
        Task<ApiResponse<PagedList<TripDto>>> GetFliterAsync(FilterQuery query);
    }
}
