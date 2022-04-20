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
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("RET_ID")]
    public class RelatorioTabelasSRV : GenericService<RELATORIO_TABELAS, RelatorioTabelasDTO, int>
    {

        private RelatorioTabelasDAO _dao { get; set; }
        public RelatorioTabelaColunasSRV _relTabColSRV { get; set; }
        public RelatorioJoinSRV _relatorioJoinSRV { get; set; }
        public RelatorioTabelaColunasSRV _relTabelaColunasSRV { get; set; }
        public RelatorioCondicaoSRV _relCondicaoSRV { get; set; }
        //private RelatorioPersonalizadoSRV RelPersonalizadoSRV { get; set; }
        
        public RelatorioTabelasSRV(RelatorioTabelasDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public IList<RelatorioTabelasDTO> ListarRelatorioTabelasPorRelId(int? relId)
        {
            return _dao.ListarRelatorioTabelasPorRelId(relId);
        }


        public void PreencherListaRelatorioTabelas(RelatorioPersonalizadoDTO relPer, bool preencherRelatorioTabelaColunas = false)
        {
            if (relPer != null && relPer.REL_ID != null)
            {
                relPer.RELATORIO_TABELAS = ListarRelatorioTabelasPorRelId(relPer.REL_ID);
                if (preencherRelatorioTabelaColunas)
                {
                    _relTabColSRV.PreencherRelatorioTabelaColunas(relPer.RELATORIO_TABELAS);
                }
            }
        }

        public RelatorioTabelasDTO FindByIdFullLoaded(int? retInd, bool preencherRelatorioTabelaColunas = false)
        {
            var relTab = FindById(retInd);

            if (preencherRelatorioTabelaColunas)
                _relTabColSRV.PreencherRelatorioTabelaColunas(relTab);

            return relTab;
        }

        private void ChecarExcluirRelatorioTabelasAusentes(RelatorioPersonalizadoDTO relatorioPersonalizado)
        {
            var relPersonalizadoSRV = ServiceFactory.RetornarServico<RelatorioPersonalizadoSRV>();

            if (relatorioPersonalizado != null)
            {
                var objetoDoBanco = relPersonalizadoSRV.FindByIdFullLoaded(relatorioPersonalizado.REL_ID, true, true);
                var lstParaExcluir = GetMissinList(objetoDoBanco.RELATORIO_TABELAS, relatorioPersonalizado.RELATORIO_TABELAS);
                
                _relatorioJoinSRV.MarcarRelatorioJoinDasTabelasComoExcluido(lstParaExcluir);
                _relTabelaColunasSRV.ExcluirColunasDasTabelas(lstParaExcluir);
                _relCondicaoSRV.ExcluirCondicaoDaTabela(lstParaExcluir, relatorioPersonalizado);
                MarcarRelatorioTabelasComoExcluido(lstParaExcluir);
            }
        }


        private void MarcarRelatorioTabelasComoExcluido(IEnumerable<RelatorioTabelasDTO> lstRelatorioTab)
        {
            if (lstRelatorioTab != null)
            {
                foreach (var relTab in lstRelatorioTab) 
                {
                    relTab.DATA_EXCLUSAO = DateTime.Now;
                }

                SaveOrUpdateAll(lstRelatorioTab);
            }
        }

        public void SalvarRelatorioTabelas(RelatorioPersonalizadoDTO rel)
        {
            var lstRelatoriosTabelas = rel.RELATORIO_TABELAS;

            if (lstRelatoriosTabelas != null)
            {
                CheckAndAssignKeyFromParentToChildsList(rel, lstRelatoriosTabelas, "REL_ID");

                ChecarExcluirRelatorioTabelasAusentes(rel);

                var lstRelatorioTabelaSalvo = SaveOrUpdateAll(lstRelatoriosTabelas).ToList();

                int index = 0;
                foreach (var relTab in lstRelatoriosTabelas)
                {
                    if (relTab.RET_ID == null)
                        relTab.RET_ID = lstRelatorioTabelaSalvo[index].RET_ID;

                    index++;
                }
            }
        }

    }
}
