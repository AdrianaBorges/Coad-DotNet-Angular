using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.SEGURANCA.Repositorios.Base
{

    public class RepositorioPadrao<T> : IDisposable, IRepositorioPadrao<T> where T : class
    {
        protected readonly COADSYSEntities db;
        public RepositorioPadrao(COADSYSEntities db)
        {
            this.db = db;
        }
        public RepositorioPadrao()
        {
            this.db = new COAD.SEGURANCA.Repositorios.Contexto.COADSYSEntities();
        }
        public virtual void Incluir(T item)
        {
             db.Set<T>().Add(item);
             db.SaveChanges();
        }
        public virtual void Excluir(T item)
        {
             db.Set<T>().Attach(item);
             db.Set<T>().Remove(item);
             db.SaveChanges();

        }
        public virtual void Salvar(T item)
        {
             db.Entry(item).State = EntityState.Modified;
             db.SaveChanges();
        }
        public virtual T Obter(object id)
        {
            return db.Set<T>().Find(id);
        }
        public virtual IQueryable<T> Tudo()
        {
            return db.Set<T>();
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

}