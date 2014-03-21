using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Conto.Data;

namespace Conto.Wpf
{
    public class CashFlowToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string) parameter == "CashFlowType")
            {
                if (((CashFlow) value).CashFlowType == "SelfInvoice")
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
