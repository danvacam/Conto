using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Conto.Data;

namespace Conto.Wpf.CashFlow
{
    public class CashFlowViewModel : INotifyPropertyChanged
    {
        ContoData _contoData;

        public CashFlowViewModel()
        {
            _contoData = new ContoData();

            AddWithdrawCommand = new RelayCommand(AddWithdrawCommand_Executed);
            AddCostCommand = new RelayCommand(AddCostCommand_Executed);

            WithdrawDate = DateTime.Now;
            CostDate = DateTime.Now;

            SelfInvoices = new CollectionView(_contoData.CashFlowSelfInvoices());
            

            Balance = _contoData.CashFlowBalance();
        }

        private decimal _balance;
        public decimal Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;
                OnPropertyChanged("Balance");
            }
        }

        private decimal _withdraw;
        public decimal Withdraw
        {
            get
            {
                return _withdraw;
            }
            set
            {
                _withdraw = value;
                OnPropertyChanged("Withdraw");
            }
        }

        private DateTime _withdrawDate;
        public DateTime WithdrawDate
        {
            get
            {
                return _withdrawDate;
            }
            set{
                _withdrawDate = value;
                OnPropertyChanged("WithdrawDate");
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

        private CollectionView _selfInvoices;
        public CollectionView SelfInvoices
        {
            get
            {
                return _selfInvoices;
            }
            set
            {
                _selfInvoices = value;
                OnPropertyChanged("SelfInvoices");
            }
        }


        public ICommand AddWithdrawCommand { get; set; }

        public void AddWithdrawCommand_Executed(object sender)
        {
            if (Withdraw > 0)
            {
                _contoData.CashFlowAdd(new CashFlowDataObject
                {
                    Cash = Withdraw,
                    Description = "Prelievo per cassa",
                    FlowDate = WithdrawDate,
                });

                Balance = _contoData.CashFlowBalance();
                Withdraw = 0;
                WithdrawDate = DateTime.Now;
            }
            else
            {
                MessageBox.Show("Immettere un valore di prelievo");
            }
        }

        public ICommand AddCostCommand { get; set; }

        public void AddCostCommand_Executed(object sender)
        {
            if (Cost > 0 && !string.IsNullOrEmpty(CostJustification))
            {
                
            }
            else
            {
                MessageBox.Show("Immettere un costo ed una causale");
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
