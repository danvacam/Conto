using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conto.Data;
using Conto.Wpf.Customer;

namespace Conto.Wpf.Materials
{
    public delegate void PersistenceErrorHandler(MaterialsObjectDataProvider dataProvider, Exception e);

    public class MaterialsObjectDataProvider
    {
        private readonly ContoData _dataAccessLayer;
        public static event PersistenceErrorHandler PersistenceError;

        public MaterialsObjectDataProvider()
        {
            _dataAccessLayer = new ContoData();
        }

        public MaterialUIObjects GetMaterials()
        {
            var materials = new MaterialUIObjects();
            //List<MaterialDataObject> materialData = _dataAccessLayer.GetMaterials();
            //foreach (var materialDataObject in materialData)
            //{
            //    materials.Add(new MaterialUIObject(materialDataObject));
            //}

            //materials.ItemEndEdit += MaterialsOnItemEndEdit;
            //materials.CollectionChanged += MaterialsOnCollectionChanged;

            return materials;
        }

        private void MaterialsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    var materialObject = item as MaterialUIObject;

                    // use the data access layer to delete the wrapped data object
                    //_dataAccessLayer.DeleteMaterial(materialObject.GetDataObject());
                }
            }
        }

        private void MaterialsOnItemEndEdit(IEditableObject sender)
        {
            var materialObject = sender as MaterialUIObject;

            try
            {
                // use the data access layer to update the wrapped data object
                //_dataAccessLayer.UpdateMaterial(materialObject.GetDataObject());
            }
            catch (Exception ex)
            {
                if (PersistenceError != null)
                {
                    PersistenceError(this, ex);
                }
            }
        }
    }
}
