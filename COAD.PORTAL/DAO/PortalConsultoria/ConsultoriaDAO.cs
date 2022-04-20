using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PORTAL.Model.DTO.PortalConsultoria;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.DAO.PortalConsultoria
{
    public class ConsultoriaDAO : AbstractGenericDao<consultoria, ConsultoriaPortalDTO, int>
    {
        private consultoriaEntities db { get; set; }

        public ConsultoriaDAO()
        {
            SetProfileName("portalConsultoria");
            db = GetDb<consultoriaEntities>(false);
        }

        public Pagina<ConsultoriaPortalDTO> BuscarConsultasPorPerfil(int id, string codigo, string status, DateTime periodoInicial, DateTime periodoFinal, string uf, string colec, int pagina, int itensPorPagina)
        {
            bool desconsideraStatus=false;

            //Se algum dos campos da pesquisa for preenchido ele desconsidera o status
            //Caso contrário ele desconsidera o status 5(respondido) para que a consulta inicial não demore
            //Obs.: O id do perfil não é considerado para evita que um perfil vizuali-se consultas de outro
            if (id == 0 && codigo.Equals("") && uf.Equals("") && !status.Equals(""))
                desconsideraStatus = true;

            string queryString = $"SELECT * FROM consultoria WHERE DATE(dataCadastro) >= '" + periodoInicial.ToString("yyyy-MM-dd") + " 00:00:01' and DATE(dataCadastro) <= '" + periodoFinal.ToString("yyyy-MM-dd") + " 23:59:59'";
            queryString = queryString + " and (STR_TO_DATE(dataUltimoAcesso, '%d-%b-%Y') IS NULL OR DATE_ADD(dataUltimoAcesso, INTERVAL 1 HOUR) < NOW())";
            queryString = (id != 0) ? queryString + $" and id = '{id}'" : queryString;
            queryString = (!codigo.Equals("")) ? queryString + $" and usuario = '{codigo}'" : queryString;
            queryString = (!uf.Equals("")) ? queryString + $" and estado = '{uf}'" : queryString;
            queryString = (!colec.Equals("")) ? queryString + $" and colec = '{colec}'" : queryString;
            if (desconsideraStatus)
                queryString = (!status.Equals("")) ? queryString + $" and status = {status}" : queryString + " and status <> 5";

            queryString = queryString + " ORDER BY dataCadastro DESC";

            var _query = GetDbSet().SqlQuery(queryString).ToList();

            return ToDTOPage(_query, pagina, itensPorPagina);
        }
    }
}
