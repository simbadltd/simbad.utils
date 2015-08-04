using System;

namespace Simbad.Utils
{
    public static class MaybeMonad
    {
        public static TOut With<TIn, TOut>(this TIn obj, Func<TIn, TOut> selector)
            where TIn : class
            where TOut : class
        {
            if (obj == null)
            {
                return null;
            }

            return selector(obj);
        }

        public static TOut WithValue<TIn, TOut>(this TIn obj, Func<TIn, TOut> selector, TOut? defaultValue = null)
            where TIn : class
            where TOut : struct
        {
            if (obj == null)
            {
                return defaultValue.HasValue ? defaultValue.Value : default(TOut);
            }

            return selector(obj);
        }
    }
}
