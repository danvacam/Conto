using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Conto.Data;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            var contoData = new ContoData();

            var settings = contoData.GetSettings();

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

            InvoceButtonStyle = Application.Current.Resources["MainMenuButtonStyle"] as Style;
            CashButtonStyle = Application.Current.Resources["CashMenuButtonStyle"] as Style;
            MaterialsButtonStyle = Application.Current.Resources["MaterialsMenuButtonStyle"] as Style;

            SelfInvoiceCommand = new RelayCommand(SelfInvoice_Executed);
            CashFlowCommand = new RelayCommand(CashFlowCommand_Executed);
            MaterialsCommand = new RelayCommand(MaterialsCommand_Executed);
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

        private Style _materialsButtonStyle;

        public Style MaterialsButtonStyle
        {
            get { return _materialsButtonStyle; }
            set
            {
                _materialsButtonStyle = value;
                OnPropertyChanged("MaterialsButtonStyle");
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

        #region Settings Properties

        private string _invoiceOwnerName;
        public string InvoiceOwnerName
        {
            get { return _invoiceOwnerName; }
            set
            {
                _invoiceOwnerName = value;
                OnPropertyChanged("InvoiceOwnerName");
            }
        }

        private string _invoiceOwnerAddress;
        public string InvoiceOwnerAddress
        {
            get { return _invoiceOwnerAddress; }
            set
            {
                _invoiceOwnerAddress = value;
                OnPropertyChanged("InvoiceOwnerAddress");
            }
        }

        private string _invoiceOwnerCity;
        public string InvoiceOwnerCity
        {
            get { return _invoiceOwnerCity; }
            set
            {
                _invoiceOwnerCity = value;
                OnPropertyChanged("InvoiceOwnerCity");
            }
        }

        private string _invoiceOwnerPostalCode;
        public string InvoiceOwnerPostalCode
        {
            get { return _invoiceOwnerPostalCode; }
            set
            {
                _invoiceOwnerPostalCode = value;
                OnPropertyChanged("InvoiceOwnerPostalCode");
            }
        }

        private string _invoiceOwnerFiscalCode;
        public string InvoiceOwnerFiscalCode
        {
            get { return _invoiceOwnerFiscalCode; }
            set
            {
                _invoiceOwnerFiscalCode = value;
                OnPropertyChanged("InvoiceOwnerFiscalCode");
            }
        }

        private string _invoiceOwnerVatCode;
        public string InvoiceOwnerVatCode
        {
            get { return _invoiceOwnerVatCode; }
            set
            {
                _invoiceOwnerVatCode = value;
                OnPropertyChanged("InvoiceOwnerVatCode");
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
                OnPropertyChanged("InvoiceMaxCost");
            }
        }

        #endregion

        #region Commands


        public ICommand SelfInvoiceCommand { get; set; }
        public void SelfInvoice_Executed(object sender)
        {
            if (!AppProperties.FormHaveModifications || DiscardFormModifications())
            {
                InvoceButtonStyle = Application.Current.Resources["SelectedMenuButtonStyle"] as Style;
                CashButtonStyle = Application.Current.Resources["CashMenuButtonStyle"] as Style;
                MaterialsButtonStyle = Application.Current.Resources["MaterialsMenuButtonStyle"] as Style;
                SelectedControl = new SelfInvoiceUserControl();
            }
        }

        public ICommand CashFlowCommand { get; set; }
        public void CashFlowCommand_Executed(object sender)
        {
            if (!AppProperties.FormHaveModifications || DiscardFormModifications())
            {
                InvoceButtonStyle = Application.Current.Resources["MainMenuButtonStyle"] as Style;
                CashButtonStyle = Application.Current.Resources["SelectedMenuButtonStyle"] as Style;
                MaterialsButtonStyle = Application.Current.Resources["MaterialsMenuButtonStyle"] as Style;
                SelectedControl = new CashFlowUserControl();
            }
        }

        public ICommand MaterialsCommand { get; set; }
        public void MaterialsCommand_Executed(object sender)
        {
            if (!AppProperties.FormHaveModifications || DiscardFormModifications())
            {
                InvoceButtonStyle = Application.Current.Resources["MainMenuButtonStyle"] as Style;
                CashButtonStyle = Application.Current.Resources["CashMenuButtonStyle"] as Style;
                MaterialsButtonStyle = Application.Current.Resources["SelectedMenuButtonStyle"] as Style;
                SelectedControl = new MaterialsUserControl();
            }
        }

        public bool DiscardFormModifications()
        {
            var result = MessageBox.Show("Proseguire perdendo le modifiche?", "Conferma", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
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
