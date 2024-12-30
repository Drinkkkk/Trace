using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trace.Dto;
using Trace.Service.HttpClient;
using Trace.Service.IService;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace Trace.ViewModels
{
    public class UserViewModel : BindableBase
    {

        #region 属性
        private ICollectionView collectionView;

        public ICollectionView CollectionView
        {
            get { return collectionView; }
            set { collectionView = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<UserDto> userlist;

        public ObservableCollection<UserDto> UserList
        {
            get { return userlist; }
            set { userlist = value; RaisePropertyChanged(); }
        }

        private DelegateCommand filter;

        public DelegateCommand Filter
        {
            get { return filter; }
            set { filter = value; RaisePropertyChanged(); }
        }

        private DelegateCommand<QueryParameter> querycommand;

        public DelegateCommand<QueryParameter> QueryCommand
        {
            get { return querycommand; }
            set { querycommand = value; RaisePropertyChanged(); }
        }


        private DelegateCommand<HandyControl.Data.FunctionEventArgs<int>> jumpcommand;

        public DelegateCommand<HandyControl.Data.FunctionEventArgs<int>> JumpCommand
        {
            get { return jumpcommand; }
            set { jumpcommand = value; RaisePropertyChanged(); }
        }
        private int currentpage;

        public int CurrentPage
        {
            get { return currentpage; }
            set { currentpage = value; RaisePropertyChanged(); }
        }


        private string search;

        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        private QueryParameter parameter;

        public QueryParameter Parameter
        {
            get { return parameter; }
            set { parameter = value; RaisePropertyChanged(); }
        }

        #endregion
        #region page属性
        private int pageIndex;

        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; RaisePropertyChanged(); }
        }

        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value;RaisePropertyChanged(); }
        }
        private int totalCount;

        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; RaisePropertyChanged(); }
        }
        private int totalPages;

        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages = value;RaisePropertyChanged(); }
        }
        private int indexFrom;

        public int IndexFrom
        {
            get { return indexFrom; }
            set { indexFrom = value; RaisePropertyChanged(); }
        }

        private bool hasPreviousPage;

        public bool HasPreviousPage
        {
            get { return hasPreviousPage; }
            set { hasPreviousPage = value; RaisePropertyChanged(); }
        }
        private bool hasNextPage;

        public bool HasNextPage
        {
            get { return hasNextPage; }
            set { hasNextPage = value; }
        }


        #endregion
        private List<UserDto> user;
        private readonly IUserService Service;

        public List<UserDto> User
        {
            get { return user; }
            set { user = value; RaisePropertyChanged(); }
        }

        public UserViewModel(IUserService userService)
        {
            UserList = new ObservableCollection<UserDto>();
            QueryCommand = new DelegateCommand<QueryParameter>(Query);
            JumpCommand = new DelegateCommand<HandyControl.Data.FunctionEventArgs<int>>(Jump);
            Parameter=new QueryParameter() { pageIndex=0,pageSize=10,search="" };
            this.Service = userService;
            Query(Parameter);
        }

        private void Jump(HandyControl.Data.FunctionEventArgs<int> obj)
        {

            parameter.pageIndex = obj.Info-1;
            
                Query(Parameter);
            
            // 根据新的页码更新数据
           
        }

   
        private async void Query(QueryParameter parameter)
        {
            var result = await Service.GetPageListAsync(parameter);
            if (result.Status)
            {

                UserList.Clear();

               
                PageSize = result.Result.PageSize;
                TotalCount = result.Result.TotalCount;
                TotalPages = result.Result.TotalPages;
                IndexFrom = result.Result.IndexFrom;
                HasPreviousPage = result.Result.HasPreviousPage;
                HasNextPage = result.Result.HasNextPage;
                foreach (var item in result.Result.Items)
                {
                    UserList.Add(item);
                }
            }
        }


       

   
    }
}
