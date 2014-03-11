using System.Windows;
using System.Windows.Input;


namespace Conto.Wpf
{
    /// <summary>
    /// Logica di interazione per ContoWindow.xaml
    /// </summary>
    public partial class ContoWindow : Window
    {
        public ContoWindow()
        {
            SettingsConfirmCommand = new RelayCommand(SettingsConfirmCommand_Executed);

            InitializeComponent();

            OptionsUserControl.SettingsButton.MouseUp += SettingsButtonOnMouseUp;
        }

        private void SettingsButtonOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            SettingsGrid.Visibility = SettingsGrid.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public static ICommand SettingsConfirmCommand { get; set; }
        public void SettingsConfirmCommand_Executed(object sender)
        {
            SettingsGrid.Visibility = Visibility.Collapsed;
        }

    }
}
