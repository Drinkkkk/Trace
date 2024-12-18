using DryIoc;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common;

namespace Trace.ViewModels
{
    public class MsgViewModel:BindableBase,IDialogHostAware
    {
        public MsgViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; RaisePropertyChanged(); }
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
        }

        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                var result=new DialogResult(ButtonResult.OK);
                result.Parameters = param;
                DialogHost.Close(DialogHostName, result);
            }
        }

        public string DialogHostName { get; set; } = "Root";
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpen(IDialogParameters dialogParameters)
        {
            if (dialogParameters.ContainsKey("Title"))
                Title = dialogParameters.GetValue<string>("Title");

            if (dialogParameters.ContainsKey("Content"))
                Content = dialogParameters.GetValue<string>("Content");
        }
    }
}
