using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Conto.Data;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class InvoiceViewModel : INotifyPropertyChanged
    {
        private const int NumberOfRowsInInvoicesGrid = 10;
        readonly ContoData _contoData;

        public InvoiceViewModel()
        {
            _contoData = new ContoData();

            _completeList = new List<SelfInvoicesMaster>(_contoData.SelfInvoicesMasterGet());
            NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInInvoicesGrid);
            SetInvoicesList();
        }

        private void SetInvoicesList(int pageIndex = 0)
        {
            PageIndex = pageIndex + 1;
            Invoices = _completeList.Skip(pageIndex * NumberOfRowsInInvoicesGrid).Take(NumberOfRowsInInvoicesGrid).ToList();
        }

        #region INVOICES GRID PROPERTIES

        private List<SelfInvoicesMaster> _completeList;

        private List<SelfInvoicesMaster> _invoices;
        public List<SelfInvoicesMaster> Invoices
        {
            get
            {
                return _invoices;
            }
            set
            {
                _invoices = value;
                OnPropertyChanged("Invoices", false);
            }
        }

        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
                OnPropertyChanged("PageIndex", false);
            }
        }

        private int _numberOfPages;
        public int NumberOfPages
        {
            get
            {
                return _numberOfPages;
            }
            set
            {
                _numberOfPages = value;
                OnPropertyChanged("NumberOfPages", false);
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName, bool formHaveModifications = true)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                AppProperties.FormHaveModifications = formHaveModifications;
            }
        }
    }
}
