using DynamicExpresso;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace UniversalOptimizer.utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Reflections the get property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static object? ReflectionGetPropertyValue(this object? obj, string name)
        {
            foreach (string part in name.Split('.'))
            {
                if (obj == null)
                {
                    return null;
                }
                Type type = obj.GetType();
                PropertyInfo? info = type.GetProperty(part);
                if (info == null)
                {
                    return null;
                }
                obj = info.GetValue(obj, null);
            }
            return obj;
        }
        public static object? ReflectionEvaluateFunctionOneVariable(this string functionExpression, double arg)
        {
            var interpreter = new Interpreter();
            string x = arg.ToString(CultureInfo.InvariantCulture.NumberFormat); 
            string expression = functionExpression.Replace("x", "(" +  x + ")");
            try
            {
                var result = interpreter.Eval(expression);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error during interpretation of expression '{0}' with argument '{1}'. Reason:{2}", functionExpression, arg, ex));
                return double.NaN;
            }
        }

    }
}
