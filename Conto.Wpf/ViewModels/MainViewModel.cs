using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Conto.Data;

namespace Conto.Wpf.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        ContoData _contoData;

        public MainViewModel()
        {
            _contoData = new ContoData();

            InvoceButtonStyle = Application.Current.Resources["MainMenuButtonStyle"] as Style;
            CashButtonStyle = Application.Current.Resources["CashMenuButtonStyle"] as Style;

            SelfInvoiceCommand = new RelayCommand(SelfInvoice_Executed);
            CashFlowCommand = new RelayCommand(CashFlowCommand_Executed);
        }

        private Style _invoiceButtonStyle;

        public Style InvoceButtonStyle
        {
            get
            {
                return _invoiceButtonStyle;
            }
            set
            {
                _invoiceButtonStyle = value;
                OnPropertyChanged("InvoceButtonStyle");
            }
        }

        private Style _cashButtonStyle;

        public Style CashButtonStyle
        {
            get { return _cashButtonStyle; }
            set
            {
                _cashButtonStyle = value;
                OnPropertyChanged("CashButtonStyle");
            }
        }

        private Control _selectedControl;

        public Control SelectedControl
        {
            get { return _selectedControl; }
            set
            {
                _selectedControl = value;
                OnPropertyChanged("SelectedControl");
            }
        }

        #region Commands


        public ICommand SelfInvoiceCommand { get; set; }

        public void SelfInvoice_Executed(object sender)
        {
            InvoceButtonStyle = Application.Current.Resources["SelectedMenuButtonStyle"] as Style;
            CashButtonStyle = Application.Current.Resources["CashMenuButtonStyle"] as Style;
            SelectedControl = new SelfInvoiceUserControl();
        }

        public ICommand CashFlowCommand { get; set; }

        public void CashFlowCommand_Executed(object sender)
        {
            InvoceButtonStyle = Application.Current.Resources["MainMenuButtonStyle"] as Style;
            CashButtonStyle = Application.Current.Resources["SelectedMenuButtonStyle"] as Style;
            SelectedControl = new CashFlowUserControl();
        }

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
