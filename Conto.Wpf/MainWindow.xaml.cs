using System.Windows;
using System.Windows.Input;
using Conto.Wpf.ViewModels;


namespace Conto.Wpf
{
    /// <summary>
    /// Logica di interazione per ContoWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            SettingsConfirmCommand = new RelayCommand(SettingsConfirmCommand_Executed);

            InitializeComponent();
        }

        public static ICommand SettingsConfirmCommand { get; set; }
        public void SettingsConfirmCommand_Executed(object sender)
        {
            SettingsGrid.Visibility = Visibility.Collapsed;
            var model = (MainViewModel)sender;
            new Data.ContoData().SetSettings(new Data.Settings
            {
                InvoiceOwnerAddress = model.InvoiceOwnerAddress,
                InvoiceOwnerCity = model.InvoiceOwnerCity,
                InvoiceOwnerFiscalCode = model.InvoiceOwnerFiscalCode,
                InvoiceOwnerName = model.InvoiceOwnerName,
                InvoiceOwnerPostalCode = model.InvoiceOwnerPostalCode,
                InvoiceOwnerVatCode = model.InvoiceOwnerVatCode,
                MaxInvoiceValue = model.InvoiceMaxCost
            });
        }

        private void SettingsButton_Click(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            SettingsGrid.Visibility = SettingsGrid.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }


        private void Window_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                    Application.Current.MainWindow.DragMove();
        }

        
        /// <summary>
        /// CloseButton_Clicked
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// MaximizedButton_Clicked
        /// </summary>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        /// <summary>
        /// Minimized Button_Clicked
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Adjusts the WindowSize to correct parameters when Maximize button is clicked
        /// </summary>
        private void AdjustWindowSize()
        {
            Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}
