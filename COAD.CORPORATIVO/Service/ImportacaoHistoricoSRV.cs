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
using COAD.SEGURANCA.Service.Custons.Context;
using GenericCrud.Exceptions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("IMH_ID")]
    public class ImportacaoHistoricoSRV : GenericService<IMPORTACAO_HISTORICO, ImportacaoHistoricoDTO, int>
    {
        private ImportacaoHistoricoDAO _dao;

        public ImportacaoHistoricoSRV(ImportacaoHistoricoDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }

        public ImportacaoHistoricoSRV()
        {
            _dao = new ImportacaoHistoricoDAO();
            Dao = _dao;
        }

        public void IncluirHistoricoImportacao(string descricao, ImportacaoDTO importacao, BatchContext batchContext = null, int? repID = null, string usuLogin = null)
        {
            if(importacao != null)
            {
                var importacaoHistorico = new ImportacaoHistoricoDTO()
                {
                    IMH_DESCRICAO = descricao,
                    IMH_DATA = DateTime.Now,
                    IMS_ID = importacao.IMS_ID,
                    IMP_ID = importacao.IMP_ID,
                    IMH_TOTAL_DUPLICADO = importacao.IMP_QTD_SUS_DUPLICADA,
                    IMH_TOTAL_PROCESSADO = importacao.IMP_QTD_REAL_SUS,
                    IMH_TOTAL_SUSPECTS = importacao.IMP_QTD_SUS_TOTAL,
                    IMH_HISTORICO_DA_IMPORTACAO = true,
                    REP_ID = repID,
                    USU_LOGIN = usuLogin
                };

                if(batchContext != null)
                {
                    importacaoHistorico.IMH_TOTAL_FALHA = batchContext.TotalFalha;
                    importacaoHistorico.IMH_TOTAL_SUCESSO = batchContext.TotalExito;
                }

                Save(importacaoHistorico);
            }
        }

        public void IncluirHistoricoImportacaoSuspect(string descricao, ImportacaoSuspectDTO importacaoSuspect)
        {
            if (importacaoSuspect != null)
            {
                var importacaoHistorico = new ImportacaoHistoricoDTO()
                {
                    IMH_DESCRICAO = descricao,
                    IMH_DATA = DateTime.Now,
                    IPS_ID = importacaoSuspect.IPS_ID,
                    IMS_ID = importacaoSuspect.IMS_ID,
                    IMP_ID = importacaoSuspect.IMP_ID
                };

                Save(importacaoHistorico);
            }
        }

        public void IncluirHistoricoErroImportacaoSuspect(ImportacaoSuspectDTO importacaoSuspect, Exception e)
        {

            if (importacaoSuspect != null && e != null)
            {
                var descricao = "Ocorreu um erro ao processar a importação do Suspect/Cliente. Veja os Detalhes... \n <br /> {0}";

                var descricaoEx = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                descricao = string.Format(descricao, descricaoEx);

                var importacaoHistorico = new ImportacaoHistoricoDTO()
                {
                    IMH_DESCRICAO = descricao,
                    IMH_DATA = DateTime.Now,
                    IPS_ID = importacaoSuspect.IPS_ID,
                    IMS_ID = importacaoSuspect.IMS_ID,
                    IMP_ID = importacaoSuspect.IMP_ID,
                    IMH_ERRO = true
                };

                Save(importacaoHistorico);
            }
        }

        public void IncluirHistoricoErroImportacao(ImportacaoDTO importacao, Exception e, string mensagemInicial = null)
        {

            if (importacao != null && e != null)
            {
                var descricao = (string.IsNullOrWhiteSpace(mensagemInicial)) ? mensagemInicial : "Ocorreu um erro ao processar a importação.";
                descricao += "Veja os Detalhes... \n <br /> {0}";

                var descricaoEx = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                descricao = string.Format(descricao, descricaoEx);

                var importacaoHistorico = new ImportacaoHistoricoDTO()
                {
                    IMH_DESCRICAO = descricao,
                    IMH_DATA = DateTime.Now,
                    IMS_ID = importacao.IMS_ID,
                    IMP_ID = importacao.IMP_ID,
                    IMH_HISTORICO_DA_IMPORTACAO = true,
                    IMH_ERRO = true,                    
                };

                Save(importacaoHistorico);
            }
        }

        public Pagina<ImportacaoHistoricoDTO> PesquisarHistorico(
            DateTime? dataInicial, 
            DateTime? dataFinal,
            int? impID,
            int? ipsID,
            int? imsID, 
            int pagina = 1, 
            int registroPorPagina = 6)
        {
            return _dao.PesquisarHistorico(dataFinal, dataFinal, impID, ipsID, imsID, pagina, registroPorPagina);
        }

        public ImportacaoHistoricoDTO BuscarUltimoHistoricoDeErro(
            int? ipsID)
        {
            return _dao.BuscarUltimoHistoricoDeErro(ipsID);
        }

        public ICollection<ImportacaoHistoricoDTO> BuscarUltimosHistoricosDeErroDaImportacao(
            int? impID)
        {
            return _dao.BuscarUltimosHistoricosDeErroDaImportacao(impID);
        }

    }
}
