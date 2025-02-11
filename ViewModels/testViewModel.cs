using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Dto;

namespace Trace.ViewModels
{
   
    class testViewModel
    {
        public ObservableCollection<UserDto> UserList { get; set; }
        public testViewModel()
        {
           UserList = new ObservableCollection<UserDto>();
            UserList.Add(new UserDto() { UserID=1});
        }

    }
}
