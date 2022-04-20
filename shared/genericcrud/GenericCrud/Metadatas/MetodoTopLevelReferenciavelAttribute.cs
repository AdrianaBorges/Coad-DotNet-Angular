using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Metadatas
{
    /// <summary>
    /// Indica que o método é um método top/middle level. Ou seja. 
    /// Pode ser referênciado dentro de outro método e também possui autonomia de ser chamado direto de um controlador.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MetodoTopLevelReferenciavelAttribute : Attribute
    {
    }
}
