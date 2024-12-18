using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Dto;
using Trace.Models;
using Trace.PageListHelper;
using Trace.Service.HttpClient;

namespace Trace.Service.IService
{
    public interface ITruckService : IBaseService<TruckDto>
    {
        Task<ApiResponse<PagedList<TruckDto>>> GetFliterAsync(FilterQuery query);
        Task<ApiResponse<SummaryDto>> GetSummaryAsync();
    }
}
