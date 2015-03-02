using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Simbad.Utils.PropertyCopier
{
    /// <summary>
    /// Static class to efficiently store the compiled delegate which can
    /// do the copying. We need a bit of work to ensure that exceptions are
    /// appropriately propagated, as the exception is generated at type initialization
    /// time, but we wish it to be thrown as an ArgumentException.
    /// Note that this type we do not have a constructor constraint on TTarget, because
    /// we only use the constructor when we use the form which creates a new instance.
    /// </summary>
    internal static class PropertyCopier<TSource, TTarget>
    {
        /// <summary>
        /// Delegate to create a new instance of the target type given an instance of the
        /// source type. This is a single delegate from an expression tree.
        /// </summary>
        private static readonly Func<TSource, TTarget> Creator;

        /// <summary>
        /// List of properties to grab values from. The corresponding targetProperties 
        /// list contains the same properties in the target type. Unfortunately we can't
        /// use expression trees to do this, because we basically need a sequence of statements.
        /// We could build a DynamicMethod, but that's significantly more work :) Please mail
        /// me if you really need this...
        /// </summary>
        private static readonly List<PropertyInfo> SourceProperties = new List<PropertyInfo>();
        private static readonly List<PropertyInfo> TargetProperties = new List<PropertyInfo>();
        private static readonly Exception InitializationException;

        internal static TTarget Copy(TSource source)
        {
            if (InitializationException != null)
            {
                throw InitializationException;
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return Creator(source);
        }

        internal static void Copy(TSource source, TTarget target)
        {
            if (InitializationException != null)
            {
                throw InitializationException;
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            for (int i = 0; i < SourceProperties.Count; i++)
            {
                var sourceProperty = SourceProperties[i];
                var targetProperty = TargetProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);

                if (targetProperty == null)
                {
                    continue;
                }

                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }

        }

        static PropertyCopier()
        {
            try
            {
                Creator = BuildCreator();
                InitializationException = null;
            }
            catch (Exception e)
            {
                Creator = null;
                InitializationException = e;
            }
        }

        private static Func<TSource, TTarget> BuildCreator()
        {
            ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource), "source");
            var bindings = new List<MemberBinding>();
            foreach (PropertyInfo sourceProperty in typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!sourceProperty.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeof(TTarget).GetProperty(sourceProperty.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " is not writable in " + typeof(TTarget).FullName);
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " is static in " + typeof(TTarget).FullName);
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " has an incompatible type in " + typeof(TTarget).FullName);
                }
                bindings.Add(Expression.Bind(targetProperty, Expression.Property(sourceParameter, sourceProperty)));
                SourceProperties.Add(sourceProperty);
                TargetProperties.Add(targetProperty);
            }
            Expression initializer = Expression.MemberInit(Expression.New(typeof(TTarget)), bindings);
            return Expression.Lambda<Func<TSource, TTarget>>(initializer, sourceParameter).Compile();
        }
    }
}
