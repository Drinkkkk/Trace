using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Dto;
using Trace.Service.HttpClient;
using Trace.Service.IService;

namespace Trace.Service
{
    public class UserService : BaseService<UserDto>, IUserService
    {
        public UserService(HttpRestClient client) : base(client,"User")
        {
        }
    }
}
