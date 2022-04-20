
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
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("REO_ID")]
    public class RelatorioCondicaoRelatorioOperadorCondicionalSRV : GenericService<RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL, RelatorioCondicaoRelatorioOperadorCondicionalDTO, int>
    {
        private RelatorioCondicaoRelatorioOperadorCondicionalDAO _dao;

        public RelatorioCondicaoRelatorioOperadorCondicionalSRV(RelatorioCondicaoRelatorioOperadorCondicionalDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }

        public IList<RelatorioCondicaoRelatorioOperadorCondicionalDTO> ListarRelatorioCondicaoOperadorCondicionalPorRecId(int? recId)
        {
            return _dao.ListarRelatorioCondicaoOperadorCondicionalPorRecId(recId);
        }

        public void PreencherRelatorioCondicaoOperadorCondicional(RelatorioCondicaoDTO relatorioCondicao)
        {
            if (relatorioCondicao != null)
            {
                relatorioCondicao.RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL = ListarRelatorioCondicaoOperadorCondicionalPorRecId(relatorioCondicao.REC_ID);
            }
        }

        public void PreencherRelatorioCondicaoOperadorCondicial(ICollection<RelatorioCondicaoDTO> lstRelatorioCondicao)
        {
            if (lstRelatorioCondicao != null)
            {
                foreach (var relCond in lstRelatorioCondicao)
                {
                    PreencherRelatorioCondicaoOperadorCondicional(relCond);
                }
            }
        }

        public void SalvarRelatorioCondicaoOperadorCondicional(RelatorioCondicaoDTO relatorioCondicao)
        {
            if (relatorioCondicao != null)
            {
                var lstRelatocioCondicaoOperadorCond = relatorioCondicao.RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL;

                if (lstRelatocioCondicaoOperadorCond != null)
                {
                    foreach (var relConOp in lstRelatocioCondicaoOperadorCond)
                    {
                        if (relConOp.REC_ID == null)
                            relConOp.REC_ID = relatorioCondicao.REC_ID;

                        if (relConOp.REC_ID == null && relConOp.RELATORIO_OPERADOR_CONDICIONAL != null)
                            relConOp.REC_ID = relConOp.RELATORIO_OPERADOR_CONDICIONAL.ROC_ID;
                        
                    }

                    ChecarExcluirRelatorioCondicaoOperadorCondicionalAusentes(relatorioCondicao);
                    SaveOrUpdateAll(lstRelatocioCondicaoOperadorCond);
                }
            }
        }

        public void ChecarExcluirRelatorioCondicaoOperadorCondicionalAusentes(RelatorioCondicaoDTO relatorio)
        {
            if (relatorio != null)
            {
                var relCondicaoSRV = ServiceFactory.RetornarServico<RelatorioCondicaoSRV>();
                var objBanco = relCondicaoSRV.FindByIdFullLoaded(relatorio.REC_ID, true);
                ExcluirList<RelatorioCondicaoDTO>(relatorio, objBanco, "RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL");
            }
        }
    }
}