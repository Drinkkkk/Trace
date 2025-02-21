using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common;

namespace Trace.ViewModels.DialogViewModels
{
    public class testViewModel : BindableBase, IDialogHostAware
    {
        public string DialogHostName { get; set; }
        public DelegateCommand CancelCommand { get; set ; }
        public DelegateCommand SaveCommand { get ; set ; }

        public void OnDialogOpen(IDialogParameters dialogParameters)
        {
            
        }
    }
}
