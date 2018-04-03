using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitaCidades.Model
{
    static class Extensions
    {
        public static string Random(this string[] array) =>
            array[Utils.Rand.Next(array.Length)];

        public static string[] ArrayOrDefault(this Dictionary<string, string[]> dictionary, string key, string[] defaultValue = null)
        {
            if (dictionary.TryGetValue(key, out string[] value))
            {
                return value;
            }
            return defaultValue;
        }

        public static string ValueOrDefault(this Dictionary<string, string[]> dictionary, string key, string defaultValue = null)
        {
            if (dictionary.TryGetValue(key, out string[] value))
            {
                return value.SingleOrDefault() ?? defaultValue;
            }
            return defaultValue;
        }

        public static int? IntOrDefault(this Dictionary<string, string[]> dictionary, string key, int? defaultValue = null)
        {
            if (int.TryParse(dictionary.ValueOrDefault(key, null), out int n))
            {
                return n;
            }
            return defaultValue;
        }

        public static double? DoubleOrDefault(this Dictionary<string, string[]> dictionary, string key, double? defaultValue = null)
        {
            if (double.TryParse(dictionary.ValueOrDefault(key, null), out double n))
            {
                return n;
            }
            return defaultValue;
        }

        public static string GetDisplayName(this Type type) =>
            (type.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute)?.DisplayName ?? type.Name;
    }
}
