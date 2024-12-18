using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Dto;

namespace Trace.ViewModels
{
    public class UserViewModel:BindableBase
    {
        private List<UserDto> user;

     

        public List<UserDto> User
        {
            get { return user; }
            set { user = value; RaisePropertyChanged(); }
        }

        public UserViewModel()
        {
            create();
        }
        void create()
        {
            user =new List<UserDto>();
            for (int i = 0; i < 10; i++) { user.Add(new UserDto() { FullName = i.ToString(), UserID = i, Password = (i * i).ToString() }); }
          
        }
    }
}
