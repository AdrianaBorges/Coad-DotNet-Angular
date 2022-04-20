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
    [ServiceConfig("REJ_ID")]
    public class RelatorioJoinSRV : GenericService<RELATORIO_JOIN, RelatorioJoinDTO, int>
    {
        private RelatorioJoinDAO _dao { get; set; }
        public RelatorioPersonalizadoSRV RelatorioPersonalizadoSRV { get; set; }

        public RelatorioJoinSRV()
        {
            this.Dao = _dao;
        }

        public RelatorioJoinSRV(RelatorioJoinDAO _dao)
        {
            this.Dao = _dao;
            this._dao = _dao;
        }

        public IList<RelatorioJoinDTO> ListarRelatorioJoinPorRelId(int? relId)
        {
            return _dao.ListarRelatorioJoinPorRelId(relId);
        }

        public IList<RelatorioJoinDTO> ListarRelatorioJoinPorTabelas(int? retId)
        {
            return _dao.ListarRelatorioJoinPorTabelas(retId);
        }

        public void PreencherRelatorioJoin(RelatorioPersonalizadoDTO relatorioPerso)
        {
            if (relatorioPerso != null)
            {
                relatorioPerso.RELATORIO_JOIN = ListarRelatorioJoinPorRelId(relatorioPerso.REL_ID);
            }
        }

        public void ChecarExcluirRelatorioJoinAusentes(RelatorioPersonalizadoDTO relatorio)
        {
            if (relatorio != null)
            {
                
            var relPersonalizadoSRV = ServiceFactory.RetornarServico<RelatorioPersonalizadoSRV>();

            var objBanco = relPersonalizadoSRV.FindByIdFullLoaded(relatorio.REL_ID, preecherJoins: true);
            var lstParaExcluir = GetMissinList(objBanco.RELATORIO_JOIN, relatorio.RELATORIO_JOIN);

            MarcarRelatorioJoinComoExcluido(lstParaExcluir);
            }
        }

        public void MarcarRelatorioJoinDasTabelasComoExcluido(IEnumerable<RelatorioTabelasDTO> lstRelatorioTabelas)
        {
            if (lstRelatorioTabelas != null)
            {
                IList<RelatorioJoinDTO> lstRelatorioJoin = new List<RelatorioJoinDTO>();

                foreach (var relTab in lstRelatorioTabelas)
                {
                    var lstRelatorioJoinEncontrado = ListarRelatorioJoinPorTabelas(relTab.RET_ID);

                    foreach (var join in lstRelatorioJoinEncontrado)
                    {
                        join.DATA_EXCLUSAO = DateTime.Now;
                        lstRelatorioJoin.Add(join);
                    }
                }

                SaveOrUpdateAll(lstRelatorioJoin);
            }
        }

        public void MarcarRelatorioJoinComoExcluido(IEnumerable<RelatorioJoinDTO> lstRelatoriosJoin)
        {
            if (lstRelatoriosJoin != null)
            {
                foreach (var relJoin in lstRelatoriosJoin)
                {
                    relJoin.DATA_EXCLUSAO = DateTime.Now;
                }

                SaveOrUpdateAll(lstRelatoriosJoin);
            }
        }

        public void SalvarRelatorioJoin(RelatorioPersonalizadoDTO relatorio)
        {
            if (relatorio != null && relatorio.RELATORIO_JOIN != null)
            {
                var lstJoin = relatorio.RELATORIO_JOIN;

                foreach (var join in lstJoin)
                {
                    if (join.REL_ID == null)
                        join.REL_ID = relatorio.REL_ID;

                    if (join.RET_ID1 == null && join.RELATORIO_TABELAS != null)
                    {
                            if (join.RELATORIO_TABELAS.RET_ID != null)
                            {
                                join.RET_ID1 = join.RELATORIO_TABELAS.RET_ID;
                            }
                            else
                            {
                                var relatorioTab = relatorio
                                    .RELATORIO_TABELAS
                                    .Where(x => x.RET_NOME_TABELA == join.RELATORIO_TABELAS.RET_NOME_TABELA && x.RET_ID != null)
                                    .FirstOrDefault();

                                if (relatorioTab != null)
                                {
                                    join.RELATORIO_TABELAS = relatorioTab;
                                    join.RET_ID1 = relatorioTab.RET_ID;
                                }
                            }
                        
                    }

                    if (join.RET_ID2 == null && join.RELATORIO_TABELAS1 != null)
                    {
                            if (join.RELATORIO_TABELAS1.RET_ID != null)
                            {
                                join.RET_ID2 = join.RELATORIO_TABELAS1.RET_ID;
                            }
                            else
                            {
                                var relatorioTab = relatorio
                                    .RELATORIO_TABELAS
                                    .Where(x => x.RET_NOME_TABELA == join.RELATORIO_TABELAS1.RET_NOME_TABELA && x.RET_ID != null)
                                    .FirstOrDefault();

                                if (relatorioTab != null)
                                {
                                    join.RELATORIO_TABELAS1 = relatorioTab;
                                    join.RET_ID2 = relatorioTab.RET_ID;
                                }
                            }                       
                    }

                    //if (join.RET_ID2 == null)
                    //    join.RET_ID2 = join.RELATORIO_TABELAS1.RET_ID;
                }

                ChecarExcluirRelatorioJoinAusentes(relatorio);
                SaveOrUpdateAll(lstJoin);
            }
        }
    }
}
