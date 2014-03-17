using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Conto.Data;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class SelfInvoiceViewModel : INotifyPropertyChanged
    {
        readonly ContoData _contoData;

        public SelfInvoiceViewModel()
        {
            _contoData = new ContoData();

            AddSelfInvoice = new RelayCommand(AddSelfInvoice_Executed);

            InvoiceDate = DateTime.Now;
            InvoiceYear = DateTime.Now.Year;
            VatExempt = true;
            Materials = new List<Material>(_contoData.MaterialsGet());
            Measures = new List<Measures>(_contoData.MeasuresGet());

            SelfInvoicesMaster = new List<SelfInvoicesMaster>(_contoData.SelfInvoicesMasterGet());

            SelfInvoices = new List<SelfInvoices>(_contoData.SelfInvoicesGet());
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
                MaterialPrice = value.Price;
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

        private List<SelfInvoices> _selfInvoices;
        public List<SelfInvoices> SelfInvoices
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

        private List<SelfInvoicesMaster> _selfInvoicesMaster;
        public List<SelfInvoicesMaster> SelfInvoicesMaster
        {
            get
            {
                return _selfInvoicesMaster;
            }
            set
            {
                _selfInvoicesMaster = value;
                OnPropertyChanged("SelfInvoicesMaster");
            }
        }

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

                var invoiceNumber = 1;
                var invoiceGroup = Guid.NewGuid();
                var settings = _contoData.GetSettings();
                var maxInvoiceValue = settings != null ? settings.MaxInvoiceValue : 999;
                maxInvoiceValue = maxInvoiceValue.HasValue ? maxInvoiceValue.Value : 990;
                var measure = _contoData.MeasureGet(SelectedMaterial.MeasureId);

                decimal invoiceCost = Quantity.Value*SelectedMaterial.Price.Value;
                var quantityAtMaxInvoiceValue = (maxInvoiceValue.Value*measure.Grams/SelectedMaterial.Price.Value) / measure.Grams;



                // TODO change measures
                //if (SelectedMeasure.Id == SelectedMaterial.MeasureId)
                //{
                //    invoiceCost = Quantity.Value * SelectedMaterial.Price.Value;
                //}
                //else
                //{
                //    invoiceCost = 0;
                //}

                //var invoiceCost = Quantity.Value*MaterialPrice.Value;
                //var wightForMaxInvoiceValue = maxInvoiceValue.Value 

                var invoices = (int)Math.Truncate(invoiceCost / maxInvoiceValue.Value);
                
                var quantityTot = Quantity.Value;

                for (int i = 0; i < invoices; i++)
                {
                    var value = Math.Abs(invoiceCost - maxInvoiceValue.Value);
                    var invoiceCurrentCost = value > maxInvoiceValue ? maxInvoiceValue : value;
                    var quantity = Math.Abs(quantityTot - quantityAtMaxInvoiceValue);
                    quantity = quantity > quantityAtMaxInvoiceValue ? quantityAtMaxInvoiceValue : quantity;
                    _contoData.SelfInvoicesAdd(new SelfInvoices
                    {
                        MaterialId = SelectedMaterial.Id,
                        Quantity = quantity,
                        VatExcept = VatExempt,
                        InvoiceNumber = invoiceNumber++,
                        InvoiceYear = InvoiceYear,
                        MeasureId = SelectedMeasure.Id,
                        InCashFlow = false,
                        InvoiceDate = InvoiceDate,
                        InvoiceCost = invoiceCurrentCost.Value,
                        InvoiceGroupId = invoiceGroup
                    });
                }
            }
            else
            {
                MessageBox.Show("AddSelfInvoice");
            }
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
