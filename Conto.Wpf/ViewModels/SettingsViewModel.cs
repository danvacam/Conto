using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Conto.Data;

namespace Conto.Wpf.ViewModels
{
    public class SettingsViewModel: INotifyPropertyChanged
    {

        
        readonly ContoData _contoData;

        public SettingsViewModel()
        {
            _contoData = new ContoData();
            var settings = _contoData.GetSettings();

            if (settings != null)
            {
                InvoiceOwnerAddress = settings.InvoiceOwnerAddress;
                InvoiceOwnerCity = settings.InvoiceOwnerCity;
                InvoiceOwnerFiscalCode = settings.InvoiceOwnerFiscalCode;
                InvoiceOwnerName = settings.InvoiceOwnerName;
                InvoiceOwnerPostalCode = settings.InvoiceOwnerPostalCode;
                InvoiceOwnerVatCode = settings.InvoiceOwnerVatCode;
                InvoiceMaxCost = settings.MaxInvoiceValue;
            }
        }

        #region Settings Properties

        private string _invoiceOwnerName;
        public string InvoiceOwnerName
        {
            get { return _invoiceOwnerName; }
            set
            {
                _invoiceOwnerName = value;
                OnPropertyChanged();
            }
        }

        private string _invoiceOwnerAddress;
        public string InvoiceOwnerAddress
        {
            get { return _invoiceOwnerAddress; }
            set
            {
                _invoiceOwnerAddress = value;
                OnPropertyChanged();
            }
        }

        private string _invoiceOwnerCity;
        public string InvoiceOwnerCity
        {
            get { return _invoiceOwnerCity; }
            set
            {
                _invoiceOwnerCity = value;
                OnPropertyChanged();
            }
        }

        private string _invoiceOwnerPostalCode;
        public string InvoiceOwnerPostalCode
        {
            get { return _invoiceOwnerPostalCode; }
            set
            {
                _invoiceOwnerPostalCode = value;
                OnPropertyChanged();
            }
        }

        private string _invoiceOwnerFiscalCode;
        public string InvoiceOwnerFiscalCode
        {
            get { return _invoiceOwnerFiscalCode; }
            set
            {
                _invoiceOwnerFiscalCode = value;
                OnPropertyChanged();
            }
        }

        private string _invoiceOwnerVatCode;
        public string InvoiceOwnerVatCode
        {
            get { return _invoiceOwnerVatCode; }
            set
            {
                _invoiceOwnerVatCode = value;
                OnPropertyChanged();
            }
        }

        private decimal? _invoiceMaxCost;
        public decimal? InvoiceMaxCost
        {
            get
            {
                return _invoiceMaxCost;
            }
            set
            {
                _invoiceMaxCost = value;
                OnPropertyChanged();
            }
        }

        #endregion
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
