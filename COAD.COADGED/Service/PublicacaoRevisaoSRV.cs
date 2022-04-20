using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using COAD.UTIL.Grafico;
using COAD.COADCORP;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PublicacaoRevisaoSRV : GenericService<PUBLICACAO_REVISAO, PublicacaoRevisaoDTO, int>
    {
        private PublicacaoRevisaoDAO _dao = new PublicacaoRevisaoDAO();

        // serviços...
        private ColaboradorSRV _serviceColaborador = new ColaboradorSRV();
        private PublicacaoRevisaoColaboradorSRV _servicePublicacaoRevisaoColaborador = new PublicacaoRevisaoColaboradorSRV();
        private PublicacaoAreaConsultoriaSRV _servicePublicacaoAreaConsultoria = new PublicacaoAreaConsultoriaSRV();
        private PublicacaoSRV _servicePublicacao = new PublicacaoSRV();
        private PublicacaoRemissaoSRV _servicePublicacaoRemissao = new PublicacaoRemissaoSRV();
        private PublicacaoTitulacaoSRV _servicePublicacaoTitulacao = new PublicacaoTitulacaoSRV();

        public PublicacaoRevisaoSRV()
        {
            Dao = _dao;
        }

        public void SalvarAlteracaoDaMateria(PublicacaoAreaConsultoriaDTO publicacaoAreaConsultoria)
        {
            // alterou, então, registre isso agora...
            PublicacaoRevisaoColaboradorDTO revisado = new PublicacaoRevisaoColaboradorDTO();

            // quem foi o revisor responsável pela alteração?...
            var colaborador = _serviceColaborador.Colaboradores(null, SessionContext.autenticado.USU_LOGIN).lista.FirstOrDefault();
            string setor = publicacaoAreaConsultoria.revisao == "T" ? "Revisão Técnica" : publicacaoAreaConsultoria.revisao == "D" ? "Digitação" : "Revisão Ortográfica";

            // prepare para salvar...
            revisado.ARE_CONS_ID = (int)publicacaoAreaConsultoria.ARE_CONS_ID;
            revisado.PUB_ID = (int)publicacaoAreaConsultoria.PUB_ID;
            revisado.COL_ID = (int)colaborador.COL_ID;
            revisado.DATA = DateTime.Now;
            revisado.EDITOU = "S";
            revisado.REVISAO = publicacaoAreaConsultoria.revisao;
            revisado.MOTIVO = "Alterada em " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " no setor de " + setor + " pelo Colaborador " + colaborador.COL_NOME.ToString();

            // salve agora...
            _servicePublicacaoRevisaoColaborador.SalvarPublicacaoRevisaoColaborador(revisado);
        }

        public void SalvarLiberacaoParaRevisaoTecnica(int publicacaoId, int colecionadorId)
        { 
            try
            {
                // abrindo a transação...
                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    // identificando o redator que está liberando a matéria...
                    var colaborador = _serviceColaborador.Colaboradores(null, SessionContext.autenticado.USU_LOGIN).lista.FirstOrDefault();

                    if (colaborador != null)
                    {
                        // preparando o registro de liberação...
                        PublicacaoRevisaoDTO pubRevisao = new PublicacaoRevisaoDTO();

                        // já existe liberação?...
                        var publicacaoRevisao = _dao.PublicacaoRevisao(publicacaoId, colecionadorId);
                        pubRevisao.REV_ID = publicacaoRevisao.lista.Count() == 0 ? null : publicacaoRevisao.lista.FirstOrDefault().REV_ID;
                        pubRevisao.COL_ID = (int)colaborador.COL_ID;
                        pubRevisao.PUB_ID = publicacaoId;
                        pubRevisao.ARE_CONS_ID = colecionadorId;
                        pubRevisao.REV_TC = "L";
                        // salvando a liberação para a revisão técnica...
                        this.SalvarPublicacaoRevisao(pubRevisao);

                        // preparando o LOG para a aprovação técnica...
                        PublicacaoRevisaoColaboradorDTO pubRevCol = new PublicacaoRevisaoColaboradorDTO();
                        pubRevCol.PUB_ID = pubRevisao.PUB_ID;
                        pubRevCol.ARE_CONS_ID = pubRevisao.ARE_CONS_ID;
                        pubRevCol.COL_ID = (int)colaborador.COL_ID;
                        pubRevCol.DATA = DateTime.Now;
                        pubRevCol.REVISAO = "L";
                        pubRevCol.EDITOU = "N";
                        pubRevCol.MOTIVO = "Liberada em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " para a REVISÃO TÉCNICA pelo Redator " + colaborador.COL_NOME.ToString();
                        // salvando o LOG para a aprovação técnica da revisão...
                        _servicePublicacaoRevisaoColaborador.SalvarPublicacaoRevisaoColaborador(pubRevCol);

                        // guardando a matéria liberada pela RDC - ALT: 14/04/2016
                        PublicacaoDTO pub = _servicePublicacao.FindById(publicacaoId);
                        pub.PUB_CONTEUDO_RDC = pub.PUB_CONTEUDO;
                        pub.PUB_CONTEUDO_RESENHA_RDC = pub.PUB_CONTEUDO_RESENHA;
                        
                        _servicePublicacao.SalvarPublicacao(pub);

                        // commit - confirmando operação sem erros...
                        scope.Complete();
                    }
                    else
                    {
                        throw new Exception("Você não está autorizado a liberar matérias para Revisão Técnica!");
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var _erro = new FormattedDbEntityValidationException(dbEx);

                SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                throw _erro;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }

        public void SalvarAprovacaoReprovacaoDaRevisaoTecnica(int revId, string aprova, string motivo = null)
        {
            // localizando o registro a ser aprovado tecnicamente...
            var publicacaoRevisao = _dao.FindById(revId);
            if (publicacaoRevisao != null)
            {
                try
                {
                    // abrindo a transação...
                    var txOpt = new TransactionOptions();
                    txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    txOpt.Timeout = TransactionManager.MaximumTimeout;

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                    {
                        // identificando o revisor técnico que está aprovando a matéria...
                        var colaborador = _serviceColaborador.Colaboradores(null, SessionContext.autenticado.USU_LOGIN).lista.FirstOrDefault();

                        if (colaborador != null)
                        {
                            // preparando o registro de aprovação da matéria na revisão técnica...
                            PublicacaoRevisaoDTO pubRevisao = new PublicacaoRevisaoDTO();
                            pubRevisao.REV_ID = revId;
                            pubRevisao.PUB_ID = publicacaoRevisao.PUB_ID;
                            pubRevisao.ARE_CONS_ID = publicacaoRevisao.ARE_CONS_ID;
                            pubRevisao.COL_ID = publicacaoRevisao.COL_ID;
                            pubRevisao.REV_TC = aprova;
                            pubRevisao.DIG_TC = aprova == "A" ? "L" : publicacaoRevisao.DIG_TC;

                            // salvando a aprovação da matéria na revisão técnica...
                            this.SalvarPublicacaoRevisao(pubRevisao);

                            // preparando o LOG da aprovação técnica...
                            PublicacaoRevisaoColaboradorDTO pubRevCol = new PublicacaoRevisaoColaboradorDTO();
                            pubRevCol.PUB_ID = pubRevisao.PUB_ID;
                            pubRevCol.ARE_CONS_ID = pubRevisao.ARE_CONS_ID;
                            pubRevCol.COL_ID = (int)colaborador.COL_ID;
                            pubRevCol.DATA = DateTime.Now;
                            pubRevCol.REVISAO = aprova;
                            pubRevCol.EDITOU = "N";
                            pubRevCol.MOTIVO = aprova == "A" ? "Liberada em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " para a DIGITAÇÃO pelo Colaborador " + colaborador.COL_NOME.ToString() :
                                                              "Devolvida em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " para a REDAÇÃO pelo Colaborador " + colaborador.COL_NOME.ToString();
                            pubRevCol.MOTIVO = pubRevCol.MOTIVO + (motivo == null ? "" : ": \"" + motivo + "\"");

                            // salvando o LOG da aprovação técnica da revisão...
                            _servicePublicacaoRevisaoColaborador.SalvarPublicacaoRevisaoColaborador(pubRevCol);

                            // guardando a matéria aprovada pela RVT - ALT: 14/04/2016
                            PublicacaoDTO pub = _servicePublicacao.FindById(publicacaoRevisao.PUB_ID);
                            pub.PUB_CONTEUDO_RVT = (aprova == "A")? pub.PUB_CONTEUDO: null;
                            pub.PUB_CONTEUDO_RESENHA_RVT = (aprova == "A") ? pub.PUB_CONTEUDO_RESENHA : null;
                            
                            //////////////////////////////////////////////////////
                            if (aprova == "A")
                            {
                                var p = _servicePublicacaoAreaConsultoria.FindById(pubRevisao.PUB_ID, pubRevisao.ARE_CONS_ID);
                                pub.PUB_CONTEUDO_RESENHA_DGT = _servicePublicacaoAreaConsultoria.MateriaImpressaTexto(p);
                            }
                            ///////////////////////////////////////////////////////

                            _servicePublicacao.SalvarPublicacao(pub);

                            // commit - confirmando operação sem erros...
                            scope.Complete();
                        }
                        else
                        {
                            throw new Exception("Você não está autorizado a reprovar matérias na Revisão Técnica!");
                        }
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    var _erro = new FormattedDbEntityValidationException(dbEx);

                    SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                    throw _erro;
                }
                catch (Exception ex)
                {
                    throw new Exception(SysException.Show(ex));
                }
            }
        }

        public void SalvarAprovacaoReprovacaoDaDigitacao(int revId, string aprova, string motivo = null)
        {
            // localizando o registro a ser aprovado...
            var publicacaoRevisao = _dao.FindById(revId);
            if (publicacaoRevisao != null)
            {
                try
                {
                    // abrindo a transação...
                    var txOpt = new TransactionOptions();
                    txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    txOpt.Timeout = TransactionManager.MaximumTimeout;

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                    {
                        // identificando o revisor técnico que está aprovando a matéria...
                        var colaborador = _serviceColaborador.Colaboradores(null, SessionContext.autenticado.USU_LOGIN).lista.FirstOrDefault();

                        if (colaborador != null)
                        {
                            // preparando o registro de aprovação da matéria...
                            PublicacaoRevisaoDTO pubRevisao = new PublicacaoRevisaoDTO();
                            pubRevisao.REV_ID = revId;
                            pubRevisao.PUB_ID = publicacaoRevisao.PUB_ID;
                            pubRevisao.ARE_CONS_ID = publicacaoRevisao.ARE_CONS_ID;
                            pubRevisao.COL_ID = publicacaoRevisao.COL_ID;
                            pubRevisao.DIG_TC = aprova;
                            pubRevisao.REV_OR = aprova == "A" ? "L" : publicacaoRevisao.REV_OR;
                            pubRevisao.REV_TC = aprova == "R" ? "L" : publicacaoRevisao.REV_TC;

                            // salvando a aprovação da matéria na revisão técnica...
                            this.SalvarPublicacaoRevisao(pubRevisao);

                            // preparando o LOG da aprovação técnica...
                            PublicacaoRevisaoColaboradorDTO pubRevCol = new PublicacaoRevisaoColaboradorDTO();
                            pubRevCol.PUB_ID = pubRevisao.PUB_ID;
                            pubRevCol.ARE_CONS_ID = pubRevisao.ARE_CONS_ID;
                            pubRevCol.COL_ID = (int)colaborador.COL_ID;
                            pubRevCol.DATA = DateTime.Now;
                            pubRevCol.REVISAO = aprova;
                            pubRevCol.EDITOU = "N";
                            pubRevCol.MOTIVO = aprova == "A" ? "Liberada em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " para a REVISÃO ORTOGRÁFICA pelo Colaborador " + colaborador.COL_NOME.ToString() :
                                                               "Devolvida em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " para a REVISÃO TÉCNICA pelo Colaborador " + colaborador.COL_NOME.ToString();
                            pubRevCol.MOTIVO = pubRevCol.MOTIVO + (motivo == null ? "" : ": \"" + motivo + "\"");

                            // salvando o LOG da aprovação técnica da revisão...
                            _servicePublicacaoRevisaoColaborador.SalvarPublicacaoRevisaoColaborador(pubRevCol);

                            //////////////////////////////////////////////////////
                            PublicacaoDTO pub = _servicePublicacao.FindById(publicacaoRevisao.PUB_ID);
                            if (aprova == "A")
                            {
                                var p = _servicePublicacaoAreaConsultoria.FindById(pubRevisao.PUB_ID, pubRevisao.ARE_CONS_ID);
                                pub.PUB_CONTEUDO_RESENHA_RVO = pub.PUB_CONTEUDO_RESENHA_DGT; //_servicePublicacaoAreaConsultoria.MateriaImpressaTexto(p, "DGT");
                            }
                            else 
                            {
                                pub.PUB_CONTEUDO_RESENHA_DGT = null;
                            }
                            _servicePublicacao.SalvarPublicacao(pub);
                            ///////////////////////////////////////////////////////

                            // commit - confirmando operação sem erros...
                            scope.Complete();
                        }
                        else
                        {
                            throw new Exception("Você não está autorizado a reprovar matérias na Digitação!");
                        }
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    var _erro = new FormattedDbEntityValidationException(dbEx);

                    SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                    throw _erro;
                }
                catch (Exception ex)
                {
                    throw new Exception(SysException.Show(ex));
                }
            }
        }

        public void SalvarAprovacaoReprovacaoDaRevisaoOrtografica(int revId, string aprova, string motivo = null)
        {
            // localizando o registro a ser aprovado...
            var publicacaoRevisao = _dao.FindById(revId);
            if (publicacaoRevisao != null)
            {
                try
                {
                    // abrindo a transação...
                    var txOpt = new TransactionOptions();
                    txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    txOpt.Timeout = TransactionManager.MaximumTimeout;

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                    {
                        // identificando o revisor técnico que está aprovando a matéria...
                        var colaborador = _serviceColaborador.Colaboradores(null, SessionContext.autenticado.USU_LOGIN).lista.FirstOrDefault();

                        if (colaborador != null)
                        {
                            // preparando o registro de aprovação da matéria...
                            PublicacaoRevisaoDTO pubRevisao = new PublicacaoRevisaoDTO();
                            pubRevisao.REV_ID = revId;
                            pubRevisao.PUB_ID = publicacaoRevisao.PUB_ID;
                            pubRevisao.ARE_CONS_ID = publicacaoRevisao.ARE_CONS_ID;
                            pubRevisao.COL_ID = publicacaoRevisao.COL_ID;
                            pubRevisao.REV_OR = aprova;

                            // Até o dia 24/06/2016 às 16h52 - seguia o fluxo normal (RDC/RVT/DGT/RVO e vice-versa) conforme abaixo. Acima a mudança pulando a DGT no retorno da RVO.
                            pubRevisao.REV_TC = publicacaoRevisao.REV_TC;
                            pubRevisao.DIG_TC = aprova == "R" ? "L" : publicacaoRevisao.DIG_TC;

                            // salvando a aprovação da matéria na revisão técnica...
                            this.SalvarPublicacaoRevisao(pubRevisao);

                            // preparando o LOG da aprovação técnica...
                            PublicacaoRevisaoColaboradorDTO pubRevCol = new PublicacaoRevisaoColaboradorDTO();
                            pubRevCol.PUB_ID = pubRevisao.PUB_ID;
                            pubRevCol.ARE_CONS_ID = pubRevisao.ARE_CONS_ID;
                            pubRevCol.COL_ID = (int)colaborador.COL_ID;
                            pubRevCol.DATA = DateTime.Now;
                            pubRevCol.REVISAO = aprova;
                            pubRevCol.EDITOU = "N";
                            pubRevCol.MOTIVO = aprova == "R" ? "Devolvida em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " para a DIGITAÇÃO pelo Colaborador " + colaborador.COL_NOME.ToString() :
                                                               "Aprovada em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " na REVISÃO ORTOGRÁFICA pelo Colaborador " + colaborador.COL_NOME.ToString();
                            pubRevCol.MOTIVO = pubRevCol.MOTIVO + (motivo == null ? "" : ": \"" + motivo + "\"");

                            // salvando o LOG da aprovação técnica da revisão...
                            _servicePublicacaoRevisaoColaborador.SalvarPublicacaoRevisaoColaborador(pubRevCol);

                            // salvando a matéria aprovada pela técnica no campo específico - ALT: 14/04/2016
                            PublicacaoDTO pub = _servicePublicacao.FindById(publicacaoRevisao.PUB_ID);
                            pub.PUB_CONTEUDO_RESENHA_RVO = (aprova == "A") ? pub.PUB_CONTEUDO_RESENHA_RVO : null;

                            _servicePublicacao.SalvarPublicacao(pub);

                            // commit - confirmando operação sem erros...
                            scope.Complete();
                        }
                        else
                        {
                            throw new Exception("Você não está autorizado a reprovar matérias na Revisão Ortográfica!");
                        }
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    var _erro = new FormattedDbEntityValidationException(dbEx);

                    SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                    throw _erro;
                }
                catch (Exception ex)
                {
                    throw new Exception(SysException.Show(ex));
                }
            }
        }

        public Pagina<PublicacaoRevisaoDTO> RevisaoTecnica(int? informativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.RevisaoTecnica(informativo, anoInformativo, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<PublicacaoRevisaoDTO> Digitacao(int? informativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Digitacao(informativo, anoInformativo, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<PublicacaoRevisaoDTO> RevisaoOrtografica(int? informativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.RevisaoOrtografica(informativo, anoInformativo, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<PublicacaoRevisaoDTO> PublicacaoRevisao(int? pubId = null, int? colecionadorId = null, string revisaoTecnica = null, string digitacao = null, string revisaoOrtografica = null, int? informativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.PublicacaoRevisao(pubId, colecionadorId, revisaoTecnica, digitacao, revisaoOrtografica, informativo, anoInformativo, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarPublicacaoRevisao(PublicacaoRevisaoDTO publicacaoRevisao)
        {
            try
            {
                if (publicacaoRevisao.REV_ID != null)
                {
                    Merge(publicacaoRevisao, "REV_ID");
                }
                else
                {
                    Save(publicacaoRevisao);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var _erro = new FormattedDbEntityValidationException(dbEx);

                SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                throw _erro;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }

    }
}
