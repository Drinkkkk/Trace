using NLog;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows;
using Trace.Common;
using Trace.Common.Extensions;
using Trace.Dto;
using Trace.Service.HttpClient;
using Trace.Service.IService;

namespace Trace.ViewModels
{
    public class TruckViewModel : NavigationViewModel
    {

        #region 属性
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private ObservableCollection<TruckDto> trucks;

        public ObservableCollection<TruckDto> Trucks
        {
            get { return trucks; }
            set { trucks = value; RaisePropertyChanged(); }
        }

        private bool isRightOpen;

        public bool IsRightOpen
        {
            get { return isRightOpen; }
            set { isRightOpen = value; RaisePropertyChanged(); }
        }
        private readonly ITruckService Service;
        private readonly IEventAggregator eventAggregator;
        private DelegateCommand<TruckDto> selectecommand;

        public DelegateCommand<TruckDto> SelectedCommand
        {
            get { return selectecommand; }
            set { selectecommand = value; RaisePropertyChanged(); }
        }


        private DelegateCommand add;
        public DelegateCommand ADD
        {
            get { return add; }
            set { add = value; RaisePropertyChanged(); }
        }


        private TruckDto currentDto;

        public TruckDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }


        private string search;

        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        private DelegateCommand<string> excute;

        public DelegateCommand<string> Excute
        {
            get { return excute; }
            set { excute = value; RaisePropertyChanged(); }
        }

        private DelegateCommand<TruckDto> deletecommand;

        public DelegateCommand<TruckDto> DeleteCommand
        {
            get { return deletecommand; }
            set { deletecommand = value; RaisePropertyChanged(); }
        }


        private int indexselected;

        public int IndexSelected
        {
            get { return indexselected; }
            set { indexselected = value; RaisePropertyChanged(); }
        }

        private readonly DialogHostService dialoghostservice;
        #endregion
        //构造函数
        public TruckViewModel(ITruckService Service, IContainerProvider provider,IEventAggregator eventAggregator) : base(provider)
        {
            this.Service = Service;
            this.eventAggregator = eventAggregator;
            //query();
            ADD = new DelegateCommand(ad);
            SelectedCommand = new DelegateCommand<TruckDto>(selected);
            Excute = new DelegateCommand<string>(ExcuteCommand);
            DeleteCommand = new DelegateCommand<TruckDto>(delete);
            dialoghostservice = provider.Resolve<DialogHostService>();
        }

        private async void delete(TruckDto dto)
        {
            try
            {
                var dialogresult = await dialoghostservice.Question("温馨提示", $"确定删除{dto.Title}吗?");
                if (dialogresult.Result != ButtonResult.OK) return;
                UpdataLoading(true);
                var result = await Service.DeleteAsync(dto.TruckID);
                if (result.Status)
                {
                    Trucks.Remove(dto);

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
                _logger.Info($"{AppSession.UserName}删除了货车");
            }

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
        #region Excute
        private void ad()
        {
            CurrentDto = new TruckDto();
            IsRightOpen = true;
        }
        public async void query()
        {
            UpdataLoading(true);

            string? index;
            if (IndexSelected == 0)
            {
                index = null;
            }
            else
            {
                index = IndexSelected == 1 ? "false" : "true";
            }

            Trucks = new ObservableCollection<TruckDto>();
            var result = await Service.GetFliterAsync(new FilterQuery { pageIndex = 0, pageSize = 100, search = Search, Filter = index });
            if (result.Status == true)
            {

                foreach (var item in result.Result.Items)
                {
                    Trucks.Add(item);
                }
            }
            UpdataLoading(false);
            eventAggregator.SendMessage("查询成功！");
        }
        private async void save()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentDto.Title) || string.IsNullOrWhiteSpace(CurrentDto.Content))
                    return;
                UpdataLoading(true);
                if (currentDto.TruckID > 0)
                {
                    var result = await Service.UpdateAsync(CurrentDto);
                    if (result.Status)
                    {
                        var truckdto = Trucks.FirstOrDefault(x => x.TruckID.Equals(CurrentDto.TruckID));
                        if (truckdto != null)
                        {
                            truckdto.Status = currentDto.Status;
                            truckdto.Title = CurrentDto.Title;
                            truckdto.Content = CurrentDto.Content;
                            truckdto.LicensePlate = CurrentDto.LicensePlate;
                            truckdto.LoadCapacity = CurrentDto.LoadCapacity;
                            truckdto.VehicleModel = CurrentDto.VehicleModel;
                        }


                    }
                }
                else
                {
                    var result = await Service.AddAsync(CurrentDto);
                    if (result.Status)
                    {
                        Trucks.Add(result.Result);

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
                _logger.Info($"{AppSession.UserName}保存了货车");
            }


        }
        #endregion
        //拉单个数据
        private async void selected(TruckDto dto)
        {
            UpdataLoading(true);
            var result = await Service.GetFirstorDefaultAsync(dto.TruckID);
            if (result.Status)
            {
                CurrentDto = result.Result;

                IsRightOpen = true;

            }
            UpdataLoading(false);
        }

        //添加货车


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("value"))
            {
                IndexSelected = navigationContext.Parameters.GetValue<int>("value");
            }
            else
                indexselected = 0;
            query();
            base.OnNavigatedTo(navigationContext);
        }


    }

}

