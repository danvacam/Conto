using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Conto.Wpf.Customer;

namespace Conto.Wpf
{
    /// <summary>
    /// Interaction logic for CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        public CustomersPage()
        {
            InitializeComponent();

            // handle errors from the data provider
            CustomerObjectDataProvider.PersistenceError += customerProvider_PersistenceError;
        }

        private void customerProvider_PersistenceError(CustomerObjectDataProvider dataProvider, Exception e)
        {
            // if an exception occurs - display a pop-up
            MessageBox.Show(e.GetType() + " " + e.Message);
        }
    }
}
