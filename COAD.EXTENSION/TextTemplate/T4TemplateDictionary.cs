using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.EXTENSION.TextTemplate
{
    public class T4TemplateDictionary
    {
        public static Dictionary<string, object> Dictionary = new Dictionary<string, object>();

        public static void AddValue(string key, object value)
        {
            if (!Dictionary.Keys.Contains(key))
            {
                Dictionary.Add(key, value);
            }
            else
            {
                Dictionary[key] = value;
            }
        }

        public static T GetValue<T>(string key) where T : class
        {
            var value = Dictionary[key];
            if (value != null &&value is T)
                return value as T;
            return null;
        }
    }
}
