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

namespace Conto.Wpf.CashFlow
{
    public class CashFlowViewModel : INotifyPropertyChanged
    {
        public CashFlowViewModel()
        {
            AddWithdrawCommand = new RelayCommand(AddWithdrawCommand_Executed);
            AddCostCommand = new RelayCommand(AddCostCommand_Executed);

            WithdrawDate = DateTime.Now;
            CostDate = DateTime.Now;

            SelfInvoices = new CollectionView(new List<string> {"item1", "item2", "item3"});

            Balance = 60000;
        }

        public decimal Balance { get; set; }


        public decimal Withdraw { get; set; }

        public DateTime WithdrawDate { get; set; }

        public decimal Cost { get; set; }

        public DateTime CostDate { get; set; }

        public string CostJustification { get; set; }

        public CollectionView SelfInvoices { get; set; }


        public ICommand AddWithdrawCommand { get; set; }

        public void AddWithdrawCommand_Executed(object sender)
        {
            MessageBox.Show("AddWithdrawCommand");
        }

        public ICommand AddCostCommand { get; set; }

        public void AddCostCommand_Executed(object sender)
        {
            if (Cost > 0 && !string.IsNullOrEmpty(CostJustification))
            {
                MessageBox.Show(CostJustification + " " + Cost);
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
