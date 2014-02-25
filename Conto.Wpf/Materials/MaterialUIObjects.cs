using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conto.Wpf.Customer;

namespace Conto.Wpf.Materials
{
    public class MaterialUIObjects : ObservableCollection<MaterialUIObject>
    {
        protected override void InsertItem(int index, MaterialUIObject item)
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
