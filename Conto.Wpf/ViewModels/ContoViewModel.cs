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
    public class UnconfirmedSelfInvoiceGridViewModel
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public DateTime InvoiceDate { get; set; }
    }

    public class ContoViewModel : INotifyPropertyChanged
    {
        ContoData _contoData;

        public ContoViewModel()
        {
            _contoData = new ContoData();

            AddSelfInvoiceCommand = new RelayCommand(AddSelfInvoice_Executed);
            AddWithdrawCommand = new RelayCommand(AddWithdrawCommand_Executed);
            AddCostCommand = new RelayCommand(AddCostCommand_Executed);

            SelfInvoiceDateTime = DateTime.Now;
            CashFlowWithdrawDate = DateTime.Now;
            CashFlowCostDate = DateTime.Now;
        }

        #region Properties

        #region SelfInvoice

        private CollectionView _selfInvoiceMaterials;
        public CollectionView SelfInvoiceMaterials
        {
            get
            {
                return _selfInvoiceMaterials;
            }
            set
            {
                _selfInvoiceMaterials = value;
                OnPropertyChanged("SelfInvoiceMaterials");
            }
        }

        private int? _selfInvoiceMaterialQuantity;
        public int? SelfInvoiceMaterialQuantity
        {
            get
            {
                return _selfInvoiceMaterialQuantity;
            }
            set
            {
                _selfInvoiceMaterialQuantity = value;
                OnPropertyChanged("SelfInvoiceMaterialQuantity");
            }
        }

        private decimal? _selfInvoiceMaterialCost;
        public decimal? SelfInvoiceMaterialCost
        {
            get
            {
                return _selfInvoiceMaterialCost;
            }
            set
            {
                _selfInvoiceMaterialCost = value;
                OnPropertyChanged("SelfInvoiceMaterialCost");
            }
        }

        private decimal? _selfInvoiceMaxCost;
        public decimal? SelfInvoiceMaxCost
        {
            get
            {
                return _selfInvoiceMaxCost;
            }
            set
            {
                _selfInvoiceMaxCost = value;
                OnPropertyChanged("SelfInvoiceMaxCost");
            }
        }

        private DateTime _selfInvoiceDateTime;
        public DateTime SelfInvoiceDateTime
        {
            get { return _selfInvoiceDateTime; }
            set
            {
                _selfInvoiceDateTime = value;
                OnPropertyChanged("SelfInvoiceDateTime");
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

        #endregion

        #region CashFlow

        private decimal? _cashFlowWithdraw;
        public decimal? CashFlowWithdraw
        {
            get
            {
                return _cashFlowWithdraw;
            }
            set
            {
                _cashFlowWithdraw = value;
                OnPropertyChanged("CashFlowWithdraw");
            }
        }

        private DateTime _cashFlowWithdrawDate;
        public DateTime CashFlowWithdrawDate
        {
            get
            {
                return _cashFlowWithdrawDate;
            }
            set
            {
                _cashFlowWithdrawDate = value;
                OnPropertyChanged("CashFlowWithdrawDate");
            }
        }

        private decimal? _cashFlowCost;
        public decimal? CashFlowCost
        {
            get
            {
                return _cashFlowCost;
            }
            set
            {
                _cashFlowCost = value;
                OnPropertyChanged("CashFlowCost");
            }
        }

        private DateTime _cashFlowCostDate;
        public DateTime CashFlowCostDate
        {
            get
            {
                return _cashFlowCostDate;
            }
            set
            {
                _cashFlowCostDate = value;
                OnPropertyChanged("CashFlowCostDate");
            }
        }

        private string _cashFlowCostJustification;

        public string CashFlowCostJustification
        {
            get
            {
                return _cashFlowCostJustification;
            }
            set
            {
                _cashFlowCostJustification = value;
                OnPropertyChanged("CashFlowCostJustification");
            }
        }

        #endregion


        
        #endregion

        #region Commands

        #region SelfInvoice

        public ICommand AddSelfInvoiceCommand { get; set; }

        public void AddSelfInvoice_Executed(object sender)
        {
            MessageBox.Show("AddSelfInvoice");
        }

        #endregion

        #region CashFlow

        public ICommand AddWithdrawCommand { get; set; }

        public void AddWithdrawCommand_Executed(object sender)
        {
            if (CashFlowWithdraw.HasValue && CashFlowWithdraw.Value > 0)
            {
                _contoData.CashFlowAdd(new CashFlowDataObject
                {
                    Cash = CashFlowWithdraw.Value,
                    Description = "Prelievo per cassa",
                    FlowDate = CashFlowWithdrawDate,
                });

                CashFlowWithdraw = 0;
                CashFlowWithdrawDate = DateTime.Now;
            }
            else
            {
                MessageBox.Show("Immettere un valore di prelievo");
            }
        }

        public ICommand AddCostCommand { get; set; }

        public void AddCostCommand_Executed(object sender)
        {
            if (_cashFlowCost.HasValue && _cashFlowCost.Value > 0 && !string.IsNullOrEmpty(CashFlowCostJustification))
            {

            }
            else
            {
                MessageBox.Show("Immettere un costo ed una causale");
            }
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
