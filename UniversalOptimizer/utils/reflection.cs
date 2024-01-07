using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UniversalOptimizer.utils
{
    public static class Reflection
    {
        /// <summary>
        /// Reflections the get property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static object? ReflectionGetPropertyValue(this object obj, string name)
        {
            foreach (string part in name.Split('.'))
            {
                if (obj == null)
                {
                    return null;
                }
                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null)
                {
                    return null;
                }
                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static object? ReflectionEvaluateExpression(this string expression) => expression;

    }
}
