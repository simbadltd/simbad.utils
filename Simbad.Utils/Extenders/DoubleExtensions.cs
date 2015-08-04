using System;

namespace Simbad.Utils.Extenders
{
    public static class DoubleExtensions
    {
        public const double EPSILON = 0.00001;

        public const decimal DECIMAL_EPSILON = 0.00001M;

        public static bool IsPositive(this double x)
        {
            return x > 0;
        }

        public static bool IsNegative(this double x)
        {
            return x < 0;
        }

        public static bool IsNotZero(this double x)
        {
            return !x.IsEqualsTo(0D);
        }

        public static bool IsZero(this double x)
        {
            return x.IsEqualsTo(0D);
        }

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

        public static bool IsPositive(this int x)
        {
            return x > 0;
        }

        public static bool IsNegative(this int x)
        {
            return x < 0;
        }

        public static bool IsNotZero(this int x)
        {
            return x != 0;
        }

        public static bool IsZero(this int x)
        {
            return x == 0;
        }

        public static int Limit(this int original, int min, int max)
        {
            if (min > max)
            {
                throw new InvalidOperationException(
                    string.Format("Can not limit '{0}' because of limit min '{1}' greater than limit max '{2}'", original, min, max));
            }

            int result;

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

            return result;
        }

        public static bool IsPositive(this decimal x)
        {
            return x > 0;
        }

        public static bool IsNegative(this decimal x)
        {
            return x < 0;
        }

        public static bool IsNotZero(this decimal x)
        {
            return !x.IsEqualsTo(0M);
        }

        public static bool IsZero(this decimal x)
        {
            return x.IsEqualsTo(0M);
        }

        public static bool IsEqualsTo(this decimal x, decimal y)
        {
            return Math.Abs(x - y) < DECIMAL_EPSILON;
        }

        public static decimal Limit(this decimal original, decimal min, decimal max, int digits)
        {
            if (min > max)
            {
                throw new InvalidOperationException(
                    string.Format("Can not limit '{0}' because of limit min '{1}' greater than limit max '{2}'", original, min, max));
            }

            decimal result;

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

        public static decimal Correct(this decimal original, decimal additiveCorrection, decimal multiplicativeCorrection)
        {
            return original * multiplicativeCorrection + additiveCorrection;
        }
    }
}
