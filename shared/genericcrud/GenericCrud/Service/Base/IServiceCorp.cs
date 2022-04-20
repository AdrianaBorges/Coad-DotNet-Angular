using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Service.Base
{
    public interface IServiceCorp<T>
    {
        void IncluirReg(T item);
        void ExcluirReg(T item);
        void SalvarReg(T item);
        T BuscarPorId(Object id);
        IQueryable<T> BuscarTodos();
    }
}
