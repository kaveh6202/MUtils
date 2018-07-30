using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace MUtils.Cqrs
{
    public static class CloneHelper
    {
        public static TTarget Clone<TTarget>(this object source, object overrides = null)
        {
            var target = New<TTarget>.Instance();

            var targetProps = typeof(TTarget).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var targetFields = typeof(TTarget).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            var sourceProps = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var ovrProps = overrides?.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var tprop in targetProps)
            {
                var setter = GetSetter(tprop, targetFields);

                var ovrProp = ovrProps?.FirstOrDefault(
                    o => tprop.Name.Equals(o.Name, StringComparison.OrdinalIgnoreCase));
                if (ovrProp != null)
                {
                    var ovValue = ovrProp.GetValue(overrides);
                    setter(target, ovValue);
                    continue;
                }

                var sprop = sourceProps.FirstOrDefault(s => s.Name == tprop.Name);
                if (sprop == null)
                    continue;
                var value = sprop.GetValue(source);
                setter(target, value);
            }

            return target;
        }

        private static Action<object, object> GetSetter(PropertyInfo tprop, FieldInfo[] targetFields)
        {
            var setterMethod = tprop.SetMethod;
            if (setterMethod != null)
                return (instance, value) => setterMethod.Invoke(instance, new[] { value });
            var backendField = targetFields.FirstOrDefault(f => f.Name == $"<{tprop.Name}>k__BackingField");
            if (backendField == null)
                return null;
            return (instance, value) => backendField.SetValue(instance, value);
        }
    }

    public static class New<T>
    {
        public static readonly Func<T> Instance = Creator();

        static Func<T> Creator()
        {
            Type t = typeof(T);
            if (t == typeof(string))
                return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();

            if (HasDefaultConstructor(t))
                return Expression.Lambda<Func<T>>(Expression.New(t)).Compile();

            return () => (T)FormatterServices.GetUninitializedObject(t);
        }

        public static bool HasDefaultConstructor(Type t)
        {
            return t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}