namespace Simbad.Utils.Silverlight.ViewModels
{
    public class BusyCounterViewModel: NotifyPropertyChangedBase
    {
        #region Variables

        private readonly object _syncRoot = new object();

        private volatile int _counter;

        private string _message;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get
            {
                return _counter > 0;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }

            private set
            {
                if (value == _message) return;
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public int Counter
        {
            get
            {
                return _counter;
            }

            set
            {
                _counter = value;

                OnPropertyChanged("Counter");
                OnPropertyChanged("IsBusy");
            }
        }

        #endregion

        public void Increase()
        {
            Increase(string.Empty);
        }

        public void Increase(string message)
        {
            lock (_syncRoot)
            {
                Message = message;
                Counter++;
            }
        }

        public void Decrease()
        {
            lock (_syncRoot)
            {
                if (Counter > 0)
                {
                    Counter--;
                }
            }
        }

        public void Reset()
        {
            lock (_syncRoot)
            {
                Counter = 0;
            }
        }
    }
}