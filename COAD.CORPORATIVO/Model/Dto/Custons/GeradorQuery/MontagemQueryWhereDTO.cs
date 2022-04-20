using COAD.CORPORATIVO.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery
{
    public class MontagemQueryWhereDTO : IQueryFromExpression
    {
        public bool SubConjunto { get; set; }

        private IList<MontagemQueryWhereItemDTO> LstMontagemWhereItem = new List<MontagemQueryWhereItemDTO>();

        public void AdicionarCondicao(string operadorLogico, string aliasTabela, string nomeColuna, IList<string> operadoresCondicionais, object valor = null)
        {
            var itemColuna = new MontagemQueryWhereItemDTO()
            {
                OperadorLogico = operadorLogico,
                AliasTabela = aliasTabela,
                NomeColuna = nomeColuna,
                OperadoresCondicionais = operadoresCondicionais,
                Valor = valor
            };

            LstMontagemWhereItem.Add(itemColuna);
        }

        public void AdicionarCondicao(MontagemQueryWhereItemDTO itemColuna)
        {
            LstMontagemWhereItem.Add(itemColuna);
        }

        /// <summary>
        /// Processa os valores de parametros e devolve em forma de parametros de SQL
        /// </summary>
        /// <param name="lstFiltros">Filtros vindos da interface com o cliente para concatenar com os parâmetros fixos</param>
        /// <returns></returns>
        public IList<SqlParameter> ListarValoresDosParametros(IEnumerable<DadosDeFiltroDTO> lstFiltros = null)
        {
            int index = 0;

            int indexFiltro = 0;
            IList<SqlParameter> listParams = new List<SqlParameter>();

            foreach(var param in LstMontagemWhereItem)
            {
                string paramAlias = null;

                if (param.Filtro != true)
                {
                    if (SubConjunto)
                        paramAlias = $"@ps{index}";
                    else paramAlias = $"@p{index}";
                    listParams.Add(new SqlParameter(paramAlias, param.Valor));
                }
                else
                {
                    var aliasParam = "@pFiltro" + indexFiltro;
                    if (lstFiltros != null)
                    {
                        if (!lstFiltros
                            .Select(x => x.NomeCampo)
                            .Contains(param.AliasTabela + "." + param.NomeColuna))
                        {                            
                            listParams.Add(new SqlParameter(aliasParam, null));
                        }
                    }
                    else
                    {
                        listParams.Add(new SqlParameter(aliasParam, null));
                    }

                    indexFiltro++;
                }

                index++;
            }

            if (lstFiltros != null)
            {
                index = 0;
                foreach (var filtro in lstFiltros)
                {
                    var paramAlias = "@pFiltro" + index;
                    listParams.Add(new SqlParameter(paramAlias, filtro.Valor));
                    index++;
                }
            }

            return listParams;
        }

        public IList<MontagemQueryWhereItemDTO> ListarFiltros()
        {
            if (LstMontagemWhereItem != null)
            {
                return LstMontagemWhereItem
                    .Where(x => x.Filtro == true)
                    .Select(x => x)
                    .ToList();
            }

            return new List<MontagemQueryWhereItemDTO>();
        }


        public IList<DadosDeFiltroDTO> ObterMetaDadoDeFiltros()
        {
            if (LstMontagemWhereItem != null)
            {
                return LstMontagemWhereItem
                    .Where(x => x.Filtro == true)
                    .Select(x => new DadosDeFiltroDTO() { 
                        Label = x.Label,
                        NomeTipo = MontagemSQLUtil.MapearTipoDadoInterfaceUsuario(x.NomeTipoDado),
                        NomeCampo =  x.AliasTabela + "." + x.NomeColuna
                    })
                    .ToList();
            }

            return new List<DadosDeFiltroDTO>();
        }

        public string GerarExpressao()
        {
            int index = 0;
            int count = LstMontagemWhereItem.Count();

            StringBuilder sb = new StringBuilder(" \t 1 = 1 ");
            foreach (var cond in LstMontagemWhereItem)
            {
                sb.Append(" ");
                sb.Append(cond.OperadorLogico);
                sb.Append(" \r\n");
                
                if(cond.Filtro == true)
                {
                    sb.Append(" ( ");
                    var condicao = "\t[{0}].[{1}]";
                    condicao = string.Format(condicao, cond.AliasTabela, cond.NomeColuna);

                    sb.Append("@pFiltro");
                    sb.Append(index);
                    sb.Append(" IS NULL OR ");

                    sb.Append(condicao);

                    bool possuiLike = false;
                    if (cond.OperadoresCondicionais != null)
                    {
                        foreach (var ope in cond.OperadoresCondicionais)
                        {
                            if (ope == "LIKE")
                            {
                                possuiLike = true;
                            }
                            sb.Append(" ");
                            sb.Append(ope);
                            sb.Append(" ");
                        }
                    }

                    if (possuiLike)
                        sb.Append("'%'+");
                    
                    sb.Append("@pFiltro");
                    sb.Append(index);
                    
                    if (possuiLike)
                        sb.Append("+'%'"); 
                    
                    sb.Append(" ) ");

                }
                else
                {

                    var condicao = "\t[{0}].[{1}]";
                    sb.Append(string.Format(condicao, cond.AliasTabela, cond.NomeColuna));

                    if (cond.OperadoresCondicionais != null)
                    {
                        foreach (var ope in cond.OperadoresCondicionais)
                        {
                            sb.Append(" ");
                            sb.Append(ope);
                            sb.Append(" ");
                        }
                    }

                    if (SubConjunto)
                        sb.Append("@ps");
                    else sb.Append("@p");

                    sb.Append(index);

                    

                }
                index++;
              
            
            }

            return sb.ToString();
        }
    }
}
