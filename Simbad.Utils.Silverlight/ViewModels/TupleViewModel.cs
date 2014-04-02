namespace Simbad.Utils.Silverlight.ViewModels
{
    public class TupleViewModel<T> : NotifyPropertyChangedBase
    {
        private T _item;

        public T Item
        {
            get
            {
                return _item;
            }

            set
            {
                if (Equals(value, _item)) return;
                _item = value;
                OnPropertyChanged("Item");
            }
        }

        #region ctor

        public TupleViewModel(T item)
        {
            Item = item;
        }

        public TupleViewModel()
        {
        }

        #endregion
    }

    public class TupleViewModel<T1, T2> : NotifyPropertyChangedBase
    {
        private T1 _item1;

        private T2 _item2;

        public T1 Item1
        {
            get
            {
                return _item1;
            }

            set
            {
                if (Equals(value, _item1)) return;
                _item1 = value;
                OnPropertyChanged("Item1");
            }
        }

        public T2 Item2
        {
            get
            {
                return _item2;
            }

            set
            {
                if (Equals(value, _item2)) return;
                _item2 = value;
                OnPropertyChanged("Item2");
            }
        }

        #region ctor

        public TupleViewModel(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public TupleViewModel()
        {
        }

        #endregion
    }

    public class TupleViewModel<T1, T2, T3> : NotifyPropertyChangedBase
    {
        private T1 _item1;

        private T2 _item2;

        private T3 _item3;

        public T1 Item1
        {
            get
            {
                return _item1;
            }

            set
            {
                if (Equals(value, _item1)) return;
                _item1 = value;
                OnPropertyChanged("Item1");
            }
        }

        public T2 Item2
        {
            get
            {
                return _item2;
            }

            set
            {
                if (Equals(value, _item2)) return;
                _item2 = value;
                OnPropertyChanged("Item2");
            }
        }

        public T3 Item3
        {
            get
            {
                return _item3;
            }

            set
            {
                if (Equals(value, _item3)) return;
                _item3 = value;
                OnPropertyChanged("Item3");
            }
        }

        #region ctor

        public TupleViewModel(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        public TupleViewModel()
        {
        }

        #endregion
    }
}