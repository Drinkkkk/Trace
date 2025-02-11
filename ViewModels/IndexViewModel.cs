using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using Trace.Common;
using Trace.Common.Models;
using Trace.Dto;
using Trace.Models;
using Trace.Service.IService;
using Trace.Views.DialogViews;

namespace Trace.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {

        public IndexViewModel(IDialogHostService dialog, ITruckService service, IContainerProvider provider, ITripService tripService) : base(provider)
        {
            create();
            ExcuteCommand = new DelegateCommand<string>(excute);
            EditTruckCommand = new DelegateCommand<TruckDto>(AddTruck);
            CompeleteCommand = new DelegateCommand<TruckDto>(complete);
            NavigateCommand = new DelegateCommand<TaskBar>(navigation);

            this.dialog = dialog;
            this.service = service;
            this.provider = provider;
            this.tripService = tripService;
            manager = this.provider.Resolve<IRegionManager>();
        }

        private void navigation(TaskBar bar)
        {
            if (bar.Target == null) return;
            var param = new NavigationParameters();
            if (bar.Title == "已完成")
            {
                param.Add("value", 2);
            }
            manager.Regions["MainRegion"].RequestNavigate(bar.Target, param);
        }

        private async void complete(TruckDto dto)
        {
            var result = await service.UpdateAsync(dto);
            if (result.Status)
            {
                var model = Summary.TruckList.FirstOrDefault(x => x.TruckID.Equals(dto.TruckID));
                if (model != null)
                {
                    Summary.TruckList.Remove(model);
                    Summary.CompletedCount++;
                    summary.CompletedRatio = (Summary.CompletedCount / (double)Summary.Sum).ToString("0%");
                    Refresh();
                }
            }
        }

        private void excute(string obj)
        {
            switch (obj)
            {
                case "增加货车": AddTruck(null); break;
                case "增加任务": AddTrip(null); break;
            }
        }

        private async void AddTrip(TripDto tripDto)
        {
            var param = new DialogParameters();
            if (tripDto != null)
            {
                var editdot = await tripService.GetFirstorDefaultAsync(tripDto.TripID);
                if (editdot.Status)
                {
                    if (editdot.Result != null)
                        param.Add("Value", editdot.Result);
                }
            }
            var result = await dialog.ShowDialog("AddTripView", param);
            if (result.Result == ButtonResult.OK)
            {
                var trip = result.Parameters.GetValue<TripDto>("Value");
                if (trip.TripID > 0)
                {
                    var result3 = await tripService.UpdateAsync(trip);
                    if (result3.Status)
                    {
                        var tripmodel = summary.TripList.FirstOrDefault(x => x.TripID.Equals(result3.Result.TripID));
                        if (tripmodel != null)
                        {

                        }
                    }
                }
                else
                {
                    var result2 = await tripService.AddAsync(trip);
                    if (result2.Status)
                    {
                        Summary.TripList.Add(result2.Result);
                      
                        Refresh();
                    }
                }
            }
        }

        private async void AddTruck(TruckDto truckDto)
        {
            var param = new DialogParameters();
            if (truckDto != null)
            {
                var editdot = await service.GetFirstorDefaultAsync(truckDto.TruckID);
                if (editdot.Status)
                {
                    if (editdot.Result != null)
                        param.Add("Value", editdot.Result);
                }
            }

            var result = await dialog.ShowDialog("AddTruckView", param);
            if (result.Result == ButtonResult.OK)
            {
                var truck = result.Parameters.GetValue<TruckDto>("Value");
                if (truck.TruckID > 0)
                {
                    var result3 = await service.UpdateAsync(truck);
                    if (result3.Status)
                    {
                        var truckModel = Summary.TruckList.FirstOrDefault(x => x.TruckID.Equals(result3.Result.TruckID));
                        if (truckModel != null)
                        {
                            truckModel.Title = result3.Result.Title;
                            truckModel.Content = result3.Result.Content;
                        }
                    }

                }
                else
                {
                    var result2 = await service.AddAsync(truck);
                    if (result2.Status)
                    {
                        Summary.TruckList.Add(result2.Result);
                        Summary.Sum++;
                        Summary.CompletedRatio = (Summary.CompletedCount / (double)Summary.Sum).ToString("0%");
                        Refresh();
                    }

                }
            }
        }
        void create()
        {
            Taskbars = new ObservableCollection<TaskBar>();
            Taskbars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = "TruckView" });
            Taskbars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = "TruckView" });
            Taskbars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Color = "#FF02C6DC", Target = "" });
            Taskbars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "任务", Color = "#FFFFA000", Target = "MissionView" });
        }

        #region 动态列表

        private ObservableCollection<TaskBar> taskbars;
        public ObservableCollection<TaskBar> Taskbars
        {
            get { return taskbars; }
            set { taskbars = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<TruckDto> trucks;

        public ObservableCollection<TruckDto> Trucks
        {
            get { return trucks; }
            set { trucks = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<TripDto> trips;

        public ObservableCollection<TripDto> Trips
        {
            get { return trips; }
            set { trips = value; RaisePropertyChanged(); }
        }
        #endregion
        #region 属性
        private readonly IDialogHostService dialog;
        private readonly ITruckService service;
        private readonly IContainerProvider provider;
        private readonly ITripService tripService;
        private readonly IRegionManager manager;
        private DelegateCommand<string> excutecommand;
        public DelegateCommand<string> ExcuteCommand
        {
            get { return excutecommand; }
            set { excutecommand = value; RaisePropertyChanged(); }
        }
        private DelegateCommand<TruckDto> edittruckcommand;

        public DelegateCommand<TruckDto> EditTruckCommand
        {
            get { return edittruckcommand; }
            set { edittruckcommand = value; RaisePropertyChanged(); }
        }

        private DelegateCommand<TruckDto> completecommand;

        public DelegateCommand<TruckDto> CompeleteCommand
        {
            get { return completecommand; }
            set { completecommand = value; RaisePropertyChanged(); }
        }

        private SummaryDto summary;

        public SummaryDto Summary
        {
            get { return summary; }
            set { summary = value; RaisePropertyChanged(); }
        }

        private DelegateCommand<TaskBar> navigateCommand;

        public DelegateCommand<TaskBar> NavigateCommand
        {
            get { return navigateCommand; }
            set { navigateCommand = value; RaisePropertyChanged(); }
        }

        #endregion

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var result = await service.GetSummaryAsync();
            if (result.Status)
            {
                Summary = result.Result;

                Refresh();
            }

            base.OnNavigatedTo(navigationContext);
        }

        void Refresh()
        {
            Taskbars[0].Content = Summary.Sum.ToString();
            Taskbars[1].Content = Summary.CompletedCount.ToString();
            Taskbars[2].Content = Summary.CompletedRatio.ToString();
            Taskbars[3].Content = Summary.TripCount.ToString();
        }
    }
}
