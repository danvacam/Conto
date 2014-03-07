using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Conto.Data;

namespace Conto.Wpf.ViewModels
{
    public class SelfInvoiceViewModel : INotifyPropertyChanged
    {
        ContoData _contoData;

        public SelfInvoiceViewModel()
        {
            _contoData = new ContoData();

            //AddWithdrawCommand = new RelayCommand(AddWithdrawCommand_Executed);
            //AddCostCommand = new RelayCommand(AddCostCommand_Executed);

            VatExempt = true;
        }


        private int? _quantity;
        public int? Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private DateTime _invoiceDate;
        public DateTime InvoiceDate
        {
            get
            {
                return _invoiceDate;
            }
            set
            {
                _invoiceDate = value;
                OnPropertyChanged("InvoiceDate");
            }
        }


        private CollectionView _materials;
        public CollectionView Materials
        {
            get
            {
                return _materials;
            }
            set
            {
                _materials = value;
                OnPropertyChanged("Materials");
            }
        }

        private bool _vatExempt;
        public bool VatExempt
        {
            get
            {
                return _vatExempt;
            }
            set
            {
                _vatExempt = value;
                OnPropertyChanged("VatExempt");
            }
        }


        public ICommand AddSelfInvoice { get; set; }

        public void AddSelfInvoice_Executed(object sender)
        {
            MessageBox.Show("AddSelfInvoice");
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
        }

    }
}
