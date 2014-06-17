using System;
using System.Linq.Expressions;

namespace Simbad.Utils
{
    public static class Guard
    {
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