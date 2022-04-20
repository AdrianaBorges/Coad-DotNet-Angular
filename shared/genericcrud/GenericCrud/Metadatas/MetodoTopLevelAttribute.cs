using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Metadatas
{
    /// <summary>
    /// Indica que o método é um método top level. Ou seja. 
    /// Não deve/deveria ser referênciado dentro de outro método e sim ser chamado pelo controlador (controller).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MetodoTopLevelAttribute : Attribute
    {
    }
}
