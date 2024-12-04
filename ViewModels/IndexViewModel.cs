using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common.Models;
using Trace.Models;

namespace Trace.ViewModels
{
    public class IndexViewModel:BindableBase
    {

        public IndexViewModel()
        {
            create();
            clist();
        }
      
		void create()
		{
            Taskbars = new ObservableCollection<TaskBar>();
            Taskbars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = "ToDoView" });
            Taskbars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = "ToDoView" });
            Taskbars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Color = "#FF02C6DC", Target = "" });
            Taskbars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "任务", Color = "#FFFFA000", Target = "MemoView" });
        }
        void clist()
        {
            Trucks = new ObservableCollection<TruckDto>();
            Missions = new ObservableCollection<MissionDto>();
            for (int i = 0; i <20; i++)
            {
                Trucks.Add(new TruckDto {Id=i });
                Missions.Add(new MissionDto {Id=i});
            }
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
            set { trucks = value;  RaisePropertyChanged(); }
        }
        private ObservableCollection<MissionDto> missions;

        public ObservableCollection<MissionDto> Missions
        {
            get { return missions; }
            set { missions = value; RaisePropertyChanged(); }
        }
        #endregion
    }
}
