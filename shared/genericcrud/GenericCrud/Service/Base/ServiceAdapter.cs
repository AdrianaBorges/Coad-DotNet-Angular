using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Service.Base
{
    public class ServiceAdapter<T,S, Id> : GenericService<T,S, Id>, IServiceCorp<T> where T : class
    {
        public DAOAdapter<T, S, Id> dao { get; private set; }

        public ServiceAdapter(DAOAdapter<T, S, Id> dao)
        {
            SetDao(dao);
        }

        public ServiceAdapter(bool useDbContextCache = true) : base(useDbContextCache)
        {

        }

        public void SetDao(DAOAdapter<T, S, Id> dao)
        {

           this.Dao = dao;
           this.dao = dao;
        }

        public virtual void IncluirReg(T item)
        {
            dao.Incluir(item);
        }

        public virtual void ExcluirReg(T item)
        {
            dao.Excluir(item);
        }

        public virtual void SalvarReg(T item)
        {
            dao.Salvar(item);
        }

        public T BuscarPorId(object id)
        {
            return dao.FindById(id);
        }

        public IQueryable<T> BuscarTodos()
        {
            return dao.FindAll().AsQueryable();
        }
    }

    public class ServiceAdapter<T, S> : ServiceAdapter<T, S, object> where T : class
    {

    }
}
