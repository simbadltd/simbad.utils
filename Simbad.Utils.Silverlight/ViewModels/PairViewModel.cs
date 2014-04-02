namespace Simbad.Utils.Silverlight.ViewModels
{
    public class PairViewModel<T1, T2>: NotifyPropertyChangedBase
    {
        #region Variables

        private T1 _key;

        private T2 _value;

        #endregion

        #region Properties

        public T1 Key
        {
            get { return _key; }

            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        public T2 Value
        {
            get { return _value; }

            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        #endregion

        #region ctor

        public PairViewModel(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }

        public PairViewModel()
        {
        }

        #endregion
    }
}
