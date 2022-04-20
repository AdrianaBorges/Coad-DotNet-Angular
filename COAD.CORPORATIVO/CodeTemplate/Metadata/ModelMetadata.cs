using GenericCrud.Config.ClassScan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.CodeTemplate.Metadata
{
    public static class ModelMetadata
    {
        public static IEnumerable<Type> GetModelTypes(Assembly assembly)
        {
            
            return ClassScanner.ScanNameSpaceForMapperAnnotations(assembly, "COAD.CORPORATIVO.Repositorios.Contexto");
        }
    }
}
