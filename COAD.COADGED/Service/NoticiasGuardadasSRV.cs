using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    public class NoticiasGuardadasSRV : GenericService<NOTICIAS_GUARDADAS, NoticiasGuardadasDTO, int>
    {
        private NoticiasGuardadasDAO _dao = new NoticiasGuardadasDAO();

        public NoticiasGuardadasSRV()
        {
            Dao = _dao;
        }

        public IList<NoticiasGuardadasDTO> NoticiasSalvas(int idCliente)
        {
            return _dao.BuscarNoticiasSalvas(idCliente);
        }

        public NoticiasGuardadasDTO BuscarNoticiaPorIdClienteIdNoticia(int idCliente, int idNoticia)
        {
            return _dao.BuscarNoticiaPorIdClienteIdNoticia(idCliente, idNoticia);
        }

        public void ExcluirNoticia(int idCliente, int idNoticia)
        {
            _dao.ExcluirNoticiaPorIdClienteIdNoticia(idCliente, idNoticia);
        }
    }
}
