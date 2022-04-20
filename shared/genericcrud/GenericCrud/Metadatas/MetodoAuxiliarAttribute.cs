using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Metadatas
{
    /// <summary>
    /// Indica que esse é um método auxiliar. 
    /// Ou seja, foi feito para ser genérico, pode ser chamado por vários métodos, para realizar a ação no qual ele se propõe.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MetodoAuxiliarAttribute : Attribute
    {
    }
}
