using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Extensions;

namespace COAD.CORPORATIVO.Service
{
    public class URASRV : ServiceAdapter<URA, UraDTO>
    {
        public URADAO _dao = new URADAO();

        public URASRV()
        {
           SetDao(_dao);
        }

        public Pagina<CONFIG_URA_vw> ListarConfigUra(string _ura_id, int numpagina = 1, int linhas = 10)
        {
            return _dao.ListarConfigUra(_ura_id, numpagina, linhas);
        }
    }
}
