using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace.Common
{
    public interface IDialogHostAware
    {
        public string DialogHostName {  get; set; }
        public void OnDialogOpen(IDialogParameters dialogParameters);
        public  DelegateCommand CancelCommand {  get; set; }
        public  DelegateCommand SaveCommand { get; set; }
    }
}
