﻿using System.Collections.ObjectModel;
using Trace.Common;
using Trace.Common.Models;

namespace Trace.ViewModels
{
    public class MainViewModel : NavigationViewModel, IConfigureService
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        public DelegateCommand LoginOutCommand { get; private set; }

        public MainViewModel(IContainerProvider containerProvider,
            IRegionManager regionManager):base(containerProvider)
        { 
            MenuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                    journal.GoBack();
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                    journal.GoForward();
            });
            LoginOutCommand = new DelegateCommand(() =>
              {
                  //注销当前用户
                  App.LoginOut(containerProvider);
              });
            this.containerProvider = containerProvider;
            this.regionManager = regionManager;
          
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            NavigationParameters keyValuePairs = new NavigationParameters();
            regionManager.Regions["MainRegion"].RequestNavigate(obj.NameSpace, back =>
             {
                 journal = back.Context.NavigationService.Journal;
             });
        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }

       
        private ObservableCollection<MenuBar> menuBars;
        private readonly IContainerProvider containerProvider;
        private readonly IRegionManager regionManager;
        private  IRegionNavigationJournal journal;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }


        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "货车", NameSpace = "TruckView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "任务", NameSpace = "MissionView" });
            MenuBars.Add(new MenuBar() { Icon = "Account", Title = "用户", NameSpace = "UserView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "设置", NameSpace = "SettingView" });
        }

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            CreateMenuBar();
            regionManager.Regions["MainRegion"].RequestNavigate("IndexView");
        }
    }
}
