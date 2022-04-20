using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using Coad.GenericCrud.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery;
using System.Data.SqlClient;

namespace COAD.CORPORATIVO.DAO.SqlDinamico
{
    public class SqlDinamicoDAO : DAOAdapter<DESCREVER_COLUNAS_Result, DescricaoColunasDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public SqlDinamicoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<TabelasDTO> ListarTabelas()
        {
            var query = (from tb in db.TABELAS_VW 
                         orderby tb.nome
                         select tb);

            var pagina = Convert<IQueryable<TABELAS_VW>, List<TabelasDTO>>(query);

            return pagina;
        }

        public IList<DescricaoColunasDTO> DescreverColunasDaTabela(string nomeTabela)
        {
            var lstDescColunas = db.DESCREVER_COLUNAS(nomeTabela);
            return ToDTO(lstDescColunas);
        }

        public IEnumerable<object> GerarQueryDinamica(string query, Type tipo, IList<SqlParameter> parametros)
        {
            ResultadoQueryDTO resultado = new ResultadoQueryDTO();
            SqlParameter[] parameters = parametros.ToArray();

            foreach (var param in parametros)
            {
                if (param.Value == null)
                    param.Value = DBNull.Value;

                if (param.Value is bool && (bool) param.Value == false)
                {
                    param.Value = DBNull.Value;
                }
            }

            var result = db.Database.SqlQuery(tipo, query, parameters);

            IList<object> list = new List<object>();
            
            foreach (var resp in result)
            {
                list.Add(resp);
            }

            return list;
        }        
    }
}
