using Coad.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.ClassScan
{
    public static class ClassScanner
    {
        public static IEnumerable<Type> ScanNameSpaceForMapperAnnotations(Assembly assembly, string namespaces)
        {
           return ReflectionProvider.FindInNamespaces(assembly, namespaces);
        }
    }
}
