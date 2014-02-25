using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conto.Data;

namespace Conto.Wpf.Customer
{
    public delegate void PersistenceErrorHandler(CustomerObjectDataProvider dataProvider, Exception e);

    public class CustomerObjectDataProvider
    {
        private readonly ContoData _dataAccessLayer;
        public static event PersistenceErrorHandler PersistenceError;

        public CustomerObjectDataProvider()
        {
            _dataAccessLayer = new ContoData();
        }

        public CustomerUIObjects GetCustomers()
        {
            CustomerUIObjects customers = new CustomerUIObjects();

            List<CustomerDataObject> customerDataObjects = _dataAccessLayer.GetCustomers();
            foreach (var customerDataObject in customerDataObjects)
            {
                customers.Add(new CustomerUIObject(customerDataObject));
            }

            customers.ItemEndEdit += CustomersOnItemEndEdit;
            customers.CollectionChanged += new NotifyCollectionChangedEventHandler(CustomersCollectionChanged);

            return customers;
        }

        private void CustomersOnItemEndEdit(IEditableObject sender)
        {
            CustomerUIObject customerObject = sender as CustomerUIObject;

            try
            {
                // use the data access layer to update the wrapped data object
                _dataAccessLayer.UpdateCustomer(customerObject.GetDataObject());
            }
            catch (Exception ex)
            {
                if (PersistenceError != null)
                {
                    PersistenceError(this, ex);
                }
            }
        }

        private void CustomersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    CustomerUIObject customerObject = item as CustomerUIObject;

                    // use the data access layer to delete the wrapped data object
                    _dataAccessLayer.DeleteCustomer(customerObject.GetDataObject());
                }
            }

        }
    }
}
