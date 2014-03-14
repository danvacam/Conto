using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Conto.Data;
using Conto.Wpf.Resources;

namespace Conto.Wpf.ViewModels
{
    public class MaterialsViewModel : INotifyPropertyChanged
    {
        private readonly ContoData _contoData;
        public MaterialsViewModel()
        {
            _contoData = new ContoData();
            Materials = new List<Material>(_contoData.MaterialsGet());
            Measures = new List<Measures>(_contoData.MeasuresGet());
            AddMaterialCommand = new RelayCommand(AddMaterial_Executed);
        }

        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private decimal? _price;
        public decimal? Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        private int? _selectedMeasure;

        public int? SelectedMeasure
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

        public ICommand AddMaterialCommand { get; set; }
        public void AddMaterial_Executed(object sender)
        {
            if (AppProperties.FormHaveModifications)
            {
                if (string.IsNullOrEmpty(Description))
                {
                    MessageBox.Show("Manca la descrizione");
                    return;
                }
                if (!Price.HasValue)
                {
                    MessageBox.Show("Manca il prezzo");
                    return;
                }
                if (!SelectedMeasure.HasValue)
                {
                    MessageBox.Show("Nessuna unità di misura selezionata");
                    return;
                }
                _contoData.MaterialAdd(new Material
                {
                    Description = Description,
                    Price = Price,
                    MeasureId = SelectedMeasure.Value
                });
                Materials = new List<Material>(_contoData.MaterialsGet());
                
                Description = string.Empty;
                Price = null;
                SelectedMeasure = null;
                AppProperties.FormHaveModifications = false;
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
