using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace Conto.Wpf.GridPaging
{
    public class GridPaging<T> where T : class
    {
        internal Action<string, bool> PropertyChangeAction;
        private Func<List<T>> _gridDataSource;
        internal readonly int NumberOfRowsInGrid;

        public GridPaging(int numberOfRowsInGrid)
        {
            NumberOfRowsInGrid = numberOfRowsInGrid;
        }

        internal void Initialize(Action<string, bool> propertyChangeAction, Func<List<T>> gridDataSource)
        {
            PropertyChangeAction = propertyChangeAction;
            _gridDataSource = gridDataSource;
            UpdateList();
            GridSourceHaveRecords = GridSource.Count > 0;
        }

        internal List<T> CompleteList;

        private bool _gridSourceHaveRecords;
        public bool GridSourceHaveRecords
        {
            get { return _gridSourceHaveRecords; }
            set
            {
                _gridSourceHaveRecords = value;
                PropertyChangeAction("GridSourceHaveRecords", false);
            }
        }

        private List<T> _gridSource;
        public List<T> GridSource
        {
            get
            {
                return _gridSource;
            }
            set
            {
                _gridSource = value;
                PropertyChangeAction("GridSource", false);
            }
        }

        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
                PropertyChangeAction("PageIndex", false);
            }
        }

        private int _numberOfPages;
        public int NumberOfPages
        {
            get
            {
                return _numberOfPages;
            }
            set
            {
                _numberOfPages = value;
                PropertyChangeAction("NumberOfPages", false);
            }
        }



        public void UpdateList()
        {
            CompleteList = new List<T>(_gridDataSource());
            NumberOfPages = (int)Math.Ceiling((double)CompleteList.Count / NumberOfRowsInGrid);
            SetList();
        }


        internal void SetList(int pageIndex = 0)
        {
            PageIndex = pageIndex + 1;
            GridSource = CompleteList.Skip(pageIndex * NumberOfRowsInGrid).Take(NumberOfRowsInGrid).ToList();
        }




        private ICommand _firstPageCommand;
        public ICommand FirstPage
        {
            get { return _firstPageCommand ?? (_firstPageCommand = new RelayCommand(First_Page)); }
        }

        public void First_Page(object sender)
        {
            SetList();
        }

        private ICommand _previousPageCommand;
        public ICommand PreviousPage
        {
            get { return _previousPageCommand ?? (_previousPageCommand = new RelayCommand(Previous_Page)); }
        }

        public void Previous_Page(object sender)
        {
            if (PageIndex > 1)
                SetList(PageIndex - 2);
        }

        private ICommand _nextPageCommand;
        public ICommand NextPage
        {
            get { return _nextPageCommand ?? (_nextPageCommand = new RelayCommand(Next_Page)); }
        }

        public void Next_Page(object sender)
        {
            if (PageIndex < NumberOfPages)
                SetList(PageIndex);
        }

        private ICommand _lastPageCommand;
        public ICommand LastPage
        {
            get { return _lastPageCommand ?? (_lastPageCommand = new RelayCommand(Last_Page)); }
        }

        public void Last_Page(object sender)
        {
            SetList(NumberOfPages - 1);
        }

    }

    public class GridPaging<T, TIn1, TIn2> : GridPaging<T> where T : class
    {
        private Func<TIn1, TIn2, List<T>> _gridDataSource;

        public GridPaging(int numberOfRowsInGrid)
            :base (numberOfRowsInGrid)
        {
        }

        internal void Initialize(Action<string, bool> propertyChangeAction, Func<TIn1, TIn2, List<T>> gridDataSource, TIn1 gridDataSourceInputParameter1, TIn2 gridDataSourceInputParameter2)
        {
            PropertyChangeAction = propertyChangeAction;
            _gridDataSource = gridDataSource;
            UpdateList(gridDataSourceInputParameter1, gridDataSourceInputParameter2);
            GridSourceHaveRecords = GridSource.Count > 0;
        }

        public void UpdateList(TIn1 gridDataSourceInputParameter1, TIn2 gridDataSourceInputParameter2)
        {
            CompleteList = new List<T>(_gridDataSource(gridDataSourceInputParameter1, gridDataSourceInputParameter2));
            NumberOfPages = (int)Math.Ceiling((double)CompleteList.Count / NumberOfRowsInGrid);
            SetList();
        }
    }
}
