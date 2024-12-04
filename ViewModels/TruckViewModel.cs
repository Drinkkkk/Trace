using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Models;

namespace Trace.ViewModels
{
    public class TruckViewModel:BindableBase
    {

        #region 属性
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
        

        #endregion
        public TruckViewModel()
        {
            create();

            ADD = new DelegateCommand(ad);
        }

        private void ad()
        {
            IsRightOpen = true;
        }

        void create()
        {
            Trucks = new ObservableCollection<TruckDto>();
            for (int i = 0; i < 20; i++)
            {
                Trucks.Add(new TruckDto() { Title = Convert.ToString(i), Content = Convert.ToString(i * i) });
            }
        }
        private DelegateCommand add;

        public DelegateCommand ADD
        {
            get { return add; }
            set { add = value;  RaisePropertyChanged(); }
        }

    }

}

