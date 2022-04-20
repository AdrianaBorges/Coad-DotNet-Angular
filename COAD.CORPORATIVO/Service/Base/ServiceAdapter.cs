using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Base
{
    public class ServiceAdapter<T,S> : GenericService<T,S, object>, IServiceCorp<T> where T : class
    {
        public DAOAdapter<T, S> dao { get; private set; }

        public ServiceAdapter(DAOAdapter<T, S> dao)
        {
            SetDao(dao);
        }

        public ServiceAdapter()
        {

        }

        public void SetDao(DAOAdapter<T, S> dao)
        {
           this.Dao = dao;
           this.dao = dao;
        }

        public void IncluirReg(T item)
        {
            dao.Incluir(item);
        }

        public void ExcluirReg(T item)
        {
            dao.Excluir(item);
        }

        public void SalvarReg(T item)
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
}
