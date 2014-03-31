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
            ExistingMeasures = new List<Measures>(_contoData.MeasuresGet());

            _completeList = new List<Material>(_contoData.MaterialsGet());
            NumberOfPages =  (int) Math.Ceiling((double) _completeList.Count / NumberOfRowsInMaterialsGrid);
            SetMaterialsList();

            AddMaterialCommand = new RelayCommand(AddMaterial_Executed);
            RemoveMaterialCommand = new RelayCommand(RemoveMaterialCommand_Executed);
            ModifyMaterialCommand = new RelayCommand(ModifyMaterialCommand_Executed);
            UpdateMaterialCommand = new RelayCommand(UpdateMaterialCommand_Executed);

            UpdatePanelVisibility = Visibility.Collapsed;
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

        private List<Material> _completeList;

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
                OnPropertyChanged("Materials", false);
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

        #region UPDATE MATERIAL PROPERTIES

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

        private string _existingDescription;
        public string ExistingDescription
        {
            get
            {
                return _existingDescription;
            }
            set
            {
                _existingDescription = value;
                OnPropertyChanged("ExistingDescription", false);
            }
        }

        private decimal? _existingPrice;
        public decimal? ExistingPrice
        {
            get
            {
                return _existingPrice;
            }
            set
            {
                _existingPrice = value;
                OnPropertyChanged("ExistingPrice", false);
            }
        }

        private int _existingSelectedMeasure;
        public int ExistingSelectedMeasure
        {
            get { return _existingSelectedMeasure; }
            set
            {
                _existingSelectedMeasure = value;
                OnPropertyChanged("ExistingSelectedMeasure", false);
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

                _completeList = new List<Material>(_contoData.MaterialsGet());
                NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInMaterialsGrid);
                SetMaterialsList();
                
                Description = string.Empty;
                Price = null;
                SelectedMeasure = null;
                AppProperties.FormHaveModifications = false;
            }
        }

        public ICommand RemoveMaterialCommand { get; set; }
        public void RemoveMaterialCommand_Executed(object sender)
        {

            if (_contoData.MaterialDelete((Material) sender))
            {
                _completeList = new List<Material>(_contoData.MaterialsGet());
                NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / NumberOfRowsInMaterialsGrid);
                SetMaterialsList();
                AppProperties.FormHaveModifications = false;
            }
            else
            {
                MessageBox.Show("Impossibile eliminare il materiale perchè in uso nelle fatture!");
            }
        }

        public ICommand ModifyMaterialCommand { get; set; }
        public void ModifyMaterialCommand_Executed(object sender)
        {
            var material = (Material) sender;
            ExistingId = material.Id;
            ExistingDescription = material.Description;
            ExistingSelectedMeasure = material.MeasureId;
            ExistingPrice = material.Price;

            UpdatePanelVisibility = Visibility.Visible;
        }

        public ICommand UpdateMaterialCommand { get; set; }
        public void UpdateMaterialCommand_Executed(object sender)
        {
            _contoData.MaterialUpdate(new Material
            {
                Id = ExistingId,
                Description = ExistingDescription,
                MeasureId = ExistingSelectedMeasure,
                Price = ExistingPrice
            });

            _completeList = new List<Material>(_contoData.MaterialsGet());
            SetMaterialsList();

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
