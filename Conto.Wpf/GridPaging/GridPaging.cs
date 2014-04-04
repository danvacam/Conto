using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Conto.Wpf.GridPaging
{
    public interface IGridPaging<T> where T : class
    {
        List<T> GridSource { get; set; }
        int PageIndex { get; set; }
        int NumberOfPages { get; set; }
        ICommand FirstPage { get; }
        ICommand PreviousPage { get; }
        ICommand NextPage { get; }
        ICommand LastPage { get; }
        void First_Page(object sender);
        void Previous_Page(object sender);
        void Next_Page(object sender);
        void Last_Page(object sender);
    }

    public class GridPaging<T> : IGridPaging<T> where T : class
    {
        private Action<string, bool> _propertyChangeAction;
        private Func<List<T>> _gridDataSource;
        private readonly int _numberOfRowsInGrid;

        public GridPaging(int numberOfRowsInGrid)
        {
            _numberOfRowsInGrid = numberOfRowsInGrid;
        }

        internal void Initialize(Action<string, bool> propertyChangeAction, Func<List<T>> gridDataSource)
        {
            _propertyChangeAction = propertyChangeAction;
            _gridDataSource = gridDataSource;

            UpdateList();
        }


        private List<T> _completeList;

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
                _propertyChangeAction("GridSource", false);
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
                _propertyChangeAction("PageIndex", false);
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
                _propertyChangeAction("NumberOfPages", false);
            }
        }



        public void UpdateList()
        {
            _completeList = new List<T>(_gridDataSource());
            NumberOfPages = (int)Math.Ceiling((double)_completeList.Count / _numberOfRowsInGrid);
            SetList();
        }


        private void SetList(int pageIndex = 0)
        {
            PageIndex = pageIndex + 1;
            GridSource = _completeList.Skip(pageIndex * _numberOfRowsInGrid).Take(_numberOfRowsInGrid).ToList();
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
}
