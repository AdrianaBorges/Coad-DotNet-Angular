using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.DAO.SqlDinamico;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery;
using GenericCrud.Service;
using System.Data.SqlClient;
using COAD.SEGURANCA.Model.Custons;

namespace COAD.CORPORATIVO.Service.SqlDinamico
{
    public class SqlDinamicoSRV : GenericService<DESCREVER_COLUNAS_Result, DescricaoColunasDTO, int>
    {
        private SqlDinamicoDAO _dao;
        public TypeBuilderSRV _typeBuilder { get; set; }

        public SqlDinamicoSRV()
        {
            this.Dao = _dao;
            this._dao = new SqlDinamicoDAO();
            this._typeBuilder = new TypeBuilderSRV();
        }

        public SqlDinamicoSRV(SqlDinamicoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public IList<TabelasDTO> ListarTabelas()
        {
            return _dao.ListarTabelas();
        }

        public IList<DescricaoColunasDTO> DescreverColunasDaTabela(string nomeTabela)
        {
            return _dao.DescreverColunasDaTabela(nomeTabela);
        }


        private MontagemQueryTabelasDTO GerarTabelas(string schema, string tabela, string alias)
        {
            var tabelaDTO = new MontagemQueryTabelasDTO()
            {

                Schema = schema,
                NomeTabela = tabela,
                Alias = alias
            };

            return tabelaDTO;
        }

        public MontagemQueryDTO GerarQueryDoRelatorio(RelatorioPersonalizadoDTO relatorioPersonalizado)
        {
            StringBuilder sb = new StringBuilder();
            if (relatorioPersonalizado != null)
            {
                var montagemQuery = new MontagemQueryDTO();
                IList<MontagemQueryTabelasDTO> lstMontagemTabela = new List<MontagemQueryTabelasDTO>();
                
                var lstRelatorioTabelas = relatorioPersonalizado.RELATORIO_TABELAS.ToList();
                var lstColunas = relatorioPersonalizado.RELATORIO_TABELA_COLUNAS;
                var lstJoin = relatorioPersonalizado.RELATORIO_JOIN;
                var lstCondicoes = relatorioPersonalizado.RELATORIO_CONDICAO;

                montagemQuery.NomeRelatorio = relatorioPersonalizado.REL_DESCRICAO;

                int index = 0;

                montagemQuery.RelatorioDerivado = 
                    (relatorioPersonalizado.RET_RELATORIO_BASE == null ||
                    relatorioPersonalizado.RET_RELATORIO_BASE == false);

                if (relatorioPersonalizado.RET_RELATORIO_BASE == true)
                {
                    foreach (var tab in lstRelatorioTabelas)
                    {
                        var alias = "tab_";
                        alias += (index < 10) ? "0" + index : index.ToString();

                        var lstColunasDaTabela = relatorioPersonalizado
                            .RELATORIO_TABELA_COLUNAS
                            .Where(x => x.RET_ID == tab.RET_ID);

                        var lstCondicoesDaTabela = relatorioPersonalizado
                            .RELATORIO_CONDICAO
                            .Where(x => x.RET_ID == tab.RET_ID);

                        foreach (var col in lstColunasDaTabela)
                        {
                            col.TableAlias = alias;
                        }

                        foreach (var cond in lstCondicoesDaTabela)
                        {
                            cond.TableAlias = alias;
                        }

                        var montagemColuna = GerarTabelas(tab.RET_SCHEMA, tab.RET_NOME_TABELA, alias);
                        montagemColuna.RET_ID = tab.RET_ID;
                        lstMontagemTabela.Add(montagemColuna);

                        index++;
                    }


                    foreach (var join in lstJoin)
                    {
                        var tab1 = lstMontagemTabela.Where(x => x.RET_ID == join.RELATORIO_TABELAS.RET_ID).FirstOrDefault();
                        var tab2 = lstMontagemTabela.Where(x => x.RET_ID == join.RELATORIO_TABELAS1.RET_ID).FirstOrDefault();
                        var tipo = join.TIPO_JOIN.TPJ_DESCRICAO;
                        var campo1 = join.REJ_NOME_CAMPO1;
                        var campo2 = join.REJ_NOME_CAMPO2;

                        montagemQuery.AdicionarJoin(tab1, tab2, tipo, campo1, campo2);
                    }

                    // tabelas que não possuí joins
                    var tabelasSemJoin = lstRelatorioTabelas.Where(x => !lstMontagemTabela.Select(sel => sel.RET_ID).Contains(x.RET_ID));
                    foreach (var tab in tabelasSemJoin)
                    {
                        var tabExp = lstMontagemTabela.Where(x => x.RET_ID == tab.RET_ID).FirstOrDefault();
                        montagemQuery.AdicionarTabela(tabExp);
                    }
                }
                 string aliasSub = "T";
                if (relatorioPersonalizado != null && relatorioPersonalizado.RelatorioPai != null)
                {
                    var subMontagem = GerarQueryDoRelatorio(relatorioPersonalizado.RelatorioPai);
                    montagemQuery.SubConjunto = subMontagem;
                   
                    montagemQuery.SubConjunto.AliasSubConjunto = aliasSub;
                }

                index = 0;
                var count = lstColunas.Count();

                foreach (var col in lstColunas)
                {
                    var aliasTabela =  (montagemQuery.RelatorioDerivado) ? aliasSub :  col.TableAlias;
                    string aliasColuna = (montagemQuery.RelatorioDerivado) ? null : col.COR_ALIAS;
                    montagemQuery.Select.AdicionarColuna(aliasTabela, col.COR_DESCRICAO, aliasColuna, col.COR_TYPE_NAME, col.COR_IS_NULLABLE);
                }

                foreach (var cond in lstCondicoes)
                {
                    string operadorLogico = null;

                    if(cond.RELATORIO_OPERADOR_LOGICO != null)
                        operadorLogico = cond.RELATORIO_OPERADOR_LOGICO.ROL_DESCRICAO;

                    var lstOperadorCondicional = cond.
                        RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL.
                        Select(x => x.RELATORIO_OPERADOR_CONDICIONAL.ROC_DESCRICAO)
                        .ToList();

                    var aliasTabela = (montagemQuery.RelatorioDerivado) ? aliasSub : cond.TableAlias;

                    var relatorioWhereItens = new MontagemQueryWhereItemDTO()
                    {
                        OperadorLogico = operadorLogico,
                        AliasTabela = aliasTabela,
                        NomeColuna = cond.REC_CAMPO,
                        OperadoresCondicionais = lstOperadorCondicional,
                        Valor = cond.REC_VALOR,
                        Filtro = cond.REC_FILTRO,
                        Label = cond.REC_LABEL_FILTRO,    
                        NomeTipoDado = cond.REC_NOME_TIPO,
                        
                    };
                    montagemQuery.Where.AdicionarCondicao(relatorioWhereItens); 
                }

                return montagemQuery;
            }

            return null;
        }

        public ResultadoQueryDTO ExecutarQueryDinamica(RelatorioPersonalizadoDTO relatorioPersonalizado, IEnumerable<DadosDeFiltroDTO> lstFiltros, int? top = null)
        {
            if (relatorioPersonalizado != null)
            {
                var montagemQuery = GerarQueryDoRelatorio(relatorioPersonalizado);
                montagemQuery.Top = top;
                return GerarQueryDinamica(montagemQuery, lstFiltros);
            }
            return null;
        }

        public ResultadoQueryDTO GerarQueryDinamica(MontagemQueryDTO montagemQuery, IEnumerable<DadosDeFiltroDTO> lstFiltros)
        {
            if (montagemQuery != null)
            {
                IList<SqlParameter> parametros = null;

                if (montagemQuery.Where != null)
                {
                    parametros = montagemQuery.Where.ListarValoresDosParametros(lstFiltros);

                    if (montagemQuery.SubConjunto != null &&
                        montagemQuery.SubConjunto.Where != null)
                    {
                        montagemQuery.SubConjunto.Where.SubConjunto = true;

                        var parametrosSubConjunto = montagemQuery.
                            SubConjunto
                            .Where
                            .ListarValoresDosParametros();

                        parametros = parametros
                            .Concat(parametrosSubConjunto)
                            .ToList();
                    }
                }
                
                var lstColunas = montagemQuery.Select.ListarColunas();
                var tipo = _typeBuilder.CriarTipo(lstColunas);
                var sqlQuery = montagemQuery.ToString();
                var dadosResultado = _dao.GerarQueryDinamica(sqlQuery, tipo, parametros);

                var resultado = new ResultadoQueryDTO();
                resultado.Colunas = montagemQuery.Select.ListarNomesDasColunas();
                resultado.Dados = dadosResultado;

                return resultado;
            }

            return null;
        }
        
    }
}
