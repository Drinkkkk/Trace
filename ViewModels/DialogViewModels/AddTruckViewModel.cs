using DryIoc;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common;
using Trace.Dto;

namespace Trace.ViewModels.DialogViewModels
{
    public class AddTruckViewModel :BindableBase, IDialogHostAware
    {
        public AddTruckViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            SaveCommand = new DelegateCommand(Save);
        }

        #region 属性
        private TruckDto model;

        public TruckDto Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }
        public string DialogHostName { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        #endregion
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Model.Title) ||
               string.IsNullOrWhiteSpace(model.Content)) return;

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                //确定时,把编辑的实体返回并且返回OK
                DialogParameters param = new DialogParameters();
                param.Add("Value", Model);
                var result = new DialogResult(ButtonResult.OK);
                result.Parameters=param;
                DialogHost.Close(DialogHostName, result);
            }
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No)); //取消返回NO告诉操作结束
        }

      
        public void OnDialogOpen(IDialogParameters dialogParameters)
        {
            if (dialogParameters.ContainsKey("Value"))
            {
                Model = dialogParameters.GetValue<TruckDto>("Value");
            }
            else
                Model = new TruckDto();
        }
    }
}
