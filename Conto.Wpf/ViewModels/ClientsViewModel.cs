using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Conto.Data;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class ClientsViewModel : INotifyPropertyChanged
    {
        private const int NumberOfRowsInClientsGrid = 10;
        private readonly ContoData _contoData;

        public ClientsViewModel()
        {
            _contoData = new ContoData();

            _completeList = new List<Client>(_contoData.ClientsGet());
            NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInClientsGrid);
            SetClientsList();

            AddClientCommand = new RelayCommand(AddClientCommand_Executed);
            RemoveClientCommand = new RelayCommand(RemoveClientCommand_Executed);
            ModifyClientCommand = new RelayCommand(ModifyClientCommand_Executed);
            UpdateClientCommand = new RelayCommand(UpdateClientCommand_Executed);

            UpdatePanelVisibility = Visibility.Collapsed;
        }

        private void SetClientsList(int pageIndex = 0)
        {
            PageIndex = pageIndex + 1;
            Clients = _completeList.Skip(pageIndex * NumberOfRowsInClientsGrid).Take(NumberOfRowsInClientsGrid).ToList();
        }

        #region NEW CLIENT PROPERTIES

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

        #endregion

        #region CLIENTS GRID PROPERTIES

        private List<Client> _completeList;

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

        #region UPDATE CLIENTS PROPERTIES

        private long _existingId;
        public long ExistingId
        {
            get
            {
                return _existingId;
            }
            set
            {
                _existingId = value;
                OnPropertyChanged("ExistingId", false);
            }
        }

        private string _existingName;
        public string ExistingName
        {
            get
            {
                return _existingName;
            }
            set
            {
                _existingName = value;
                OnPropertyChanged("ExistingName", false);
            }
        }

        private string _existingAddress;
        public string ExistingAddress
        {
            get
            {
                return _existingAddress;
            }
            set
            {
                _existingAddress = value;
                OnPropertyChanged("ExistingAddress", false);
            }
        }

        private string _existingCity;
        public string ExistingCity
        {
            get
            {
                return _existingCity;
            }
            set
            {
                _existingCity = value;
                OnPropertyChanged("ExistingCity", false);
            }
        }

        private string _existingPostalCode;
        public string ExistingPostalCode
        {
            get
            {
                return _existingPostalCode;
            }
            set
            {
                _existingPostalCode = value;
                OnPropertyChanged("ExistingPostalCode", false);
            }
        }

        private string _existingFiscalCode;
        public string ExistingFiscalCode
        {
            get
            {
                return _existingFiscalCode;
            }
            set
            {
                _existingFiscalCode = value;
                OnPropertyChanged("ExistingFiscalCode", false);
            }
        }

        private string _existingVatCode;
        public string ExistingVatCode
        {
            get
            {
                return _existingVatCode;
            }
            set
            {
                _existingVatCode = value;
                OnPropertyChanged("ExistingVatCode", false);
            }
        }

        
        private Visibility _updatePanelVisibility;
        public Visibility UpdatePanelVisibility
        {
            get { return _updatePanelVisibility; }
            set
            {
                _updatePanelVisibility = value;
                OnPropertyChanged("UpdatePanelVisibility", false);
            }
        }

        #endregion

        #region COMMANDS

        public ICommand AddClientCommand { get; set; }
        public void AddClientCommand_Executed(object sender)
        {
            if (AppProperties.FormHaveModifications)
            {
                _contoData.ClientAdd(new Client
                {
                    Name = Name,
                    Address = Address,
                    City = City,
                    PostalCode = PostalCode,
                    FiscalCode = FiscalCode,
                    VatCode = VatCode
                });

                Name = string.Empty;
                Address = string.Empty;
                City = string.Empty;
                PostalCode = string.Empty;
                FiscalCode = string.Empty;
                VatCode = string.Empty;

                _completeList = new List<Client>(_contoData.ClientsGet());
                NumberOfPages = (int) Math.Ceiling((double) _completeList.Count/NumberOfRowsInClientsGrid);
                SetClientsList();

                AppProperties.FormHaveModifications = false;
            }
        }

        public ICommand RemoveClientCommand { get; set; }
        public void RemoveClientCommand_Executed(object sender)
        {

            if (_contoData.ClientDelete((Client)sender))
            {
                _completeList = new List<Client>(_contoData.ClientsGet());
                NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInClientsGrid);
                SetClientsList();
                AppProperties.FormHaveModifications = false;
            }
            else
            {
                MessageBox.Show("Impossibile eliminare il Cliente perchè in uso nelle fatture!");
            }
        }

        public ICommand ModifyClientCommand { get; set; }
        public void ModifyClientCommand_Executed(object sender)
        {
            var client = (Client)sender;
            ExistingId = client.Id;
            ExistingName = client.Name;
            ExistingAddress = client.Address;
            ExistingCity = client.City;
            ExistingPostalCode = client.PostalCode;
            ExistingFiscalCode = client.FiscalCode;
            ExistingVatCode = client.VatCode;

            UpdatePanelVisibility = Visibility.Visible;
        }

        public ICommand UpdateClientCommand { get; set; }
        public void UpdateClientCommand_Executed(object sender)
        {
            _contoData.ClientUpdate(new Client
            {
                Id = ExistingId,
                Name = ExistingName,
                Address = ExistingAddress,
                City = ExistingCity,
                PostalCode = ExistingPostalCode,
                FiscalCode = ExistingFiscalCode,
                VatCode = ExistingVatCode
            });

            _completeList = new List<Client>(_contoData.ClientsGet());
            SetClientsList();

            UpdatePanelVisibility = Visibility.Collapsed;
        }

        #region PAGING COMMANDS

        private ICommand _firstPageCommand;
        public ICommand FirstPage
        {
            get { return _firstPageCommand ?? (_firstPageCommand = new RelayCommand(First_Page)); }
        }

        public void First_Page(object sender)
        {
            SetClientsList();
        }

        private ICommand _previousPageCommand;
        public ICommand PreviousPage
        {
            get { return _previousPageCommand ?? (_previousPageCommand = new RelayCommand(Previous_Page)); }
        }

        public void Previous_Page(object sender)
        {
            if (PageIndex > 1)
                SetClientsList(PageIndex - 2);
        }

        private ICommand _nextPageCommand;
        public ICommand NextPage
        {
            get { return _nextPageCommand ?? (_nextPageCommand = new RelayCommand(Next_Page)); }
        }

        public void Next_Page(object sender)
        {
            if (PageIndex < NumberOfPages)
                SetClientsList(PageIndex);
        }

        private ICommand _lastPageCommand;
        public ICommand LastPage
        {
            get { return _lastPageCommand ?? (_lastPageCommand = new RelayCommand(Last_Page)); }
        }

        public void Last_Page(object sender)
        {
            SetClientsList(NumberOfPages - 1);
        }

        #endregion

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
