using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Conto.Wpf.ViewModels;

namespace Conto.Wpf.Converters
{
    public class CashFlowToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string) parameter == "CashFlowType")
            {
                if (((CashFlowGridRow) value).CashFlowType == "SelfInvoice")
                    return Visibility.Visible;
            }
            if ((string) parameter == "AddToPdf")
            {
                return ((CashFlowGridRow)value).AddToPdfButtonVisibility;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
