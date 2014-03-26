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
    public class MaterialsViewModel : INotifyPropertyChanged
    {
        private const int NumberOfRowsInMaterialsGrid = 10;
        private readonly ContoData _contoData;
        public MaterialsViewModel()
        {
            _contoData = new ContoData();
            Measures = new List<Measures>(_contoData.MeasuresGet());

            _completeList = new List<Material>(_contoData.MaterialsGet());
            NumberOfPages =  (int) Math.Ceiling((double) _completeList.Count / NumberOfRowsInMaterialsGrid);
            SetMaterialsList();

            AddMaterialCommand = new RelayCommand(AddMaterial_Executed);
        }

        private void SetMaterialsList(int pageIndex = 0)
        {
            PageIndex = pageIndex + 1;
            Materials = _completeList.Skip(pageIndex * NumberOfRowsInMaterialsGrid).Take(NumberOfRowsInMaterialsGrid).ToList();
        }

        #region NEW MATERIAL FORM PROPERTIES

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

        #endregion

        #region MATERIALS GRID PROPERTIES

        private readonly List<Material> _completeList;

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
                OnPropertyChanged("PageIndex");
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
                OnPropertyChanged("NumberOfPages");
            }
        }

        #endregion

        #region COMMANDS

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

        #region PAGING COMMANDS

        private ICommand _firstPageCommand;
        public ICommand FirstPage
        {
            get { return _firstPageCommand ?? (_firstPageCommand = new RelayCommand(First_Page)); }
        }

        public void First_Page(object sender)
        {
            SetMaterialsList();
        }

        private ICommand _previousPageCommand;
        public ICommand PreviousPage
        {
            get { return _previousPageCommand ?? (_previousPageCommand = new RelayCommand(Previous_Page)); }
        }

        public void Previous_Page(object sender)
        {
            if (PageIndex > 1)
                SetMaterialsList(PageIndex - 2);
        }

        private ICommand _nextPageCommand;
        public ICommand NextPage
        {
            get { return _nextPageCommand ?? (_nextPageCommand = new RelayCommand(Next_Page)); }
        }

        public void Next_Page(object sender)
        {
            if(PageIndex < NumberOfPages)
                SetMaterialsList(PageIndex);
        }

        private ICommand _lastPageCommand;
        public ICommand LastPage
        {
            get { return _lastPageCommand ?? (_lastPageCommand = new RelayCommand(Last_Page)); }
        }

        public void Last_Page(object sender)
        {
            SetMaterialsList(NumberOfPages - 1);
        }

        #endregion

        #endregion

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
