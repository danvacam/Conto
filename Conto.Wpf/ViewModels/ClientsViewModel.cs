using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Conto.Data;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class ClientsViewModel : INotifyPropertyChanged
    {
        private readonly ContoData _contoData;

        public ClientsViewModel()
        {
            _contoData = new ContoData();
            Clients = new List<Client>(_contoData.ClientsGet());

            AddClientCommand = new RelayCommand(AddClientCommand_Executed);
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }

        private string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                OnPropertyChanged("PostalCode");
            }
        }

        private string _fiscalCode;
        public string FiscalCode
        {
            get { return _fiscalCode; }
            set
            {
                _fiscalCode = value;
                OnPropertyChanged("FiscalCode");
            }
        }

        private string _vatCode;
        public string VatCode
        {
            get { return _vatCode; }
            set
            {
                _vatCode = value;
                OnPropertyChanged("VatCode");
            }
        }

        private List<Client> _clients;
        public List<Client> Clients
        {
            get
            {
                return _clients;
            }
            set
            {
                _clients = value;
                OnPropertyChanged("Clients");
            }
        }

        public ICommand AddClientCommand { get; set; }
        public void AddClientCommand_Executed(object sender)
        {

            Name = null;
            Address = null;
            City = null;
            PostalCode = null;
            FiscalCode = null;
            VatCode = null;

            _contoData.ClientAdd(new Client());

            Clients = new List<Client>(_contoData.ClientsGet());

            AppProperties.FormHaveModifications = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                AppProperties.FormHaveModifications = true;
            }
        }
    }
}
