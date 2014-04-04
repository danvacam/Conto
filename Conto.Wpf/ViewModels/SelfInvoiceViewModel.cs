using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Conto.Data;
using Conto.Wpf.Resources;
using Conto.Wpf.GridPaging;

namespace Conto.Wpf.ViewModels
{
    public class SelfInvoiceViewModel : GridPaging<SelfInvoicesMaster>, INotifyPropertyChanged
    {
        private const int NumberOfRowsInSelfInvoicesGrid = 10;
        readonly ContoData _contoData;

        public SelfInvoiceViewModel()
            : base(NumberOfRowsInSelfInvoicesGrid)
        {
            _contoData = new ContoData();

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

            Initialize(OnPropertyChanged, _contoData.SelfInvoicesMasterGet);

            UpdatePanelVisibility = Visibility.Collapsed;
        }

        #region NEW SELFINVOICE PROPERTIES

        private long _id;
        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        private int _invoiceNumber;
        public int InvoiceNumber
        {
            get { return _invoiceNumber; }
            set
            {
                _invoiceNumber = value;
                OnPropertyChanged();
            }
        }

        private int _invoiceYear;
        public int InvoiceYear
        {
            get { return _invoiceYear; }
            set
            {
                _invoiceYear = value;
                OnPropertyChanged();
            }
        }

        private Measures _selectedMeasure;
        public Measures SelectedMeasure
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

        private string _materialPriceMessage;
        public string MaterialPriceMessage
        {
            get { return _materialPriceMessage; }
            set
            {
                _materialPriceMessage = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged(formHaveModifications: false);
            }
        }

        private long _existingSelectedMaterialIndex;
        public long ExistingSelectedMaterialIndex
        {
            get { return _existingSelectedMaterialIndex; }
            set
            {
                _existingSelectedMaterialIndex = value;
                OnPropertyChanged(formHaveModifications: false);
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
                OnPropertyChanged(formHaveModifications: false);
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
                OnPropertyChanged(formHaveModifications: false);
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
                OnPropertyChanged(formHaveModifications: false);
            }
        }

        private int _existingInvoiceYear;
        public int ExistingInvoiceYear
        {
            get { return _existingInvoiceYear; }
            set
            {
                _existingInvoiceYear = value;
                OnPropertyChanged(formHaveModifications: false);
            }
        }

        private int _existingSelectedMeasureIndex;
        public int ExistingSelectedMeasureIndex
        {
            get { return _existingSelectedMeasureIndex; }
            set
            {
                _existingSelectedMeasureIndex = value;
                OnPropertyChanged(formHaveModifications: false);
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
                OnPropertyChanged(formHaveModifications: false);
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
                OnPropertyChanged(formHaveModifications: false);
            }
        }

        private Visibility _updatePanelVisibility;
        public Visibility UpdatePanelVisibility
        {
            get { return _updatePanelVisibility; }
            set
            {
                _updatePanelVisibility = value;
                OnPropertyChanged(formHaveModifications: false);
            }
        }

        #endregion

        #region COMMANDS

        private ICommand _addSelfInvoice;
        public ICommand AddSelfInvoice
        {
            get { return _addSelfInvoice ?? (_addSelfInvoice = new RelayCommand(AddSelfInvoice_Executed)); }
        }
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

                decimal invoiceCost = (Quantity.Value * SelectedMaterial.Price.Value) / measure.Grams * SelectedMeasure.Grams;
                //decimal quantityAtMaxInvoiceValue = (measure.Grams / SelectedMeasure.Grams);
                decimal quantityAtMaxInvoiceValue = maxInvoiceValue.Value * (Quantity.Value * SelectedMeasure.Grams) /
                                                    invoiceCost /
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

                UpdateList();

                AppProperties.FormHaveModifications = false;
            }
            else
            {
                MessageBox.Show("AddSelfInvoice");
            }
        }

        private ICommand _addToCashFlowCommand;
        public ICommand AddToCashFlowCommand
        {
            get{ return _addToCashFlowCommand ?? (_addToCashFlowCommand = new RelayCommand(AddToCashFlowCommand_Executed)); }
        }
        public void AddToCashFlowCommand_Executed(object sender)
        {
            _contoData.SelfInvoiceAddToCashFlow((SelfInvoicesMaster)sender);
            UpdateList();
            AppProperties.FormHaveModifications = false;
        }

        public ICommand RemoveSelfInvoiceCommand { get; set; }
        public void RemoveSelfInvoiceCommand_Executed(object sender)
        {

            if (_contoData.SelfInvoiceDelete((SelfInvoicesMaster)sender))
            {
                UpdateList();
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

            UpdateList();

            UpdatePanelVisibility = Visibility.Collapsed;
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
