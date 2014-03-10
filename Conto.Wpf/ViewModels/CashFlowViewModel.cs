using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

        public CashFlowViewModel()
        {
            DepositCommand = new RelayCommand(DepositCommand_Executed);
            CostCommand = new RelayCommand(CostCommand_Executed);

            DepositDate = DateTime.Now;
            CostDate = DateTime.Now;
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

        #region Commands

        public ICommand DepositCommand { get; set; }

        public void DepositCommand_Executed(object sender)
        {
            
        }

        public ICommand CostCommand { get; set; }

        public void CostCommand_Executed(object sender)
        {

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
