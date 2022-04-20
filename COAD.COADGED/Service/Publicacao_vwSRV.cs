using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class Publicacao_vwSRV : GenericService<PUBLICACAO_vw, Publicacao_vwDTO, int>
    {
        //private Publicacao_vwDAO _dao_vw = new Publicacao_vwDAO();

        public Publicacao_vwSRV()
        {
            //Dao = _dao_vw;
        }

        //public Pagina<Publicacao_vwDTO> Busca(string coadgedBI = null, int pagina = 1, int itensPorPagina = 10)
        //{
            //var resp = _dao_vw.Busca(coadgedBI);
            //return resp;
        //}
    }
}
