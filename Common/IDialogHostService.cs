using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace.Common
{
    public interface IDialogHostService:IDialogService
    {
        Task<IDialogResult> ShowDialog(string name,IDialogParameters parameter,string dialogHostname="Root");
    }
}
