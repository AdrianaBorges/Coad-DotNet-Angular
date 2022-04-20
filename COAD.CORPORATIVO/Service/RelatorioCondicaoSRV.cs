
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
    [ServiceConfig("REC_ID")]
    public class RelatorioCondicaoSRV : GenericService<RELATORIO_CONDICAO, RelatorioCondicaoDTO, int>
    {
        private RelatorioCondicaoDAO _dao;
        //public RelatorioPersonalizadoSRV RelatorioPersonalizado { get; set; }
        public RelatorioCondicaoRelatorioOperadorCondicionalSRV _relCondOperaCondicionalSRV { get; set; }

        public RelatorioCondicaoSRV(RelatorioCondicaoDAO _dao)
        {
            Dao = _dao;
            this._dao = _dao;
        }


        public IList<RelatorioCondicaoDTO> ListarRelatorioCondicao(int? relId)
        {
            return _dao.ListarRelatorioCondicao(relId);
        }

        public void PreencherRelatorioCondicao(RelatorioPersonalizadoDTO relatorioPersonalizado, bool preencherRelatorioCondicaoOperadorLogico = false)
        {
            if (relatorioPersonalizado != null)
            {
                relatorioPersonalizado.RELATORIO_CONDICAO = ListarRelatorioCondicao(relatorioPersonalizado.REL_ID);
                _relCondOperaCondicionalSRV.PreencherRelatorioCondicaoOperadorCondicial(relatorioPersonalizado.RELATORIO_CONDICAO);
            }
        }

        public void ChecarExcluirRelatorioCondicaoAusentes(RelatorioPersonalizadoDTO relatorio)
        {
            if (relatorio != null)
            {

                var relPersonalizadoSRV = ServiceFactory.RetornarServico<RelatorioPersonalizadoSRV>();

                var objBanco = relPersonalizadoSRV.FindByIdFullLoaded(relatorio.REL_ID, preecherRelatorioCondicao: true);

                var excludeList = GetMissinList(objBanco.RELATORIO_CONDICAO, relatorio.RELATORIO_CONDICAO);
                MarcarRelatorioCondicaoComoDeletado(excludeList);

                //ExcluirList<RelatorioPersonalizadoDTO>(relatorio, objBanco, "RELATORIO_CONDICAO");
            }
        }

        public void SalvarRelatorioCondicao(RelatorioPersonalizadoDTO relatorioPersonalizado)
        {
            if (relatorioPersonalizado != null)
            {
                var lstRelatorioCondicao = relatorioPersonalizado.RELATORIO_CONDICAO;

                foreach (var relatorioCondicao in lstRelatorioCondicao)
                {
                    if (relatorioCondicao.REL_ID == null)
                        relatorioCondicao.REL_ID = relatorioPersonalizado.REL_ID;

                    if (relatorioCondicao.RET_ID == null && relatorioCondicao.RELATORIO_TABELAS != null)
                    {
                        if (relatorioCondicao.RELATORIO_TABELAS.RET_ID != null)
                        {
                            relatorioCondicao.RET_ID = relatorioCondicao.RELATORIO_TABELAS.RET_ID;
                        }
                        else
                        {
                            var relatorioTab = relatorioPersonalizado
                               .RELATORIO_TABELAS
                               .Where(x => relatorioCondicao.RELATORIO_TABELAS.RET_NOME_TABELA == x.RET_NOME_TABELA)
                               .FirstOrDefault();

                            if (relatorioTab != null)
                            {
                                relatorioCondicao.RET_ID = relatorioTab.RET_ID;
                                relatorioCondicao.RELATORIO_TABELAS = relatorioTab;
                            }
                        }
                    }

                    ChecarExcluirRelatorioCondicaoAusentes(relatorioPersonalizado);
                    var relaCondSalvo = SaveOrUpdate(relatorioCondicao);

                    var index = 0;
                    
                    if(relaCondSalvo != null){
                            if (relatorioCondicao.REC_ID == null)
                                relatorioCondicao.REC_ID = relaCondSalvo.REC_ID;
                         

                        _relCondOperaCondicionalSRV.SalvarRelatorioCondicaoOperadorCondicional(relatorioCondicao);
                    }
                }
            }
        }

        public RelatorioCondicaoDTO FindByIdFullLoaded(int? recId, bool preencherRelatorioCondicaoOperadorCondicional = false)
        {
            var relCond = FindById(recId);

            if (preencherRelatorioCondicaoOperadorCondicional)
                _relCondOperaCondicionalSRV.PreencherRelatorioCondicaoOperadorCondicional(relCond);

            return relCond;
        }

        public IList<RelatorioCondicaoDTO> ListarRelatorioCondicaoPorTabela(int? retId)
        {
            return _dao.ListarRelatorioCondicaoPorTabela(retId);
        }

        public void ExcluirCondicaoDaTabela(IEnumerable<RelatorioTabelasDTO> lstRelatorioTabelas, RelatorioPersonalizadoDTO relPersonalizado)
        {
            if (lstRelatorioTabelas != null)
            {
                IList<RelatorioCondicaoDTO> lstRelCondicao = new List<RelatorioCondicaoDTO>();

                if (relPersonalizado != null && relPersonalizado.RELATORIO_CONDICAO != null)
                {
                    // retira do relatório personalizado as condições da tabela que será excluída
                    relPersonalizado.RELATORIO_CONDICAO = relPersonalizado
                        .RELATORIO_CONDICAO
                        .Where(x => !lstRelatorioTabelas.Select(sel => sel.RET_ID).Contains(x.RET_ID))
                        .ToList();
                }

                foreach (var relTb in lstRelatorioTabelas)
                {
                    var lstRelCondicaoRetornado = ListarRelatorioCondicaoPorTabela(relTb.RET_ID);
                    lstRelCondicao = lstRelCondicao
                        .Concat(lstRelCondicaoRetornado)
                        .ToList();                    
                }

                MarcarRelatorioCondicaoComoDeletado(lstRelCondicao);
            }
        }

        public void MarcarRelatorioCondicaoComoDeletado(IEnumerable<RelatorioCondicaoDTO> lstRelatorioCondicao)
        {
            foreach (var relCon in lstRelatorioCondicao)
            {
                relCon.DATA_EXCLUSAO = DateTime.Now;
            }

            SaveOrUpdateAll(lstRelatorioCondicao);
        }
    }
}