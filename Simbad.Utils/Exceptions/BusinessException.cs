using System;

namespace Simbad.Utils.Exceptions
{
    public class BusinessException: Exception
    {
        #region ctor

        public BusinessException()
        {
        }

        public BusinessException(string message)
            : base(message)
        {
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion

        public static BusinessException FromMessageFormat(string message, params string[] args)
        {
            return new BusinessException(string.Format(message, args));
        }
    }
}
