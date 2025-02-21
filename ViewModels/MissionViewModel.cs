using GMap.NET.WindowsPresentation;
using GMap.NET;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common.Extensions;
using Trace.Dto;
using Trace.Gmap;
using Trace.Common.Events;
using System.Windows.Media;
using System.Windows.Ink;
using HandyControl.Controls;
using Trace.Service.IService;
using MaterialDesignColors;
using System.Reflection;
using Trace.Service.HttpClient;
using Prism.Commands;
using Trace.Common;
using NLog;

namespace Trace.ViewModels
{
    public class MissionViewModel : NavigationViewModel
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #region 属性
        private ObservableCollection<TripDto> trips;

        public ObservableCollection<TripDto> Trips
        {
            get { return trips; }
            set { trips = value; RaisePropertyChanged(); }
        }

        private MapControl mainmap;

        public MapControl MainMap
        {
            get { return mainmap; }
            set { mainmap = value; RaisePropertyChanged(); }
        }
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        private int indexselected;

        public int IndexSelected
        {
            get { return indexselected; }
            set { indexselected = value; RaisePropertyChanged(); }
        }

        private DelegateCommand<string> excute;

        public DelegateCommand<string> Excute
        {
            get { return excute; }
            set { excute = value; RaisePropertyChanged(); }
        }

        private TripDto currentdto;

        public TripDto CurrentDto
        {
            get { return currentdto; }
            set { currentdto = value; RaisePropertyChanged(); }
        }
        private bool isRightOpen;

        public bool IsRightOpen
        {
            get { return isRightOpen; }
            set { isRightOpen = value; RaisePropertyChanged(); }
        }

        private DelegateCommand<TripDto> selectecommand;

        public DelegateCommand<TripDto> SelectedCommand
        {
            get { return selectecommand; }
            set { selectecommand = value; RaisePropertyChanged(); }
        }
        private DelegateCommand<TripDto> deletecommand;

        public DelegateCommand<TripDto> DeleteCommand
        {
            get { return deletecommand; }
            set { deletecommand = value; RaisePropertyChanged(); }
        }

        private DialogHostService dialoghostservice;

        private DelegateCommand<TripDto> showmarkercommand;

        public DelegateCommand<TripDto> ShowMarkerCommand
        {
            get { return showmarkercommand; }
            set { showmarkercommand = value; RaisePropertyChanged(); }
        }

        #endregion
       
        private readonly ITripService Service;
        private readonly ICoordinateService coordinateService;
        private readonly IEventAggregator eventAggregator;

        public MissionViewModel(IContainerProvider provider, ITripService Service, ICoordinateService coordinateService,IEventAggregator eventAggregator) : base(provider)
        {
            
            this.Service = Service;
            this.coordinateService = coordinateService;
            this.eventAggregator = eventAggregator;
            trips = new ObservableCollection<TripDto>();
            Excute = new DelegateCommand<string>(ExcuteCommand);
            SelectedCommand = new DelegateCommand<TripDto>(selected);
            DeleteCommand = new DelegateCommand<TripDto>(delete);
            dialoghostservice = provider.Resolve<DialogHostService>();
            ShowMarkerCommand = new DelegateCommand<TripDto>(ShowMarker);
            Aggregator.ReceiveReport(Receive);
            query();
        }

        public async void Receive(GeoLocation location)
        {
            
                var result = await Service.GetFirstorDefaultAsync(location.TripID);
                if (result.Status && result.Result.TruckID == location.TruckID)
                {
                    var Dto = new CoordinateDto() { Latitude = (decimal?)location.Latitude, Longitude = (decimal?)location.Longitude, TripID = location.TripID, Timestamp = DateTime.Now };
                    var result2 = await this.coordinateService.AddAsync(Dto);
                    if (result2.Status)
                    {
                    
                    
                    if (CurrentDto != null && CurrentDto.TripID == location.TripID)
                        {
                            Aggregator.SendCoordinates(new CoordinatesInTrip() { Coordinates = new List<CoordinateDto>() { Dto }, Message = "AddNew" });
                        }
                    }
                }
            
        }

        private void ShowMarker(TripDto dto)
        {
            CurrentDto = dto;
            if (CurrentDto.Coordinates != null)
            {
                Aggregator.SendCoordinates(new CoordinatesInTrip() { Coordinates = CurrentDto.Coordinates });
            }

        }

        private async void delete(TripDto dto)
        {
            try
            {
                var dialogresult = await dialoghostservice.Question("温馨提示", $"确定删除{dto.Title}吗?");
                if (dialogresult.Result != ButtonResult.OK) return;
                UpdataLoading(true);
                var result = await Service.DeleteAsync(dto.TripID);
                if (result.Status)
                {
                    trips.Remove(dto);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                UpdataLoading(false);
                eventAggregator.SendMessage("删除成功！");
            }
            _logger.Info($"{AppSession.UserName}删除了日志");
        }
        private async void selected(TripDto dto)
        {
            UpdataLoading(true);
            var result = await Service.GetFirstorDefaultAsync(dto.TripID);
            if (result.Status)
            {
                CurrentDto = result.Result;

                IsRightOpen = true;

            }
            UpdataLoading(false);
        }
        private void ExcuteCommand(string obj)
        {
            switch (obj)
            {
                case "新增": ad(); break;
                case "查询": query(); break;
                case "保存": save(); break;
            }
        }
        #region excute
        private void ad()
        {
            CurrentDto = new TripDto();
            IsRightOpen = true;
        }
        public async void query()
        {
            trips.Clear();
            string? index;
            if (IndexSelected == 0)
            {
                index = null;
            }
            else
            {
                index = IndexSelected == 1 ? "false" : "true";
            }

            var result = await Service.GetFliterAsync(new FilterQuery { pageIndex = 0, pageSize = 100, search = Search, Filter = index });
            if (result.Status)
            {
                foreach (var item in result.Result.Items)
                {
                    trips.Add(item);
                }
                eventAggregator.SendMessage("查询成功！");
            }
        }
        private async void save()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentDto.Title) || string.IsNullOrWhiteSpace(CurrentDto.Content))
                    return;
                UpdataLoading(true);
                if (CurrentDto.TripID > 0)
                {
                    var result = await Service.UpdateAsync(CurrentDto);
                    if (result.Status)
                    {
                        var tripdto = trips.FirstOrDefault(x => x.TripID.Equals(CurrentDto.TripID));
                        if (tripdto != null)
                        {

                            tripdto.Title = CurrentDto.Title;
                            tripdto.Content = CurrentDto.Content;

                        }


                    }
                }
                else
                {
                    var result = await Service.AddAsync(CurrentDto);
                    if (result.Status)
                    {
                        trips.Add(result.Result);

                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                isRightOpen = false;
                UpdataLoading(false);
                eventAggregator.SendMessage("保存成功！");
                _logger.Info($"{AppSession.UserName}保存了任务");
            }


        }
        #endregion
        #region 收到MQTT发来的消息

        #endregion





    }
}
