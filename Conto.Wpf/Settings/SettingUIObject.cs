using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Conto.Data;
using Conto.Wpf.Customer;

namespace Conto.Wpf.Settings
{
    public class SettingUIObject : IEditableObject, INotifyPropertyChanged
    {
        private readonly CommonDataObject _setting;

        public SettingUIObject()
        {
            _setting = new CommonDataObject();
        }

        public SettingUIObject(CommonDataObject setting)
        {
            _setting = setting;
        }

        public CommonDataObject GetDataObject()
        {
            return _setting;
        }

        public int Year
        {
            get { return _setting.Work_Year; }
            set
            {
                _setting.Work_Year = value;
                OnPropertyChanged("Year");
            }
        }

        public long InvoiceNumber
        {
            get { return _setting.Invoice_Number; }
            set
            {
                _setting.Invoice_Number = value;
                OnPropertyChanged("InvoiceNumber");
            }
        }

        public decimal MaxSelfInvoiceCost
        {
            get { return _setting.Max_Self_Invoice_cost; }
            set
            {
                _setting.Max_Self_Invoice_cost = value;
                OnPropertyChanged("MaxSelfInvoiceCost");
            }
        }

        public int SelfInvoiceCustomerId
        {
            get { return _setting.Self_Invoice_Customer; }
            set
            {
                _setting.Self_Invoice_Customer = value;
                OnPropertyChanged("SelfInvoiceCustomerId");
            }
        }

        public event ItemEndEditEventHandler ItemEndEdit;

        public void BeginEdit()
        {
        }

        public void EndEdit()
        {
            if (ItemEndEdit != null)
            {
                ItemEndEdit(this);
            }
        }

        public void CancelEdit()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
