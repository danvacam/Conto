using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Conto.Data;
using Conto.Data.PdfHelpers;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class UnconfirmedCashFlow
    {
        public decimal DepositValue { get; set; }
        public DateTime DepositDate { get; set; }
        public decimal CostValue { get; set; }
        public DateTime CostDate { get; set; }
        public string CostJustification { get; set; }
    }

    public class CashFlowViewModel : INotifyPropertyChanged
    {

        private readonly ContoData _contoData;

        public CashFlowViewModel()
        {
            _contoData = new ContoData();

            DepositCommand = new RelayCommand(DepositCommand_Executed);
            CostCommand = new RelayCommand(CostCommand_Executed);
            FilterGridCommand = new RelayCommand(FilterGridCommand_Executed);
            ExportPdf = new RelayCommand(ExportPdf_Executed);
            AddSelfInvoiceToPrintCommand = new RelayCommand(AddSelfInvoiceToPrintCommand_Executed);

            var lastDate = _contoData.CashFlowGetLastDateTime();
            SelectedYear = lastDate.Year;
            SelectedMonth = lastDate.Month;

            CashFlows = new List<CashFlow>(_contoData.CashFlowGetYearMonth(lastDate.Year, lastDate.Month));

            DepositDate = DateTime.Now;
            CostDate = DateTime.Now;
            SelfInvoicePrintButtonVisibility = Visibility.Collapsed;
            
        }
        
        private decimal _depostit;
        public decimal Deposit
        {
            get
            {
                return _depostit;
            }
            set
            {
                _depostit = value;
                OnPropertyChanged("Deposit");
            }
        }

        private DateTime _depostitDate;
        public DateTime DepositDate
        {
            get
            {
                return _depostitDate;
            }
            set
            {
                _depostitDate = value;
                OnPropertyChanged("DepositDate");
            }
        }

        private decimal _cost;
        public decimal Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
                OnPropertyChanged("Cost");
            }
        }

        private DateTime _costDate;
        public DateTime CostDate
        {
            get
            {
                return _costDate;
            }
            set
            {
                _costDate = value;
                OnPropertyChanged("CostDate");
            }
        }

        private string _costJustification;
        public string CostJustification
        {
            get
            {
                return _costJustification;
            }
            set
            {
                _costJustification = value;
                OnPropertyChanged("CostJustification");
            }
        }

        #region CASH FLOW GRID

        private int? _selectedMonth;
        public int? SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                OnPropertyChanged("SelectedMonth");
            }
        }

        private Dictionary<int, string> _months;
        public Dictionary<int, string> Months
        {
            get { return _months; }
            set
            {
                _months = value;
                OnPropertyChanged("Months");
            }
        }

        private int? _selectedYear;
        public int? SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged("SelectedYear");
            }
        }

        private Dictionary<int, string> _years;
        public Dictionary<int, string> Years
        {
            get { return _years; }
            set
            {
                _years = value;
                OnPropertyChanged("Years");
            }
        }

        private List<CashFlow> _cashFlows;
        public List<CashFlow> CashFlows
        {
            get
            {
                return _cashFlows;
            }
            set
            {
                _cashFlows = value;
                OnPropertyChanged("CashFlows");
            }
        }

        #endregion

        #region PRINT

        private int _totSelfInvoicePrint;
        private List<CashFlow> _selfInvoicePrintList = new List<CashFlow>();

        private string _selfInvoicePrint;
        public string SelfInvoicePrint
        {
            get
            {
                return _selfInvoicePrint;
            }
            set
            {
                _selfInvoicePrint = value;
                OnPropertyChanged("SelfInvoicePrint");
            }
        }

        private Visibility _selfInvoicePrintButtonVisibility;
        public Visibility SelfInvoicePrintButtonVisibility
        {
            get
            {
                return _selfInvoicePrintButtonVisibility;
            }
            set
            {
                _selfInvoicePrintButtonVisibility = value;
                OnPropertyChanged("SelfInvoicePrintButtonVisibility");
            }
        }

        #endregion

        #region Commands

        public ICommand DepositCommand { get; set; }
        public void DepositCommand_Executed(object sender)
        {
            
        }

        public ICommand CostCommand { get; set; }
        public void CostCommand_Executed(object sender)
        {

        }

        public ICommand FilterGridCommand { get; set; }
        public void FilterGridCommand_Executed(object sender)
        {
            if (SelectedYear.HasValue && SelectedMonth.HasValue)
                CashFlows = new List<CashFlow>(_contoData.CashFlowGetYearMonth(SelectedYear.Value, SelectedMonth.Value));
            else
                MessageBox.Show("Selezionare anno e mese");
        }

        public ICommand ExportPdf { get; set; }
        public void ExportPdf_Executed(object sender)
        {
            ItextObjectExport.ExportSelfInvoicesPdf(_contoData.SelfInvoiceGetByCashFlow(((CashFlow) sender).Id));
        }

        public ICommand AddSelfInvoiceToPrintCommand { get; set; }
        public void AddSelfInvoiceToPrintCommand_Executed(object sender)
        {
            var item = (CashFlow) sender;

            if (!_selfInvoicePrintList.Contains(item))
            {
                _selfInvoicePrintList.Add(item);
                _totSelfInvoicePrint++;
                SelfInvoicePrint = string.Format("Pdf autofatture {0}", _totSelfInvoicePrint);
                SelfInvoicePrintButtonVisibility = Visibility.Visible;
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                AppProperties.FormHaveModifications = true;
            }
        }

        #endregion

    }
}
