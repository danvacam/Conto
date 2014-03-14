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
        public MaterialsViewModel()
        {
            var contoData = new ContoData();
            Materials = new List<Material>(contoData.MaterialsGet());
            Measures = new List<Measures>(contoData.MeasuresGet());
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
                MessageBox.Show("AddMaterial MODIFIED");
            }
            else
            {
                MessageBox.Show("AddMaterial");
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
