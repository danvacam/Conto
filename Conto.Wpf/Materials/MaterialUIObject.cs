using System.ComponentModel;
using System.Runtime.CompilerServices;
using Conto.Data;
using Conto.Wpf.Customer;

namespace Conto.Wpf.Materials
{
    public class MaterialUIObject : IEditableObject, INotifyPropertyChanged
    {
        private readonly MaterialDataObject _materialDataObject;

        public MaterialUIObject()
        {
            _materialDataObject = new MaterialDataObject();
        }

        public MaterialUIObject(MaterialDataObject materialDataObject)
        {
            _materialDataObject = materialDataObject;
        }

        public MaterialDataObject GetDataObject()
        {
            return _materialDataObject;
        }

        public int Id
        {
            get { return _materialDataObject.Id; }
            set
            {
                _materialDataObject.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return _materialDataObject.Name; }
            set
            {
                _materialDataObject.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public decimal Price
        {
            get { return _materialDataObject.Price; }
            set
            {
                _materialDataObject.Price = value;
                OnPropertyChanged("Price");
            }
        }
        
        public event ItemEndEditEventHandler ItemEndEdit;

        public void BeginEdit()
        {
        }

        public void EndEdit()
        {
            if (ItemEndEdit != null)
            {
                ItemEndEdit(this);
            }
        }

        public void CancelEdit()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
