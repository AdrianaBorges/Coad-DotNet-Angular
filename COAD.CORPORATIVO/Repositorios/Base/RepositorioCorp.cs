using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
    
namespace COAD.CORPORATIVO.Repositorios.Base
{

    public class RepositorioCorp<T> : IDisposable, IRepositorioPadrao<T> where T : class
    {
        protected readonly COADCORPEntities db;
        public RepositorioCorp(COADCORPEntities db)
        {
            this.db = db;
        }
        public RepositorioCorp()
        {
            this.db = new COAD.CORPORATIVO.Repositorios.Contexto.COADCORPEntities();
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