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
    public class MaterialsViewModel : GridPaging<Material>, INotifyPropertyChanged
    {
        private const int NumberOfRowsInMaterialsGrid = 10;
        private readonly ContoData _contoData;
        public MaterialsViewModel()
            : base(NumberOfRowsInMaterialsGrid)
        {
            _contoData = new ContoData();
            Measures = new List<Measures>(_contoData.MeasuresGet());
            ExistingMeasures = new List<Measures>(_contoData.MeasuresGet());

            Initialize(OnPropertyChanged, _contoData.MaterialsGet);

            UpdatePanelVisibility = Visibility.Collapsed;
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
                OnPropertyChanged();
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
                OnPropertyChanged(formHaveModifications: false);
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
                OnPropertyChanged(formHaveModifications: false);
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
                OnPropertyChanged(formHaveModifications: false);
            }
        }

        private int _existingSelectedMeasure;
        public int ExistingSelectedMeasure
        {
            get { return _existingSelectedMeasure; }
            set
            {
                _existingSelectedMeasure = value;
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

        private ICommand _addMaterialCommand;
        public ICommand AddMaterialCommand
        {
            get { return _addMaterialCommand ?? (_addMaterialCommand = new RelayCommand(AddMaterial_Executed)); }
        }
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

                UpdateList();
                
                Description = string.Empty;
                Price = null;
                SelectedMeasure = null;
                AppProperties.FormHaveModifications = false;
            }
        }

        private ICommand _removeMaterialCommand;
        public ICommand RemoveMaterialCommand
        {
            get
            {
                return _removeMaterialCommand ??
                       (_removeMaterialCommand = new RelayCommand(RemoveMaterialCommand_Executed));
            }
        }
        public void RemoveMaterialCommand_Executed(object sender)
        {

            if (_contoData.MaterialDelete((Material) sender))
            {
                UpdateList();
                AppProperties.FormHaveModifications = false;
            }
            else
            {
                MessageBox.Show("Impossibile eliminare il materiale perchè in uso nelle fatture!");
            }
        }

        private ICommand _modifyMaterialCommand;
        public ICommand ModifyMaterialCommand
        {
            get
            {
                return _modifyMaterialCommand ??
                       (_modifyMaterialCommand = new RelayCommand(ModifyMaterialCommand_Executed));
            }
        }
        public void ModifyMaterialCommand_Executed(object sender)
        {
            var material = (Material) sender;
            ExistingId = material.Id;
            ExistingDescription = material.Description;
            ExistingSelectedMeasure = material.MeasureId;
            ExistingPrice = material.Price;

            UpdatePanelVisibility = Visibility.Visible;
        }

        private ICommand _updateMaterialCommand;
        public ICommand UpdateMaterialCommand
        {
            get
            {
                return _updateMaterialCommand ??
                       (_updateMaterialCommand = new RelayCommand(UpdateMaterialCommand_Executed));
            }
        }
        public void UpdateMaterialCommand_Executed(object sender)
        {
            _contoData.MaterialUpdate(new Material
            {
                Id = ExistingId,
                Description = ExistingDescription,
                MeasureId = ExistingSelectedMeasure,
                Price = ExistingPrice
            });

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
