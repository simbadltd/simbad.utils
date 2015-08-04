using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Simbad.Utils.Extenders;

namespace Simbad.Utils
{
    public static class Guard
    {
        public static void EnsurePositive(Expression<Func<int>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsurePositive(value, name);
        }

        public static void EnsureZeroOrPositive(Expression<Func<int>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsureZeroOrPositive(value, name);
        }

        public static void EnsureNegative(Expression<Func<int>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsureNegative(value, name);
        }

        public static void EnsureZeroOrNegative(Expression<Func<int>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsureZeroOrNegative(value, name);
        }

        public static void EnsurePositive(int i, string name)
        {
            if (i.IsZero() || i.IsNegative())
            {
                throw new ArgumentException("Value should be positive", name);
            }
        }

        public static void EnsureZeroOrPositive(int i, string name)
        {
            if (i.IsNegative())
            {
                throw new ArgumentException("Value should be zero or positive", name);
            }
        }

        public static void EnsureNegative(int i, string name)
        {
            if (i.IsZero() || i.IsPositive())
            {
                throw new ArgumentException("Value should be negative", name);
            }
        }

        public static void EnsureZeroOrNegative(int i, string name)
        {
            if (i.IsPositive())
            {
                throw new ArgumentException("Value should be zero or negative", name);
            }
        }

        public static void EnsurePositive(Expression<Func<double>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsurePositive(value, name);
        }

        public static void EnsureZeroOrPositive(Expression<Func<double>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsureZeroOrPositive(value, name);
        }

        public static void EnsureNegative(Expression<Func<double>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsureNegative(value, name);
        }

        public static void EnsureZeroOrNegative(Expression<Func<double>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsureZeroOrNegative(value, name);
        }

        public static void EnsurePositive(double d, string name)
        {
            if (d.IsZero() || d.IsNegative())
            {
                throw new ArgumentException("Value should be positive", name);
            }
        }

        public static void EnsureZeroOrPositive(double d, string name)
        {
            if (d.IsNegative())
            {
                throw new ArgumentException("Value should be zero or positive", name);
            }
        }

        public static void EnsureNegative(double d, string name)
        {
            if (d.IsZero() || d.IsPositive())
            {
                throw new ArgumentException("Value should be negative", name);
            }
        }

        public static void EnsureZeroOrNegative(double d, string name)
        {
            if (d.IsPositive())
            {
                throw new ArgumentException("Value should be zero or negative", name);
            }
        }

        public static void EnsurePositive(Expression<Func<decimal>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsurePositive(value, name);
        }

        public static void EnsureZeroOrPositive(Expression<Func<decimal>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsureZeroOrPositive(value, name);
        }

        public static void EnsureNegative(Expression<Func<decimal>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsureNegative(value, name);
        }

        public static void EnsureZeroOrNegative(Expression<Func<decimal>> parameterExpression)
        {
            var value = parameterExpression.Compile()();
            var name = GetParameterName(parameterExpression);

            EnsureZeroOrNegative(value, name);
        }

        public static void EnsurePositive(decimal d, string name)
        {
            if (d.IsZero() || d.IsNegative())
            {
                throw new ArgumentException("Value should be positive", name);
            }
        }

        public static void EnsureZeroOrPositive(decimal d, string name)
        {
            if (d.IsNegative())
            {
                throw new ArgumentException("Value should be zero or positive", name);
            }
        }

        public static void EnsureNegative(decimal d, string name)
        {
            if (d.IsZero() || d.IsPositive())
            {
                throw new ArgumentException("Value should be negative", name);
            }
        }

        public static void EnsureZeroOrNegative(decimal d, string name)
        {
            if (d.IsPositive())
            {
                throw new ArgumentException("Value should be zero or negative", name);
            }
        }

        public static void NotNullOrEmpty(Expression<Func<string>> parameterExpression)
        {
            var value = parameterExpression.Compile()();

            if (String.IsNullOrWhiteSpace(value))
            {
                var name = GetParameterName(parameterExpression);
                throw new ArgumentException("Cannot be null or empty", name);
            }
        }

        public static void NotNull<T>(Expression<Func<T>> parameterExpression)
            where T : class
        {
            var parameterValue = parameterExpression.Compile()();
            var parameterName = GetParameterName(parameterExpression);

            NotNull(parameterValue, parameterName);
        }

        [ContractAnnotation("parameterValue:null => halt")]
        public static void NotNull<T>(T parameterValue, string parameterName) where T: class
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void EnsureHasValue<T>(Expression<Func<T?>> parameterExpression) where T : struct
        {
            var parameterValue = parameterExpression.Compile()();
            var parameterName = GetParameterName(parameterExpression);

            EnsureHasValue(parameterValue, parameterName);
        }

        [ContractAnnotation("parameterValue:null => halt")]
        public static void EnsureHasValue<T>(T? parameterValue, string parameterName) where T : struct
        {
            if (!parameterValue.HasValue)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        private static string GetParameterName<T>(Expression<Func<T>> parameterExpression)
        {
            dynamic body = parameterExpression.Body;
            return body.Member.Name;
        }
    }
}