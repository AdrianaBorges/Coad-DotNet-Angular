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
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes.Maps;
using System.Transactions;
using COAD.CORPORATIVO.Service.SqlDinamico;
using COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery;
using COAD.SEGURANCA.Model.Custons;
using GenericCrud.Excel.Impl;
using System.IO;
using GenericCrud.Util;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("REL_ID")]
    public class RelatorioPersonalizadoSRV : GenericService<RELATORIO_PERSONALIZADO, RelatorioPersonalizadoDTO, int>
    {
        private RelatorioPersonalizadoDAO _dao { get; set; }
        public RelatorioTabelasSRV _relTabSRV { get; set; }
        public RelatorioTabelaColunasSRV _relTabColSRV { get; set; }
        public SqlDinamicoSRV _sqlDinamicoSRV { get; set; }
        public RelatorioJoinSRV _relatorioJoin { get; set; }
        public RelatorioCondicaoSRV _relatorioCondicaoSRV { get; set; }
        
        public RelatorioPersonalizadoSRV(RelatorioPersonalizadoDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }
        public void SalvarRelatorioPersonalizado(RelatorioPersonalizadoDTO relatorio)
        {
            using(var scope = new TransactionScope())
            {
                if (relatorio != null)
                {
                    relatorio.RET_RELATORIO_BASE = true;
                    var relatorioSalvo = SaveOrUpdate(relatorio);
                    relatorio.REL_ID = relatorioSalvo.REL_ID;

                    _relTabSRV.SalvarRelatorioTabelas(relatorio);
                    _relTabColSRV.SalvarRelatorioTabelaColunas(relatorio);
                    _relatorioJoin.SalvarRelatorioJoin(relatorio);
                    _relatorioCondicaoSRV.SalvarRelatorioCondicao(relatorio);
                }

                scope.Complete();
            }
        }

        public void SalvarRelatorioPersonalizadoDerivado(RelatorioPersonalizadoDTO relatorio)
        {
            using (var scope = new TransactionScope())
            {
                if (relatorio != null)
                {
                    relatorio.RET_RELATORIO_BASE = false;

                    if (relatorio.REL_ID_PAI == null && 
                        relatorio.RelatorioPai != null)
                    {
                        relatorio.REL_ID_PAI = relatorio.RelatorioPai.REL_ID;
                    }

                    var relatorioSalvo = SaveOrUpdate(relatorio);
                    relatorio.REL_ID = relatorioSalvo.REL_ID;

                    _relTabColSRV.SalvarRelatorioTabelaColunas(relatorio);
                    _relatorioCondicaoSRV.SalvarRelatorioCondicao(relatorio);
                }

                scope.Complete();
            }
        }

        public RelatorioPersonalizadoDTO CarregarDadosRelatorioPersonalizadoDerivado(int? relId)
        {
            var relatorioDerivado = FindById(relId);
            _relTabColSRV.PreencherRelatorioTabelaColunas(relatorioDerivado);
            _relatorioCondicaoSRV.PreencherRelatorioCondicao(relatorioDerivado);
            PreencherRelatorioPai(relatorioDerivado);


            return relatorioDerivado;
        }


        public RelatorioPersonalizadoDTO FindByIdFullLoaded(int? relId, 
            bool preencherRelatorioTabelas = true, 
            bool preencherRelatorioTabelaColunas = false, 
            bool preecherJoins = false,
            bool preecherColunasNaTabela = false,
            bool preecherRelatorioCondicao = false)
        {
            var relPersonalizado = FindById(relId);

            if (relPersonalizado.RET_RELATORIO_BASE != null && 
                relPersonalizado.RET_RELATORIO_BASE == true)
            {
                if (relPersonalizado != null)
                {
                    if (preencherRelatorioTabelas)
                        _relTabSRV.PreencherListaRelatorioTabelas(relPersonalizado);

                    if (preencherRelatorioTabelaColunas)
                        _relTabColSRV.PreencherRelatorioTabelaColunas(relPersonalizado);

                    if (preecherJoins)
                        _relatorioJoin.PreencherRelatorioJoin(relPersonalizado);

                    if (preecherColunasNaTabela && preencherRelatorioTabelas)
                        PreencherColunasDaTabela(relPersonalizado.RELATORIO_TABELAS);

                    if (preecherRelatorioCondicao)
                        _relatorioCondicaoSRV.PreencherRelatorioCondicao(relPersonalizado, true);
                }
            }
            else
            {
                if (preencherRelatorioTabelaColunas)
                    _relTabColSRV.PreencherRelatorioTabelaColunas(relPersonalizado);

                if (preecherRelatorioCondicao)
                    _relatorioCondicaoSRV.PreencherRelatorioCondicao(relPersonalizado, true);

                PreencherRelatorioPai(relPersonalizado);
            }
            return relPersonalizado;
        }

        public IList<RelatorioTabelasDTO> ListarTabelas()
        {
            var lstTabelas = _sqlDinamicoSRV.ListarTabelas();
            if(lstTabelas != null){

                var lstRelatorioTabelasDTO = lstTabelas.Select(sel => new RelatorioTabelasDTO() { 
                
                    RET_NOME_TABELA = sel.nome,
                    RET_SCHEMA = sel.schema,                    
                });

                return lstRelatorioTabelasDTO.ToList();
            }

            return new List<RelatorioTabelasDTO>();
        }

        public IList<RelatorioTabelaColunasDTO> DescreverColunasDaTabela(string nomeTabela)
        {
            var lstColunas = _sqlDinamicoSRV.DescreverColunasDaTabela(nomeTabela);

            if (lstColunas != null)
            {
                var lstRelatorioColunasDTO = lstColunas.Select(sel => new RelatorioTabelaColunasDTO()
                {
                    COR_DESCRICAO = sel.COLUMN_NAME,
                    COR_TYPE_NAME = sel.TYPE_NAME,
                    COR_IS_NULLABLE = (sel.IS_NULLABLE == "YES") ? true : false,
                    RELATORIO_TABELAS = new RelatorioTabelasDTO()
                    {
                        RET_NOME_TABELA = sel.TABLE_NAME,
                        RET_SCHEMA = sel.TABLE_OWNER,
                    }
                });

                return lstRelatorioColunasDTO.ToList();
            }

            return new List<RelatorioTabelaColunasDTO>();
        }

        public void PreencherColunasDaTabela(ICollection<RelatorioTabelasDTO> lstTabelas)
        {
            if (lstTabelas != null)
            {
                foreach (var tb in lstTabelas)
                {
                    var nomeTabela = tb.RET_NOME_TABELA;
                    tb.Colunas = DescreverColunasDaTabela(nomeTabela);
                }
            }
        }


        public MetaDadoRelatorioDTO ObterMetaDadoDoRelatorio(int? relId)
        {
            var montagem = GerarQueryDoRelatorio(relId);

            if (montagem != null)
            {
                MetaDadoRelatorioDTO metadata = new MetaDadoRelatorioDTO();

                metadata.NomeRelatorio = montagem.NomeRelatorio;

                if(montagem.Select != null)
                {
                    metadata.Colunas = montagem.Select.ListarNomesDasColunasMetadata();
                }

                if (montagem.Where != null)
                {
                    metadata.Filtros = montagem.Where.ObterMetaDadoDeFiltros();
                }

                return metadata;
            }

            return null;
        }

        public MontagemQueryDTO GerarQueryDoRelatorio(int? relId)
        {
            var relatorioPersonalizado = FindByIdFullLoaded(relId, true, true, true, false, true);

            MontagemQueryDTO montagem = _sqlDinamicoSRV.GerarQueryDoRelatorio(relatorioPersonalizado);

            return montagem;

        }

        public ResultadoQueryDTO ExecutarQueryDinamica(int? relId, IEnumerable<DadosDeFiltroDTO> lstFiltros = null, int? top = null)
        {
            var relatorioPersonalizado = FindByIdFullLoaded(relId, true, true, true, false, true);
            return _sqlDinamicoSRV.ExecutarQueryDinamica(relatorioPersonalizado, lstFiltros, top); 
        }

        public void PreencherRelatorioPai(RelatorioPersonalizadoDTO relPersonalizado)
        {
            if (relPersonalizado != null && relPersonalizado.REL_ID_PAI != null)
            {
                relPersonalizado.RelatorioPai = FindByIdFullLoaded(relPersonalizado.REL_ID_PAI, true, true, true, true, true);
            }
        }

        public Pagina<RelatorioPersonalizadoDTO> ListarRelatorioPersonalizadoBase(string usuario = null, int pagina = 1, int registrosPorPagina = 5)
        {
            return _dao.ListarRelatorioPersonalizadoBase(usuario, pagina, registrosPorPagina);
        }

        public Pagina<RelatorioPersonalizadoDTO> ListarRelatorioPersonalizado(string usuario = null, int pagina = 1, int registrosPorPagina = 5)
        {
            return _dao.ListarRelatorioPersonalizado(usuario, pagina, registrosPorPagina);
        }

        public void ExcluirRelatorioPersonalizado(int? relId)
        {
            var relPersonalizado = FindById(relId);
            relPersonalizado.DATA_EXCLUSAO = DateTime.Now;

            SaveOrUpdate(relPersonalizado);
        }


        /// <summary>
        /// Executa a query Dinâmica. Salva em uma planilha e retorna o Path da Planilha.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="relId"></param>
        /// <param name="lstFiltros"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public string ExecutarQueryDinamicaPlanilha(string path, int? relId,IEnumerable<DadosDeFiltroDTO> lstFiltros = null, int? top = null)
        {
            
            try
            {
                var resultado = ExecutarQueryDinamica(relId, lstFiltros, top);
                var relatorio = FindById(relId);
                if(resultado != null)
                {
                    string descricaoRelatorio = null;
                    if(relatorio != null && !string.IsNullOrWhiteSpace(relatorio.REL_DESCRICAO))
                    {
                        descricaoRelatorio = relatorio.REL_DESCRICAO;
                        descricaoRelatorio = StringUtil.LimparAcentuacao(descricaoRelatorio);
                        descricaoRelatorio = StringUtil.RetirarCaractereEspecialComTrim(descricaoRelatorio, false, false);
                        descricaoRelatorio = descricaoRelatorio.Replace(" ", "_");
                    }

                    var fileName = string.Format(@"{0}download\relatorio_{1}_{2:yyyy-MM-dd hh-mm-ss}.xlsx", path, descricaoRelatorio, DateTime.Now);

                    using (ExcelProxyOpenXML excelLoad = new ExcelProxyOpenXML())
                    {
                        excelLoad.ToSheet(fileName, resultado.Dados.ToList());
                    };

                    if (File.Exists(fileName))
                        return fileName;

                }
                return null;
                }
                catch (Exception e)
                {
                    throw new Exception("Não é possível gerar a planilha", e);
                }
        }

        public FileInfoDTO RetornarPlanilha(string fileName)
        {

            var bytes = File.ReadAllBytes(fileName);
            var downloadInfo = new FileInfoDTO()
            {
                Path = fileName,
                Bytes = bytes
            };

            File.Delete(fileName);
            return downloadInfo;
        }


    }
}
