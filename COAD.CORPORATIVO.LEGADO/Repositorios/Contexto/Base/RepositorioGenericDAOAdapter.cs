using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Repositorios.Base
{
    public class DAOAdapter<T, S, Id> : AbstractGenericDao<T, S, Id>, IDisposable, IRepositorioPadrao<T> where T : class
    {

        protected readonly COADCORPEntities db;

        public DAOAdapter()
        {
            this.db = new COADCORPEntities();
        }

        public DAOAdapter(COADCORPEntities db)
        {
            this.db = db;
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

        public virtual Autenticado BuscaUsuarioAutenticado()
        {
            return SessionContext.autenticado;
        }

        #region IDisposable Members

        public void Dispose()
        {
            db.Dispose();
        }

        #endregion
    }

    public class DAOAdapter<T, S> : DAOAdapter<T, S, Object> where T : class
    {
        
    }
}
