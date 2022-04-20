using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COAD.EXTENSION.Util
{
    public static class TypeUtil
    {
        public static bool IsEnumerable(string typeStr)
        {
            if (!string.IsNullOrWhiteSpace(typeStr))
            {
                var resp = (typeStr.Contains("ICollection") ||
                    typeStr.Contains("IList") ||
                    typeStr.Contains("IEnumerable") ||
                    typeStr.Contains("IQueryable") ||
                    typeStr.Contains("ISet") ||
                    typeStr.Contains("List") ||
                    typeStr.Contains("ArrayList") ||
                    typeStr.Contains("LinkedList") ||
                    typeStr.Contains("HashSet"));
                return resp;
            }
            return false;
        }

        public static bool IsGenerics(string typeStr)
        {
            if (!string.IsNullOrWhiteSpace(typeStr))
            {
                var rgx = new Regex(@"\<(.*)\>");
                var temGenerics = rgx.IsMatch(typeStr);
                return temGenerics;
            }
            return false;
        }

        private static void NormalizeCharArrayName(string[] wordsArray, StringBuilder sb)
        {
            
            if (wordsArray.Count() > 0)
            {
                foreach (var className in wordsArray)
                {
                    if(className == className.ToUpper())
                    {
                        var captalizedWord = CaptalizeClassName(className);
                        sb.Append(captalizedWord);
                    }
                    else
                    {
                        var wordsArrayUpperCase = Regex.Split(className, @"(?<!^)(?=[A-Z])");
                        foreach(var word in wordsArrayUpperCase)
                        {
                            var captalizedWord = CaptalizeClassName(word);
                            sb.Append(captalizedWord);
                        }
                    }
                    
                }
            }
            else
            {
                sb.Append(wordsArray.First());
            }
        }
        public static string TransformToDTOName(string entityClassName, bool dtoSufix = true)
        {
            if (!string.IsNullOrWhiteSpace(entityClassName))
            {
                StringBuilder sb = new StringBuilder();

                var wordsArray = entityClassName.Split('_');
                NormalizeCharArrayName(wordsArray, sb);

                if (dtoSufix)
                    sb.Append("DTO");
                return sb.ToString();
            }
            return null;
        }

        public static string CaptalizeClassName(string className)
        {
            if (!string.IsNullOrWhiteSpace(className))
            {
                className = className.ToLower();
                char[] lethers = className.ToCharArray();
                lethers[0] = lethers[0].ToString().ToUpper().ToCharArray()[0];

                className = new string(lethers);
                return className;
            }
            return null;
        }
    }
}
