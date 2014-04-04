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
    public class SelfInvoiceViewModel : INotifyPropertyChanged
    {
        private const int NumberOfRowsInSelfInvoicesGrid = 10;
        readonly ContoData _contoData;

        public SelfInvoiceViewModel()
        {
            _contoData = new ContoData();

            _completeList = new List<SelfInvoicesMaster>(_contoData.SelfInvoicesMasterGet());
            NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInSelfInvoicesGrid);
            SetSelfInvoicesList();

            AddSelfInvoice = new RelayCommand(AddSelfInvoice_Executed);
            AddToCashFlowCommand = new RelayCommand(AddToCashFlowCommand_Executed);

            ModifySelfInvoiceCommand = new RelayCommand(ModifySelfInvoiceCommand_Executed);
            UpdateSelfInvoiceCloseCommand = new RelayCommand(UpdateSelfInvoiceCommandClose_Executed);
            UpdateSelfInvoiceCommand = new RelayCommand(UpdateSelfInvoiceCommand_Executed);
            RemoveSelfInvoiceCommand = new RelayCommand(RemoveSelfInvoiceCommand_Executed);
            
            InvoiceDate = DateTime.Now;
            InvoiceYear = DateTime.Now.Year;
            VatExempt = true;
            Materials = new List<Material>(_contoData.MaterialsGet());
            ExistingMaterials = new List<Material>(Materials);
            Measures = new List<Measures>(_contoData.MeasuresGet());
            ExistingMeasures = new List<Measures>(_contoData.MeasuresGet());

            UpdatePanelVisibility = Visibility.Collapsed;
        }

        private void SetSelfInvoicesList(int pageIndex = 0)
        {
            PageIndex = pageIndex + 1;
            SelfInvoices = _completeList.Skip(pageIndex * NumberOfRowsInSelfInvoicesGrid).Take(NumberOfRowsInSelfInvoicesGrid).ToList();
        }

        private long _id;
        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private Material _selectedMaterial;
        public Material SelectedMaterial
        {
            get { return _selectedMaterial; }
            set
            {
                _selectedMaterial = value;
                if (value != null)
                {
                    MaterialPrice = value.Price;
                    MaterialPriceMessage = string.Format("Prezzo materiale {0}€ a {1}", value.Price, value.MeasureDescription);
                }
                OnPropertyChanged("SelectedMaterial");
            }
        }

        private List<Material> _materials;
        public List<Material> Materials
        {
            get
            {
                return _materials;
            }
            set
            {
                _materials = value;
                OnPropertyChanged("Materials");
            }
        }

        private decimal? _quantity;
        public decimal? Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
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

        private int _invoiceNumber;
        public int InvoiceNumber
        {
            get { return _invoiceNumber; }
            set
            {
                _invoiceNumber = value;
                OnPropertyChanged("InvoiceNumber");
            }
        }

        private int _invoiceYear;
        public int InvoiceYear
        {
            get { return _invoiceYear; }
            set
            {
                _invoiceYear = value;
                OnPropertyChanged("InvoiceYear");
            }
        }

        private Measures _selectedMeasure;
        public Measures SelectedMeasure
        {
            get { return _selectedMeasure; }
            set
            {
                _selectedMeasure = value;
                OnPropertyChanged("SelectedMeasure");
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
                OnPropertyChanged("Measures");
            }
        }

        private string _materialPriceMessage;
        public string MaterialPriceMessage
        {
            get { return _materialPriceMessage; }
            set
            {
                _materialPriceMessage = value;
                OnPropertyChanged("MaterialPriceMessage");
            }
        }

        private decimal? _materialPrice;
        public decimal? MaterialPrice
        {
            get
            {
                return _materialPrice;
            }
            set
            {
                _materialPrice = value;
                OnPropertyChanged("MaterialPrice");
            }
        }

        private bool _inCashFlow;
        public bool InCashFlow
        {
            get
            {
                return _inCashFlow;
            }
            set
            {
                _inCashFlow = value;
                OnPropertyChanged("InCashFlow");
            }
        }

        private DateTime _invoiceDate;
        public DateTime InvoiceDate
        {
            get
            {
                return _invoiceDate;
            }
            set
            {
                _invoiceDate = value;
                OnPropertyChanged("InvoiceDate");
            }
        }
        
        #region SELFINVOICES GRID PROPERTIES

        private List<SelfInvoicesMaster> _completeList;

        private List<SelfInvoicesMaster> _selfInvoices;
        public List<SelfInvoicesMaster> SelfInvoices
        {
            get
            {
                return _selfInvoices;
            }
            set
            {
                _selfInvoices = value;
                OnPropertyChanged("SelfInvoices", false);
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

        #region UPDATE SELFINVOICE PROPERTIES

        private Guid _existingId;
        public Guid ExistingId
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

        private long _existingSelectedMaterialIndex;
        public long ExistingSelectedMaterialIndex
        {
            get { return _existingSelectedMaterialIndex; }
            set
            {
                _existingSelectedMaterialIndex = value;
                OnPropertyChanged("ExistingSelectedMaterialIndex", false);
            }
        }

        private List<Material> _existingMaterials;
        public List<Material> ExistingMaterials
        {
            get
            {
                return _existingMaterials;
            }
            set
            {
                _existingMaterials = value;
                OnPropertyChanged("ExistingMaterials", false);
            }
        }

        private decimal? _existingQuantity;
        public decimal? ExistingQuantity
        {
            get
            {
                return _existingQuantity;
            }
            set
            {
                _existingQuantity = value;
                OnPropertyChanged("ExistingQuantity", false);
            }
        }

        private bool _existingVatExempt;
        public bool ExistingVatExempt
        {
            get
            {
                return _existingVatExempt;
            }
            set
            {
                _existingVatExempt = value;
                OnPropertyChanged("ExistingVatExempt", false);
            }
        }

        private int _existingInvoiceYear;
        public int ExistingInvoiceYear
        {
            get { return _existingInvoiceYear; }
            set
            {
                _existingInvoiceYear = value;
                OnPropertyChanged("ExistingInvoiceYear", false);
            }
        }

        private int _existingSelectedMeasureIndex;
        public int ExistingSelectedMeasureIndex
        {
            get { return _existingSelectedMeasureIndex; }
            set
            {
                _existingSelectedMeasureIndex = value;
                OnPropertyChanged("ExistingSelectedMeasureIndex", false);
            }
        }

        private List<Measures> _existingMeasures;
        public List<Measures> ExistingMeasures
        {
            get
            {
                return _existingMeasures;
            }
            set
            {
                _existingMeasures = value;
                OnPropertyChanged("ExistingMeasures", false);
            }
        }

        private DateTime _existingInvoiceDate;
        public DateTime ExistingInvoiceDate
        {
            get
            {
                return _existingInvoiceDate;
            }
            set
            {
                _existingInvoiceDate = value;
                OnPropertyChanged("ExistingInvoiceDate", false);
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

        public ICommand AddSelfInvoice { get; set; }
        public void AddSelfInvoice_Executed(object sender)
        {
            if (AppProperties.FormHaveModifications)
            {
                if (SelectedMaterial == null || !SelectedMaterial.Price.HasValue)
                {
                    MessageBox.Show("Selezionare un materiale");
                    return;
                }
                if (!Quantity.HasValue)
                {
                    MessageBox.Show("Selezionare una quantità");
                    return;
                }
                if (SelectedMeasure == null)
                {
                    MessageBox.Show("Selezionare una misura");
                    return;
                }
                if (!MaterialPrice.HasValue)
                {
                    MessageBox.Show("Selezionare un costo");
                    return;
                }

                //var invoiceNumber = 1;
                var invoiceGroup = Guid.NewGuid();
                var settings = _contoData.GetSettings();
                var maxInvoiceValue = settings != null ? settings.MaxInvoiceValue : 990;
                maxInvoiceValue = maxInvoiceValue.HasValue ? maxInvoiceValue.Value : 990;
                var measure = _contoData.MeasureGet(SelectedMaterial.MeasureId);

                decimal invoiceCost = (Quantity.Value*SelectedMaterial.Price.Value)/measure.Grams*SelectedMeasure.Grams;
                //decimal quantityAtMaxInvoiceValue = (measure.Grams / SelectedMeasure.Grams);
                decimal quantityAtMaxInvoiceValue = maxInvoiceValue.Value*(Quantity.Value*SelectedMeasure.Grams)/
                                                    invoiceCost/
                                                    SelectedMeasure.Grams;

                var invoices = (int)Math.Truncate(invoiceCost / maxInvoiceValue.Value) + (invoiceCost % maxInvoiceValue.Value == 0 ? 0 : 1);
                
                var quantityTot = Quantity.Value;

                for (int i = 0; i < invoices; i++)
                {
                    var invoiceCurrentCost = invoiceCost > maxInvoiceValue ? maxInvoiceValue.Value : invoiceCost;
                    invoiceCost = invoiceCost - maxInvoiceValue.Value;
                    var quantity = quantityTot > quantityAtMaxInvoiceValue ? quantityAtMaxInvoiceValue : quantityTot;
                    quantityTot = quantityTot - quantityAtMaxInvoiceValue;
                    _contoData.SelfInvoicesAdd(new SelfInvoices
                    {
                        MaterialId = SelectedMaterial.Id,
                        Quantity = quantity,
                        VatExcept = VatExempt,
                        //InvoiceNumber = invoiceNumber++,
                        InvoiceYear = InvoiceYear,
                        MeasureId = SelectedMeasure.Id,
                        InCashFlow = false,
                        InvoiceDate = InvoiceDate,
                        InvoiceCost = invoiceCurrentCost,
                        InvoiceGroupId = invoiceGroup
                    });
                }

                SelectedMaterial = null;
                MaterialPrice = null;
                SelectedMeasure = null;
                Quantity = null;

                _completeList = new List<SelfInvoicesMaster>(_contoData.SelfInvoicesMasterGet());
                NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInSelfInvoicesGrid);
                SetSelfInvoicesList();

                AppProperties.FormHaveModifications = false;
            }
            else
            {
                MessageBox.Show("AddSelfInvoice");
            }
        }

        public ICommand AddToCashFlowCommand { get; set; }
        public void AddToCashFlowCommand_Executed(object sender)
        {
            _contoData.SelfInvoiceAddToCashFlow((SelfInvoicesMaster)sender);
            _completeList = new List<SelfInvoicesMaster>(_contoData.SelfInvoicesMasterGet());
            NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInSelfInvoicesGrid);
            SetSelfInvoicesList();
            AppProperties.FormHaveModifications = false;
        }

        public ICommand RemoveSelfInvoiceCommand { get; set; }
        public void RemoveSelfInvoiceCommand_Executed(object sender)
        {

            if (_contoData.SelfInvoiceDelete((SelfInvoicesMaster)sender))
            {
                _completeList = new List<SelfInvoicesMaster>(_contoData.SelfInvoicesMasterGet());
                NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInSelfInvoicesGrid);
                SetSelfInvoicesList();
                AppProperties.FormHaveModifications = false;
            }
            else
            {
                MessageBox.Show("Impossibile eliminare l'autofattura perchè in uso nel conto!");
            }
        }

        public ICommand ModifySelfInvoiceCommand { get; set; }
        public void ModifySelfInvoiceCommand_Executed(object sender)
        {
            var selfInvoice = (SelfInvoicesMaster)sender;
            ExistingId = selfInvoice.InvoiceGroupId;
            ExistingQuantity = selfInvoice.Quantity;
            ExistingSelectedMaterialIndex = ExistingMaterials.FindIndex(m => m.Id == selfInvoice.MaterialId);
            ExistingSelectedMeasureIndex = ExistingMeasures.FindIndex(m => m.Id == selfInvoice.MeasureId);
            ExistingVatExempt = selfInvoice.VatExcept;
            ExistingInvoiceYear = selfInvoice.InvoiceYear;
            ExistingInvoiceDate = selfInvoice.InvoiceDate;

            UpdatePanelVisibility = Visibility.Visible;
        }

        public ICommand UpdateSelfInvoiceCloseCommand { get; set; }
        public void UpdateSelfInvoiceCommandClose_Executed(object sender)
        {
            UpdatePanelVisibility = Visibility.Collapsed;
        }

        public ICommand UpdateSelfInvoiceCommand { get; set; }
        public void UpdateSelfInvoiceCommand_Executed(object sender)
        {
            //_contoData.ClientUpdate(new SelfInvoices
            //{
            //    Id = ExistingId,
            //    Name = ExistingName,
            //    Address = ExistingAddress,
            //    City = ExistingCity,
            //    PostalCode = ExistingPostalCode,
            //    FiscalCode = ExistingFiscalCode,
            //    VatCode = ExistingVatCode
            //});

            _completeList = new List<SelfInvoicesMaster>(_contoData.SelfInvoicesMasterGet());
            SetSelfInvoicesList();

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
            SetSelfInvoicesList();
        }

        private ICommand _previousPageCommand;
        public ICommand PreviousPage
        {
            get { return _previousPageCommand ?? (_previousPageCommand = new RelayCommand(Previous_Page)); }
        }

        public void Previous_Page(object sender)
        {
            if (PageIndex > 1)
                SetSelfInvoicesList(PageIndex - 2);
        }

        private ICommand _nextPageCommand;
        public ICommand NextPage
        {
            get { return _nextPageCommand ?? (_nextPageCommand = new RelayCommand(Next_Page)); }
        }

        public void Next_Page(object sender)
        {
            if (PageIndex < NumberOfPages)
                SetSelfInvoicesList(PageIndex);
        }

        private ICommand _lastPageCommand;
        public ICommand LastPage
        {
            get { return _lastPageCommand ?? (_lastPageCommand = new RelayCommand(Last_Page)); }
        }

        public void Last_Page(object sender)
        {
            SetSelfInvoicesList(NumberOfPages - 1);
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
