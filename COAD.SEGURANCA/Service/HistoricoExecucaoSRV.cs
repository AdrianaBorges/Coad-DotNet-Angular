using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using GenericCrud.Exceptions;
using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Exceptions;
using Coad.GenericCrud.ActionResultTools;

namespace COAD.SEGURANCA.Service
{
    /// <summary>
    /// Salva as execuções de Job
    /// Todos os salvamentos dos métodos foram comentados. Essa tabela está crescendo exponencialmente. 
    /// Deve-se escolher outra estratégia de salvar os dados de execução
    /// </summary>
    [ServiceConfig("HIE_ID")]
    public class HistoricoExecucaoSRV : GenericService<HISTORICO_EXECUCAO, HistoricoExecucaoDTO, int>
    {
        private HistoricoExecucaoDAO _dao;

        public HistoricoExecucaoSRV()
        {
            this._dao = new HistoricoExecucaoDAO();
            this.Dao = _dao;
        }

        public HistoricoExecucaoSRV(HistoricoExecucaoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }



        public void Incluir(string nomeDaExecucao, string descricao, DateTime data, string nomeServico, string nomeProjeto, Exception e = null)
        {
            string nomeExcecao = null;
            string descricaoEx = null;
            string stackTrace = null;

            if (e != null)
            {
                nomeExcecao = e.GetType().FullName;
                descricaoEx = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                stackTrace = e.StackTrace.ToString();

            }

            /*
            HistoricoExecucaoDTO historicoEx = new HistoricoExecucaoDTO()
            {
                HIE_NOME = nomeDaExecucao.Substring(0, (nomeDaExecucao.Length > 150 ? 150 : nomeDaExecucao.Length)),
                HIE_DESCRICAO = descricao,
                HIE_DATA_EXECUCAO = data,
                HIE_ERRO_NOME = nomeExcecao.Substring(0, (nomeExcecao.Length > 150 ? 150 : nomeExcecao.Length)),
                HIE_ERRO_DESCRICAO = descricaoEx,
                HIE_NOME_SERVICO = nomeServico.Substring(0, (nomeServico.Length > 120 ? 120 : nomeServico.Length)),
                HIE_PROJETO = nomeProjeto.Substring(0, (nomeProjeto.Length > 120 ? 120 : nomeProjeto.Length)),
                HIE_STACK_TRACE = stackTrace                    

            };

            Save(historicoEx);
            */
        }

        public void Incluir(BatchJobException e)
        {
            string nomeExcecao = null;
            string descricaoEx = null;
            string stackTrace = null;
            bool? erro = true;
            var ex = e.reg.exception;

            if (e != null)
            {
                nomeExcecao = e.GetType().FullName;
                descricaoEx = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                stackTrace = e.StackTrace.ToString();
            }

            HistoricoExecucaoDTO historicoEx = new HistoricoExecucaoDTO()
            {
                HIE_NOME = e.reg.nomeDaExecucao,
                HIE_DESCRICAO = e.reg.descricao,
                HIE_DATA_EXECUCAO = (e.reg.data != null) ? e.reg.data : DateTime.Now,
                HIE_ERRO_NOME = nomeExcecao,
                HIE_ERRO_DESCRICAO = descricaoEx,
                HIE_NOME_SERVICO = e.reg.nomeServico,
                HIE_PROJETO = e.reg.nomeProjeto,
                HIE_STACK_TRACE = stackTrace,
                HIE_CORREU_ERRO = erro,
                HIE_ID_REF = e.reg.codReferencia,
                TPJ_ID = e.reg.codTipoJob
            };

            //Save(historicoEx);
        }

        public void Incluir(RegistroNotificacaoSistemaDTO hist)
        {
            string nomeExcecao = null;
            string descricaoEx = null;
            string stackTrace = null;
            bool? erro = hist.erro;
            var e = hist.exception;

            if (e != null)
            {
                nomeExcecao = e.GetType().FullName;
                //descricaoEx = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                stackTrace = e.StackTrace.ToString();
                hist.erro = true;

                var message = Message.Fail("Ocorreu um erro de validação");
                descricaoEx = ExceptionFormatter.RecursiveFindExceptionsMessage(e, message);
                
                if (message != null)
                {
                    StringBuilder sb = new StringBuilder(" Erros de validação: ->");
                    foreach (var men in message.subMessages)
                    {
                        if (men.ValidationErrors != null && men.ValidationErrors.Count() > 0)
                        {
                            foreach(var key in men.ValidationErrors.Keys)
                            {
                                sb.Append("(");
                                sb.Append(key);
                                sb.Append(") ====> [ ");

                                int index = 0;
                                var list = men.ValidationErrors[key];
                                foreach (var validation in list)
                                {
                                    sb.Append(validation);
                                    if (index > 0)
                                        sb.Append(", ");
                                    index++;
                                }
                                sb.Append(" ] ");

                            }
                            descricaoEx += sb.ToString();
                            //lstValidacoes = lstValidacoes.Concat(men.ValidationErrors).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                }
            }

            HistoricoExecucaoDTO historicoEx = new HistoricoExecucaoDTO()
            {
                HIE_NOME = hist.nomeDaExecucao,
                HIE_DESCRICAO = hist.descricao,
                HIE_DATA_EXECUCAO = (hist.data != null) ? hist.data : DateTime.Now,
                HIE_ERRO_NOME = nomeExcecao,
                HIE_ERRO_DESCRICAO = descricaoEx,
                HIE_NOME_SERVICO = hist.nomeServico,
                HIE_PROJETO = hist.nomeProjeto,
                HIE_STACK_TRACE = stackTrace,
                HIE_CORREU_ERRO = erro,
                HIE_ID_REF = hist.codReferencia,
                TPJ_ID = hist.codTipoJob,
                HIE_ID_DESC = hist.descricaoCodigoReferencia,
                HIE_ID_STR_REF = hist.codReferenciaStr,
                NTS_ID = hist.codNotificacaoSistema
            };  

            //Save(historicoEx);
        }
    }
}
