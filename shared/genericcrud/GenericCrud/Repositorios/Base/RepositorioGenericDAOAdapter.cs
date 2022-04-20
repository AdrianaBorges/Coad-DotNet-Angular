using Coad.GenericCrud.Dao.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Repositorios.Base
{
    public class DAOAdapter<T, S, Id> : AbstractGenericDao<T, S, Id>, IDisposable, IRepositorioPadrao<T> where T : class
    {
        public DAOAdapter(bool useDbContextCache = true) 
            : base(useDbContextCache)
        {

        }

        public void Incluir(T item)
        {
            Save(item);
        }

        public void Excluir(T item)
        {
            Delete(item);
        }

        public void Salvar(T item)
        {
            Update(item);
        }

        public T Obter(object id)
        {
            return FindById(id);
        }

        public IQueryable<T> Tudo()
        {
            return FindAll().AsQueryable();
        }

        //public virtual Autenticado BuscaUsuarioAutenticado()
        //{
        //    return SessionContext.autenticado;
        //}

        #region IDisposable Members

        //public void Dispose()
        //{
        //    db.Dispose();
        //}

        #endregion
    }

    public class DAOAdapter<T, S> : DAOAdapter<T, S, Object> where T : class
    {
        
    }
}
