﻿using System;
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

namespace Conto.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public ICommand ConfirmSettingsCommand
        {
            get { return (ICommand)GetValue(ConfirmSettingsCommandProperty); }
            set { SetValue(ConfirmSettingsCommandProperty, value); }
        }

        public static readonly DependencyProperty ConfirmSettingsCommandProperty =
            DependencyProperty.Register("ConfirmSettingsCommand", typeof(ICommand), typeof(SettingsView),
                new PropertyMetadata(default(ICommand)));

        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
