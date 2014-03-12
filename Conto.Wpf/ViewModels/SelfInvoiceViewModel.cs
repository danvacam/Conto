using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Conto.Data;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class SelfInvoiceViewModel : INotifyPropertyChanged
    {
        ContoData _contoData;

        public SelfInvoiceViewModel()
        {
            _contoData = new ContoData();

            AddSelfInvoice = new RelayCommand(AddSelfInvoice_Executed);

            InvoiceStartDate = DateTime.Now;
            InvoiceEndDate = DateTime.Now;
            InvoiceYear = DateTime.Now.Year;

            VatExempt = true;
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

        private decimal? _materialPrice;
        public decimal? MaterialPrice
        {
            get
            {
                return _materialPrice;
            }
            set
            {
                _materialPrice = value;
                OnPropertyChanged("MaterialPrice");
            }
        }

        private CollectionView _measures;
        public CollectionView Measures
        {
            get
            {
                return _measures;
            }
            set
            {
                _measures = value;
                OnPropertyChanged("Measures");
            }
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

        private DateTime _invoiceStartDate;
        public DateTime InvoiceStartDate
        {
            get
            {
                return _invoiceStartDate;
            }
            set
            {
                _invoiceStartDate = value;
                OnPropertyChanged("InvoiceStartDate");
            }
        }

        private DateTime _invoiceEndDate;
        public DateTime InvoiceEndDate
        {
            get
            {
                return _invoiceEndDate;
            }
            set
            {
                _invoiceEndDate = value;
                OnPropertyChanged("InvoiceEndDate");
            }
        }

        private int _invoiceYear;
        public int InvoiceYear
        {
            get { return _invoiceYear; }
            set
            {
                _invoiceYear = value;
                OnPropertyChanged("InvoiceYear");
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
            if (AppProperties.FormHaveModifications)
            {
                MessageBox.Show("AddSelfInvoice MODIFIED");
            }
            else
            {
                MessageBox.Show("AddSelfInvoice");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                AppProperties.FormHaveModifications = true;
            }
        }
    }
}
