using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Conto.Data;
using Conto.Wpf.GridPaging;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class NewInvoiceContent
    {
        public NewInvoiceContent()
            : this(Guid.NewGuid())
        {
        }

        public NewInvoiceContent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public List<NewInvoiceMaterialRow> Rows { get; set; }
    }

    public class NewInvoiceMaterialRow
    {
        public NewInvoiceMaterialRow()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid MasterId { get; set; }
        public string Description { get; set; }
        public decimal? MaterialQuantity { get; set; }
        public decimal? MaterialPrice { get; set; }
        public decimal MaterialCost { get; set; }
    }

    public class InvoiceViewModel : GridPaging<InvoiceMaster>, INotifyPropertyChanged
    {
        private const int NumberOfRowsInInvoicesGrid = 10;
        readonly ContoData _contoData;

        public InvoiceViewModel()
            : base(NumberOfRowsInInvoicesGrid)
        {
            _contoData = new ContoData();

            Clients = new List<Client>(_contoData.ClientsGet());
            InvoiceDate = DateTime.Now;
            Measures = new List<Measures>(_contoData.MeasuresGet());

            var invoiceRowId = Guid.NewGuid();
            InvoiceRows = new List<NewInvoiceContent> { new NewInvoiceContent(invoiceRowId) { Rows = new List<NewInvoiceMaterialRow> { new NewInvoiceMaterialRow{ MasterId = invoiceRowId } } } };

            Initialize(OnPropertyChanged, _contoData.InvoicesMasterGet);
        }

        #region NEW INVOICE

        private long? _selectedClient;
        public long? SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        private List<Client> _clients;
        public List<Client> Clients
        {
            get { return _clients; }
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        private DateTime _invoiceDate;
        public DateTime InvoiceDate
        {
            get { return _invoiceDate; }
            set
            {
                _invoiceDate = value;
                OnPropertyChanged();
            }
        }

        private int? _selectedMeasure;
        public int? SelectedMeasure
        {
            get { return _selectedMeasure; }
            set
            {
                _selectedMeasure = value;
                OnPropertyChanged();
            }
        }

        private List<Measures> _measures;
        public List<Measures> Measures
        {
            get
            {
                return _measures;
            }
            set
            {
                _measures = value;
                OnPropertyChanged();
            }
        }

        private List<NewInvoiceContent> _invoiceRows;
        public List<NewInvoiceContent> InvoiceRows
        {
            get { return _invoiceRows; }
            set
            {
                _invoiceRows = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region UPDATE INVOICE

        #endregion

        #region COMMANDS

        #region NEW INVOICE COMMANDS

        private ICommand _addInvoiceRowCommand;
        public ICommand AddInvoiceRowCommand
        {
            get { return _addInvoiceRowCommand ?? (_addInvoiceRowCommand = new RelayCommand(AddInvoiceRowCommand_Executed)); }
        }
        public void AddInvoiceRowCommand_Executed(object sender)
        {
            var tmpinvoicerows = InvoiceRows;
            var invoiceRowId = Guid.NewGuid();
            tmpinvoicerows.Add(new NewInvoiceContent(invoiceRowId) { Rows = new List<NewInvoiceMaterialRow> { new NewInvoiceMaterialRow {MasterId = invoiceRowId} } });
            InvoiceRows = null;
            InvoiceRows = tmpinvoicerows;
        }

        private ICommand _removeInvoiceRowCommand;
        public ICommand RemoveInvoiceRowCommand
        {
            get { return _removeInvoiceRowCommand ?? (_removeInvoiceRowCommand = new RelayCommand(RemoveInvoiceRowCommand_Executed)); }
        }
        public void RemoveInvoiceRowCommand_Executed(object sender)
        {
            var currentRow = (NewInvoiceContent)sender;
            var tmpinvoicerows = InvoiceRows;
            foreach (var newInvoiceContent in tmpinvoicerows)
            {
                if (newInvoiceContent.Id == currentRow.Id)
                {
                    tmpinvoicerows.Remove(newInvoiceContent);
                    break;
                }
            }
            InvoiceRows = null;
            InvoiceRows = tmpinvoicerows;
        }

        private ICommand _addInvoiceContentRowCommand;
        public ICommand AddInvoiceContentRowCommand
        {
            get { return _addInvoiceContentRowCommand ?? (_addInvoiceContentRowCommand = new RelayCommand(AddInvoiceContentRowCommand_Executed)); }
        }
        public void AddInvoiceContentRowCommand_Executed(object sender)
        {
            var currentRow = (NewInvoiceContent)sender;

            var tmpinvoicerows = InvoiceRows;
            foreach (var newInvoiceContent in tmpinvoicerows.Where(newInvoiceContent => newInvoiceContent.Id == currentRow.Id))
            {
                newInvoiceContent.Rows.Add(new NewInvoiceMaterialRow {MasterId  = currentRow.Id});
                break;
            }
            InvoiceRows = null;
            InvoiceRows = tmpinvoicerows;
        }

        private ICommand _removeInvoiceContentRowCommand;
        public ICommand RemoveInvoiceContentRowCommand
        {
            get { return _removeInvoiceContentRowCommand ?? (_removeInvoiceContentRowCommand = new RelayCommand(RemoveInvoiceContentRowCommand_Executed)); }
        }
        public void RemoveInvoiceContentRowCommand_Executed(object sender)
        {
            var currentRow = (NewInvoiceMaterialRow)sender;

            var tmpinvoicerows = InvoiceRows;
            foreach (var newInvoiceContent in tmpinvoicerows.Where(newInvoiceContent => newInvoiceContent.Id == currentRow.MasterId))
            {
                foreach (var newInvoiceMaterialRow in newInvoiceContent.Rows)
                {
                    if (newInvoiceMaterialRow.Id == currentRow.Id)
                    {
                        newInvoiceContent.Rows.Remove(newInvoiceMaterialRow);
                        break;
                    }
                }
            }

            InvoiceRows = null;
            InvoiceRows = tmpinvoicerows;
        }

        #endregion

        private ICommand _addInvoiceCommand;
        public ICommand AddInvoiceCommand 
        { 
            get { return _addInvoiceCommand ?? (_addInvoiceCommand = new RelayCommand(AddInvoiceCommand_Executed)); } 
        }
        public void AddInvoiceCommand_Executed(object sender)
        {
            if (AppProperties.FormHaveModifications)
            {
                //_contoData.InvoiceAdd(new Invoice
                //{
                //});

                MessageBox.Show(SelectedClient.HasValue ? SelectedClient.Value.ToString() : "ops");
                
                AppProperties.FormHaveModifications = false;
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "", bool formHaveModifications = true)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                AppProperties.FormHaveModifications = formHaveModifications;
            }
        }
    }
}
