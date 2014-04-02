using System;
using System.ComponentModel;

namespace Simbad.Utils.Silverlight
{
    public sealed class OperationResult
    {
        public Action OnSuccess { get; private set; }

        public Action<Exception> OnError { get; private set; }

        public OperationResult(Action onSuccess, Action<Exception> onError)
        {
            OnSuccess = onSuccess;
            OnError = onError;
        }
    }

    public sealed class OperationResult<T> where T : AsyncCompletedEventArgs
    {
        public Action<T> OnSuccess { get; private set; }

        public Action<Exception> OnError { get; private set; }

        public OperationResult(Action<T> onSuccess, Action<Exception> onError)
        {
            OnSuccess = onSuccess;
            OnError = onError;
        }
    }
}