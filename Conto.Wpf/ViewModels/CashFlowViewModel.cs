using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Conto.Data;
using Conto.Data.PdfHelpers;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class CashFlowGridRow
    {
        public CashFlowGridRow()
        {
        }

        public CashFlowGridRow(CashFlow cashFlow)
        {
            Id = cashFlow.Id;
            Cash = cashFlow.Cash;
            Description = cashFlow.Description;
            FlowDate = cashFlow.FlowDate.ToString("dd/MM/yyyy");
            CashFlowType = cashFlow.CashFlowType;
            AddToPdfButtonVisibility = cashFlow.CashFlowType == "SelfInvoice" ? Visibility.Visible : Visibility.Hidden;
        }

        public long Id { get; set; }
        public decimal Cash { get; set; }
        public string Description { get; set; }
        public string FlowDate { get; set; }
        public string CashFlowType { get; set; }
        public Visibility AddToPdfButtonVisibility;
    }

    public class CashFlowViewModel : INotifyPropertyChanged
    {
        private const int NumberOfRowsInCashFlowsGrid = 10;
        private readonly ContoData _contoData;

        public CashFlowViewModel()
        {
            _contoData = new ContoData();

            DepositCommand = new RelayCommand(DepositCommand_Executed);
            CostCommand = new RelayCommand(CostCommand_Executed);
            FilterGridCommand = new RelayCommand(FilterGridCommand_Executed);
            ExportPdf = new RelayCommand(ExportPdf_Executed);
            AddSelfInvoiceToPrintCommand = new RelayCommand(AddSelfInvoiceToPrintCommand_Executed);

            Months = new Dictionary<int, string>
            {
                {1,"Gennaio"},
                {2,"Febbraio"},
                {3,"Marzo"},
                {4,"Aprile"},
                {5,"Maggio"},
                {6,"Giugno"},
                {7,"Luglio"},
                {8,"Agosto"},
                {9,"Settembre"},
                {10,"Ottobre"},
                {11,"Novembre"},
                {12,"Dicembre"},
            };

            var lastDate = _contoData.CashFlowGetLastDateTime();
            SelectedYear = lastDate.Year;
            SelectedMonth = lastDate.Month;

            _completeList = CashFlowListToCashFlowGridRows(new List<CashFlow>(_contoData.CashFlowGetYearMonth(lastDate.Year, lastDate.Month)));
            NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInCashFlowsGrid);
            SetCashFlowsList();

            DepositDate = DateTime.Now;
            CostDate = DateTime.Now;
            SelfInvoicePrintButtonVisibility = Visibility.Collapsed;
            
        }

        private void SetCashFlowsList(int pageIndex = 0)
        {
            PageIndex = pageIndex + 1;
            CashFlows = _completeList.Skip(pageIndex * NumberOfRowsInCashFlowsGrid).Take(NumberOfRowsInCashFlowsGrid).ToList();
        }

        private List<CashFlowGridRow> CashFlowListToCashFlowGridRows(IEnumerable<CashFlow> cashFlowList)
        {
            return cashFlowList.Select(cashFlow => new CashFlowGridRow(cashFlow)).ToList();
        }


        private decimal? _depostit;
        public decimal? Deposit
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

        private decimal? _cost;
        public decimal? Cost
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

        #region CASH FLOW GRID FILTERS

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

        

        #endregion

        #region PRINT

        private int _totSelfInvoicePrint;
        
        private List<CashFlowGridRow> _selfInvoicePrintList = new List<CashFlowGridRow>();
        public List<CashFlowGridRow> SelfInvoicePrintList
        {
            get
            {
                return _selfInvoicePrintList;
            }
            set
            {
                _selfInvoicePrintList = value;
                OnPropertyChanged("SelfInvoicePrintList");
            }
        }

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

        #region CASH FLOW GRID PROPERTIES

        private List<CashFlowGridRow> _completeList;

        private List<CashFlowGridRow> _cashFlows;
        public List<CashFlowGridRow> CashFlows
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
                CashFlows = CashFlowListToCashFlowGridRows(new List<CashFlow>(_contoData.CashFlowGetYearMonth(SelectedYear.Value, SelectedMonth.Value)));
            else
                MessageBox.Show("Selezionare anno e mese");
        }

        public ICommand ExportPdf { get; set; }
        public void ExportPdf_Executed(object sender)
        {
            ItextObjectExport.ExportSelfInvoicesPdf(_contoData.SelfInvoiceGetByCashFlow(((CashFlowGridRow)sender).Id));
        }

        public ICommand AddSelfInvoiceToPrintCommand { get; set; }
        public void AddSelfInvoiceToPrintCommand_Executed(object sender)
        {
            var item = (CashFlowGridRow) sender;

            if (!_selfInvoicePrintList.Contains(item))
            {
                foreach (var cashFlowGridRow in CashFlows)
                {
                    if (cashFlowGridRow.Id == item.Id)
                    {
                        cashFlowGridRow.AddToPdfButtonVisibility = Visibility.Hidden;
                    }
                }

                var cashFlows = CashFlows;
                CashFlows = null;
                CashFlows = cashFlows;

                var selfInvoicesPrintList = SelfInvoicePrintList;
                selfInvoicesPrintList.Add(item);
                SelfInvoicePrintList = null;
                SelfInvoicePrintList = selfInvoicesPrintList;

                _totSelfInvoicePrint++;
                SelfInvoicePrint = string.Format("Pdf autofatture {0}", _totSelfInvoicePrint);
                SelfInvoicePrintButtonVisibility = Visibility.Visible;
            }
        }

        #region PAGING COMMANDS

        private ICommand _firstPageCommand;
        public ICommand FirstPage
        {
            get { return _firstPageCommand ?? (_firstPageCommand = new RelayCommand(First_Page)); }
        }

        public void First_Page(object sender)
        {
            SetCashFlowsList();
        }

        private ICommand _previousPageCommand;
        public ICommand PreviousPage
        {
            get { return _previousPageCommand ?? (_previousPageCommand = new RelayCommand(Previous_Page)); }
        }

        public void Previous_Page(object sender)
        {
            if (PageIndex > 1)
                SetCashFlowsList(PageIndex - 2);
        }

        private ICommand _nextPageCommand;
        public ICommand NextPage
        {
            get { return _nextPageCommand ?? (_nextPageCommand = new RelayCommand(Next_Page)); }
        }

        public void Next_Page(object sender)
        {
            if (PageIndex < NumberOfPages)
                SetCashFlowsList(PageIndex);
        }

        private ICommand _lastPageCommand;
        public ICommand LastPage
        {
            get { return _lastPageCommand ?? (_lastPageCommand = new RelayCommand(Last_Page)); }
        }

        public void Last_Page(object sender)
        {
            SetCashFlowsList(NumberOfPages - 1);
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName, bool formHaveModifications = true)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                AppProperties.FormHaveModifications = formHaveModifications;
            }
        }

        #endregion

    }
}
