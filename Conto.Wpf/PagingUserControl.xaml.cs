using System;
using System.Windows;
using System.Windows.Input;

namespace Conto.Wpf
{
    public partial class PagingUserControl
    {
        public PagingUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex",
            typeof (int), typeof (PagingUserControl));

        public static readonly DependencyProperty NumberOfPagesProperty = DependencyProperty.Register("NumberOfPages",
            typeof(int), typeof(PagingUserControl));

        public int PageIndex
        {
            get { return Int32.Parse(GetValue(PageIndexProperty).ToString()); }
            set { SetValue(PageIndexProperty, value); }
        }

        public int NumberOfPages
        {
            get { return Int32.Parse(GetValue(NumberOfPagesProperty).ToString()); }
            set { SetValue(NumberOfPagesProperty, value); }
        }

        #region EVENTS

        public ICommand FirstPageCommand
        {
            get { return (ICommand) GetValue(FirstPageCommandProperty); }
            set { SetValue(FirstPageCommandProperty, value); }
        }

        public static readonly DependencyProperty FirstPageCommandProperty =
            DependencyProperty.Register("FirstPageCommand", typeof (ICommand), typeof (PagingUserControl),
                new PropertyMetadata(default(ICommand)));

        public ICommand PreviousPageCommand
        {
            get { return (ICommand)GetValue(PreviousPageCommandProperty); }
            set { SetValue(PreviousPageCommandProperty, value); }
        }

        public static readonly DependencyProperty PreviousPageCommandProperty =
            DependencyProperty.Register("PreviousPageCommand", typeof(ICommand), typeof(PagingUserControl),
                new PropertyMetadata(default(ICommand)));

        public ICommand NextPageCommand
        {
            get { return (ICommand)GetValue(NextPageCommandProperty); }
            set { SetValue(NextPageCommandProperty, value); }
        }

        public static readonly DependencyProperty NextPageCommandProperty =
            DependencyProperty.Register("NextPageCommand", typeof(ICommand), typeof(PagingUserControl),
                new PropertyMetadata(default(ICommand)));

        public ICommand LastPageCommand
        {
            get { return (ICommand)GetValue(LastPageCommandProperty); }
            set { SetValue(LastPageCommandProperty, value); }
        }

        public static readonly DependencyProperty LastPageCommandProperty =
            DependencyProperty.Register("LastPageCommand", typeof(ICommand), typeof(PagingUserControl),
                new PropertyMetadata(default(ICommand)));

        #endregion
    }
}
