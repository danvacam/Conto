using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Conto.Wpf.Resources
{
    public static class AppProperties
    {
        public static bool FormHaveModifications
        {
            get
            {
                if (Application.Current.Properties["FormHaveModifications"] == null)
                    Application.Current.Properties["FormHaveModifications"] = false;
                return (bool)Application.Current.Properties["FormHaveModifications"];
            }
            set { Application.Current.Properties["FormHaveModifications"] = value; }
        }
    }
}
