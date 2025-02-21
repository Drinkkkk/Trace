using NLog;
using Prism.Events;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Trace.Common;
using Trace.Common.Extensions;
using Trace.Dto;
using Trace.Models;
using Trace.Service.HttpClient;
using Trace.Service.IService;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace Trace.ViewModels
{
   
    public class UserViewModel : BindableBase
    {

        #region 属性
        public DelegateCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand { get; private set; }



        private Role role;

        public Role SelectedRole
        {
            get { return role; }
            set { role = value; RaisePropertyChanged(); }
        }


        private List<UserDto> user;
        public List<UserDto> User
        {
            get { return user; }
            set { user = value; RaisePropertyChanged(); }
        }

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


        private DelegateCommand returncommand;

        public DelegateCommand ReturnCommand
        {
            get { return returncommand; }
            set { returncommand = value; RaisePropertyChanged(); }
        }

        private DelegateCommand<FilterQuery> querycommand;

        public DelegateCommand<FilterQuery> QueryCommand
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

        private FilterQuery parameter;

        public FilterQuery Parameter
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
        private readonly IUserService Service;
        private readonly IEventAggregator eventAggregator;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public UserViewModel(IUserService userService,IEventAggregator eventAggregator)
        {
            CellEditEndingCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(HandleCellEditEnding);
            UserList = new ObservableCollection<UserDto>();
            QueryCommand = new DelegateCommand<FilterQuery>(Query);
            ReturnCommand = new DelegateCommand(ReturnKey);
            JumpCommand = new DelegateCommand<HandyControl.Data.FunctionEventArgs<int>>(Jump);
            Parameter=new FilterQuery() { pageIndex=0,pageSize=10,search="",Filter="" };
            this.Service = userService;
            this.eventAggregator = eventAggregator;
            Query(Parameter);
        }

        private async void HandleCellEditEnding(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedItem = e.Row.DataContext;
                var user = editedItem as UserDto;

                if (user != null)
                {
                    try
                    {
                       var result=await Service.UpdateAsync(user);
                        if (result.Status)
                        {
                            eventAggregator.SendMessage("修改用户信息成功！");
                            _logger.Info($"{AppSession.UserName}修改了用户信息");
                            Query(Parameter);
                        }
                        else
                        {
                            eventAggregator.SendMessage("保存用户信息失败！");
                           
                        }
                    }
                    catch (Exception)
                    {
                        eventAggregator.SendMessage("保存用户信息出错！");
                        _logger.Info($"{AppSession.UserName}保存用户信息出错");
                    }
                }
            }
        }

        private void ReturnKey()
        {
           
            Query(Parameter);
        }

        private void Jump(HandyControl.Data.FunctionEventArgs<int> obj)
        {

            parameter.pageIndex = obj.Info-1;
            
                Query(Parameter);
            
            // 根据新的页码更新数据
           
        }

   
        private async void Query(FilterQuery parameter)
        {
            Parameter.search =Search;
            Parameter.Filter = SelectedRole.ToString();
            var result = await Service.GetFliterAsync(parameter);
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
