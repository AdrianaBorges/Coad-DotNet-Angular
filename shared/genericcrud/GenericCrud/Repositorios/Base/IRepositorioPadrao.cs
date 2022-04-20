using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Repositorios.Base
{
    public interface IRepositorioPadrao<T> where T : class
    {
        void Incluir(T item);
        void Excluir(T item);
        void Salvar(T item);
        T Obter(object id);
        IQueryable<T> Tudo();
    }
}
