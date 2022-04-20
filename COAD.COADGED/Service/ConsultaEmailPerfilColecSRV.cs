using System;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    public class ConsultaEmailPerfilColecSRV : GenericService<CONSULTA_EMAIL_PERFIL_COLEC, ConsultaEmailPerfilColecDTO, string>
    {
        private ConsultaEmailPerfilColecDAO _dao = new ConsultaEmailPerfilColecDAO();

        public ConsultaEmailPerfilColecSRV()
        {
            Dao = _dao;
        }

        public ConsultaEmailPerfilColecDTO BuscarColecionadorPorPerfil(string per_id)
        {
            return _dao.BuscarColecionadorPorPerfil(per_id);
        }
    }
}
