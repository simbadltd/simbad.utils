using System;

namespace Simbad.Utils.Extenders
{
    public static class DoubleExtensions
    {
        public const double EPSILON = 0.00001;

        public static bool IsEqualsTo(this double x, double y)
        {
            return Math.Abs(x - y) < EPSILON;
        }

        public static double Limit(this double original, double min, double max, int digits)
        {
            if (min > max)
            {
                throw new InvalidOperationException(
                    string.Format("Can not limit '{0}' because of limit min '{1}' greater than limit max '{2}'", original, min, max));
            }

            double result;

            if (original < min)
            {
                result = min;
            }
            else if (original > max)
            {
                result = max;
            }
            else
            {
                result = original;
            }

            return Math.Round(result, digits);
        }

        public static double Correct(this double original, double additiveCorrection, double multiplicativeCorrection)
        {
            return original * multiplicativeCorrection + additiveCorrection;
        }
    }
}
