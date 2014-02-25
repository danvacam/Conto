using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Conto.Data;

namespace Conto.Wpf.Customer
{
    public delegate void ItemEndEditEventHandler(IEditableObject sender);

    public class CustomerUIObject : IEditableObject, INotifyPropertyChanged
    {
        private CustomerDataObject _customer;

        public CustomerUIObject()
        {
            _customer = new CustomerDataObject();
        }

        public CustomerUIObject(CustomerDataObject customer)
        {
            _customer = customer;
        }

        public CustomerDataObject GetDataObject()
        {
            return _customer;
        }

        public int Id
        {
            get { return _customer.Id; }
        }

        public string Name
        {
            get
            {
                return _customer.Name;
            }
            set
            {
                _customer.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Address
        {
            get
            {
                return _customer.Address;
            }
            set
            {
                _customer.Address = value;
                OnPropertyChanged("Address");
            }
        }

        public string FiscalCode
        {
            get
            {
                return _customer.Fiscal_Code;
            }
            set
            {
                _customer.Fiscal_Code = value;
                OnPropertyChanged("FiscalCode");
            }
        }

        public string VatCode
        {
            get
            {
                return _customer.Vat_Code;
            }
            set
            {
                _customer.Vat_Code = value;
                OnPropertyChanged("VatCode");
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
