namespace Simbad.Utils.Silverlight.ViewModels
{
    public class KeyValuePairViewModel<T1,T2>: NotifyPropertyChangedBase
    {
        private T1 _key;

        private T2 _value;

        public T1 Key
        {
            get
            {
                return _key;
            }

            set
            {
                if (Equals(value, _key)) return;
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        public T2 Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        #region ctor

        public KeyValuePairViewModel(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }

        public KeyValuePairViewModel()
        {
        }

        #endregion
    }
}
