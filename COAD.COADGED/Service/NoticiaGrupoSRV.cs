using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    public class NoticiaGrupoSRV : GenericService<NOTICIA_GRUPO, NoticiaGrupoDTO, int>
    {
        private NoticiaGrupoDAO _dao = new NoticiaGrupoDAO();

        public NoticiaGrupoSRV()
        {
            Dao = _dao;
        }
    }
}
