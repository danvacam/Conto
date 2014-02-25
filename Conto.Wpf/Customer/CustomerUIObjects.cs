using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conto.Data;

namespace Conto.Wpf.Customer
{
    public class CustomerUIObjects : ObservableCollection<CustomerUIObject>
    {
        protected override void InsertItem(int index, CustomerUIObject item)
        {
            base.InsertItem(index, item);

            item.ItemEndEdit += ItemOnItemEndEdit;
        }

        private void ItemOnItemEndEdit(IEditableObject sender)
        {
            if (ItemEndEdit != null)
            {
                ItemEndEdit(sender);
            }
        }

        public event ItemEndEditEventHandler ItemEndEdit;
    }
}
