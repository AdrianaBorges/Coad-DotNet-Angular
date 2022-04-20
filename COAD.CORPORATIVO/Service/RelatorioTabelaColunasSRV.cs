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
    [ServiceConfig("COR_ID")]
    public class RelatorioTabelaColunasSRV : GenericService<RELATORIO_TABELA_COLUNAS, RelatorioTabelaColunasDTO, int>
    {
        private RelatorioTabelaColunasDAO _dao;
        
        public RelatorioTabelaColunasSRV()
        {
            this.Dao = _dao;
        }

        public RelatorioTabelaColunasSRV(RelatorioTabelaColunasDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }


        public IList<RelatorioTabelaColunasDTO> ListarRelatorioTabelaColunasPorRetId(int? retId)
        {
            return _dao.ListarRelatorioTabelaColunasPorRetId(retId);
        }

        public IList<RelatorioTabelaColunasDTO> ListarRelatorioTabelaColunasPorRelatorioPersonalizado(int? relId)
        {
            return _dao.ListarRelatorioTabelaColunasPorRelatorioPersonalizado(relId);
        }

        public void PreencherRelatorioTabelaColunas(ICollection<RelatorioTabelasDTO> lstRelatorioTabelas)
        {
            if (lstRelatorioTabelas != null)
            {
                foreach (var relTab in lstRelatorioTabelas)
                {
                    if (relTab.RET_ID != null)
                    {
                        relTab.RELATORIO_TABELA_COLUNAS = ListarRelatorioTabelaColunasPorRetId(relTab.RET_ID);
                    }
                }
            }
        }

        public void PreencherRelatorioTabelaColunas(RelatorioTabelasDTO relatorioTabelas)
        {
            PreencherRelatorioTabelaColunas(new HashSet<RelatorioTabelasDTO>() { relatorioTabelas });
        }

        public void PreencherRelatorioTabelaColunas(ICollection<RelatorioPersonalizadoDTO> lstRelatorioPersonalizado)
        {
            if (lstRelatorioPersonalizado != null)
            {
                foreach (var relTab in lstRelatorioPersonalizado)
                {
                    if (relTab.REL_ID != null)
                    {
                        relTab.RELATORIO_TABELA_COLUNAS = ListarRelatorioTabelaColunasPorRelatorioPersonalizado(relTab.REL_ID);
                    }
                }
            }
        }

        public void PreencherRelatorioTabelaColunas(RelatorioPersonalizadoDTO relatorioPersonalizado)
        {
            PreencherRelatorioTabelaColunas(new HashSet<RelatorioPersonalizadoDTO>() { relatorioPersonalizado });
        }

        public void SalvarRelatorioTabelaColunas(ICollection<RelatorioPersonalizadoDTO> lstRelatorioTabelas)
        {
            if (lstRelatorioTabelas != null)
            {
                foreach (var relTab in lstRelatorioTabelas)
                {
                    SalvarRelatorioTabelaColunas(relTab);
                }
            }
        }

        public void SalvarRelatorioTabelaColunas(RelatorioPersonalizadoDTO relatorioPersonalizadoDTO)
        {
            if (relatorioPersonalizadoDTO != null && relatorioPersonalizadoDTO.RELATORIO_TABELA_COLUNAS != null)
            {
                var lstRelatorioTabelaColunas = relatorioPersonalizadoDTO.RELATORIO_TABELA_COLUNAS;

                foreach (var relTabelaColunas in lstRelatorioTabelaColunas)
                {
                    if (relTabelaColunas.REL_ID == null)
                        relTabelaColunas.REL_ID = relatorioPersonalizadoDTO.REL_ID;

                    if (relTabelaColunas.RET_ID == null && relTabelaColunas.RELATORIO_TABELAS != null)
                    {
                        if (relTabelaColunas.RELATORIO_TABELAS.REL_ID != null)
                        {
                            relTabelaColunas.RET_ID = relTabelaColunas.RELATORIO_TABELAS.REL_ID;
                        }
                        else
                        {
                            var relatorioTab = relatorioPersonalizadoDTO
                                .RELATORIO_TABELAS
                                .Where(x => x.RET_NOME_TABELA == relTabelaColunas.RELATORIO_TABELAS.RET_NOME_TABELA && x.RET_ID != null)
                                .FirstOrDefault();

                            if (relatorioTab != null)
                            {
                                relTabelaColunas.RELATORIO_TABELAS = relatorioTab;
                                relTabelaColunas.RET_ID = relatorioTab.RET_ID;
                            }
                        }
                    }
                }

                ChecarExcluirRelatorioTabelasColunasAusentes(relatorioPersonalizadoDTO);
                SaveOrUpdateAll(lstRelatorioTabelaColunas);
            }
        }

        public void ChecarExcluirRelatorioTabelasColunasAusentes(RelatorioPersonalizadoDTO relatorioPersonalizado)
        {
            var relPersonalizdoSRV = ServiceFactory.RetornarServico<RelatorioPersonalizadoSRV>();

            if (relatorioPersonalizado != null)
            {
                var objetoDoBanco = relPersonalizdoSRV.FindByIdFullLoaded(relatorioPersonalizado.REL_ID, false, true);
                ExcluirList<RelatorioPersonalizadoDTO>(relatorioPersonalizado, objetoDoBanco, "RELATORIO_TABELA_COLUNAS");
            }
        }

        public void ExcluirColunasDasTabelas(IEnumerable<RelatorioTabelasDTO> lstRelatorioTabelas)
        {
            if (lstRelatorioTabelas != null)
            {
                IList<RelatorioTabelaColunasDTO> lstRelatorioTabColunas = new List<RelatorioTabelaColunasDTO>();

                foreach (var relTb in lstRelatorioTabelas)
                {

                    var lstRelatorioTabColunasEncontrado = ListarRelatorioTabelaColunasPorRetId(relTb.RET_ID);

                    lstRelatorioTabColunas = lstRelatorioTabColunas
                        .Concat(lstRelatorioTabColunasEncontrado)
                        .ToList();
                }

                DeleteAll(lstRelatorioTabColunas);
            }
        }

        /// <summary>
        /// Lista as colunas do relatório personalizado substituindo o nome do campo
        /// pelo ALIAS. Deve ser utilizado para descrever as colunas de um relatório
        /// personalizado base. Ou seja, para montar um relatório derivado.
        /// </summary>
        /// <param name="retId"></param>
        /// <returns></returns>
        public IList<RelatorioTabelaColunasDTO> ListarColunasDoRelatorioFormatado(int? relId)
        {
            var lstColunas = ListarRelatorioTabelaColunasPorRelatorioPersonalizado(relId);

            lstColunas = lstColunas.Select(x => new RelatorioTabelaColunasDTO() { 
                
                COR_DESCRICAO = (!string.IsNullOrEmpty(x.COR_ALIAS) ? x.COR_ALIAS : x.COR_DESCRICAO),
                COR_ALIAS = x.COR_ALIAS,
                COR_ID = null,
                COR_IS_NULLABLE = x.COR_IS_NULLABLE,
                COR_ORDEM = x.COR_ORDEM,
                COR_ORDEM_ASC = x.COR_ORDEM_ASC,
                COR_ORDENACAO = x.COR_ORDENACAO,
                COR_TYPE_NAME = x.COR_TYPE_NAME,
                REL_ID = null,
                RET_ID = null,                
            })
            .ToList();

            return lstColunas;
        }
    }
}
