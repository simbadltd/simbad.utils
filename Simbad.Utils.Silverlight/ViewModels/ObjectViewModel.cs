using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simbad.Utils.Silverlight.ViewModels
{
    public class ObjectViewModel<T> : NotifyPropertyChangedBase
    {
        #region Variables

        private T _value;

        #endregion

        #region Properties

        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        #endregion

        #region ctor

        public ObjectViewModel(T value)
        {
            Value = value;
        }

        public ObjectViewModel()
        {
        }

        #endregion
    }
}
