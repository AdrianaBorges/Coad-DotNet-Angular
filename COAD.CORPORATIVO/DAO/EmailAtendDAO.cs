using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class EmailAtendDAO : DAOAdapter<EMAIL_ATEND, EmailAtendDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public EmailAtendDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }
        public Pagina<EmailAtendDTO> ListarEmail(string _usu_login = null, int pagina = 1, int registroPorPagina = 20)
        {
            IQueryable<EMAIL_ATEND> query = (from e in db.EMAIL_ATEND
                                             where e.USU_LOGIN == _usu_login
                                             select e).OrderByDescending(x => x.EAT_DATA);

            return ToDTOPage(query, pagina, registroPorPagina);
        } 
    }
}
