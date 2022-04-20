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
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto.Custons.Validacoes;
using GenericCrud.Service;
using COAD.CORPORATIVO.Model.Dto.Custons.Historicos;
using COAD.CORPORATIVO.Service.Custons;
using System.Transactions;
using COAD.CORPORATIVO.Exceptions;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("RIT_ID")]
    public class RegistroLiberacaoItemSRV : GenericService<REGISTRO_LIBERACAO_ITEM, RegistroLiberacaoItemDTO, int>
    {
        public HistoricoNotificacaoSRV historicoNotificacaoSRV { get; set; }
        public RegistroLiberacaoSRV _registroLiberacaoSRV { get; set; }

        private RegistroLiberacaoItemDAO _dao;

        public RegistroLiberacaoItemSRV()
        {
            this._dao = new RegistroLiberacaoItemDAO();
            this.Dao = _dao;
        }

        public RegistroLiberacaoItemSRV(RegistroLiberacaoItemDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public IList<RegistroLiberacaoItemDTO> ListarRegistroItemPorRegistroAtivo(int? rliId)
        {
            return _dao.ListarRegistroItemPorRegistroAtivo(rliId);
        }

        public IList<RegistroLiberacaoItemDTO> ListarRegistroItemPorRegistro(int? rliId)
        {
            return _dao.ListarRegistroItemPorRegistro(rliId);
        }

        public void CriarRegistroLiberacaoItem(PropostaItemDTO propostaItem, 
            string mensagem, 
            int? repId, 
            string usuario,
            bool usaCliente = false,
            bool insereHistorico = true)
        {
            var registroLiberacao = _registroLiberacaoSRV.RetornarRegistroLiberacaoPropostaItem(propostaItem, repId, usuario);
            var registroLiberacaoItem = new RegistroLiberacaoItemDTO()
            {
                RLI_ID = registroLiberacao.RLI_ID,
                RIT_DESCRICAO = mensagem,
                RIT_DATA_CRIACAO = DateTime.Now 
            };

            Save(registroLiberacaoItem);
            ServiceFactory.RetornarServico<PropostaItemSRV>().AlterarStatusPropostaItem(propostaItem.PPI_ID, 9);
            
            if (insereHistorico)
            {
                var descriptor = new HistoricoFormatterSRV()
                {
                    CLI_ID = propostaItem.PROPOSTA.CLI_ID,
                    Message = mensagem,
                    PPI_ID = propostaItem.PPI_ID,
                    REP_ID = repId,
                    PST_ID = 9,
                    RLI_ID = registroLiberacao.RLI_ID,
                    usuario = usuario
                };
                historicoNotificacaoSRV.RegistrarHistoricoRegistroLiberacaoItem(descriptor);
            }
        }

        public void PreencharRegistroLiberacaoItem(RegistroLiberacaoDTO registroLib)
        {
            if(registroLib != null)
            {
                registroLib.REGISTRO_LIBERACAO_ITEM = ListarRegistroItemPorRegistroAtivo(registroLib.RLI_ID);
            }
        }

        public string RegistrarHistoricoPendenciaLiberacaoProposta(PropostaItemDTO propostaItem, ValidacaoClienteInadimplenteDTO validacao, string usuario, int? repId)
        {
            string mensagem = "O representante {{representante}} emitiu um item de proposta '{{PPI_ID}}'. Porém ouve pendência para liberação da mesma. ";
            mensagem += string.Format("Detalhes: {0}. ", validacao);


            var descriptor = new HistoricoFormatterSRV()
            {
                CLI_ID = propostaItem.PROPOSTA.CLI_ID,
                Message = mensagem,
                PPI_ID = propostaItem.PPI_ID,
                REP_ID = repId,
                PST_ID = 9,
                usuario = usuario
            };

            historicoNotificacaoSRV.RegistrarHistoricoPendenciaDeLiberação(descriptor);
            return mensagem;
        }

        public void AprovarPendencia(int? rliId, string usuario, int? repId, string observacoes)
        {
            int ppiId = 0;
            try
            {
                using (var scope = new TransactionScope())
                {
                    if(rliId != null)
                    {
                        var registroLiberacao = _registroLiberacaoSRV.FindById(rliId);
                        var lstRegistroLiberacaoItem = ListarRegistroItemPorRegistroAtivo(rliId);
                        var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindByRegistroLiberacao(rliId);

                        if(lstRegistroLiberacaoItem != null && lstRegistroLiberacaoItem.Count > 0)
                        {
                            foreach(var reg in lstRegistroLiberacaoItem)
                            {
                                reg.RIT_LIBERADO = true;
                            }
                            
                            MergeAll(lstRegistroLiberacaoItem);

                            var mensagem = "A representante {{representante}} aprovou a pendência de liberação com o seguinte código {{RLI_ID}}. Observações do(a) representante: '{{OBS}}'.";
                            var descriptor = new HistoricoFormatterSRV()
                            {
                                CLI_ID = propostaItem.PROPOSTA.CLI_ID,
                                Message = mensagem,
                                PPI_ID = propostaItem.PPI_ID,
                                REP_ID = repId,
                                PST_ID = 9,
                                RLI_ID = rliId,
                                usuario = usuario,
                                Observacoes = observacoes
                            };
                            historicoNotificacaoSRV.RegistrarHistoricoPendenciaDeLiberação(descriptor);
                        }

                        ServiceFactory.RetornarServico<PropostaItemSRV>().AlterarStatusPropostaItem(propostaItem.PPI_ID, 1);
                        registroLiberacao.DATA_EXCLUSAO = DateTime.Now;
                        registroLiberacao.RLT_DATA_ACAO = DateTime.Now;
                        _registroLiberacaoSRV.Merge(registroLiberacao);
                        ServiceFactory.RetornarServico<RepresentanteSRV>().NotificarRepresentanteRegistroLiberacao(rliId, repId, observacoes, true);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new RegistroLiberacaoException(string.Format("Não é possível rejeitar a proposta {0}. Veja os detalhes para mais informações.", ppiId), e);
            }
          
        }

        public void RejeitarPendencia(int? rliId, string usuario, int? repId, string observacoes)
        {
            int ppiId = 0;
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (rliId != null)
                    {
                        var registroLiberacao = _registroLiberacaoSRV.FindById(rliId);
                        var lstRegistroLiberacaoItem = ListarRegistroItemPorRegistroAtivo(rliId);
                        var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindByRegistroLiberacao(rliId);
                        ppiId = propostaItem.PPI_ID.Value;

                        if (lstRegistroLiberacaoItem != null && lstRegistroLiberacaoItem.Count > 0)
                        {
                            foreach (var reg in lstRegistroLiberacaoItem)
                            {
                                reg.RIT_LIBERADO = false;
                            }

                            MergeAll(lstRegistroLiberacaoItem);

                            var mensagem = "A representante {{representante}} rejeitou a pendência de liberação com o seguinte código {{RLI_ID}}. Observações do(a) representante: '{{OBS}}'.";
                            var descriptor = new HistoricoFormatterSRV()
                            {
                                CLI_ID = propostaItem.PROPOSTA.CLI_ID,
                                Message = mensagem,
                                PPI_ID = propostaItem.PPI_ID,
                                REP_ID = repId,
                                PST_ID = 10,
                                usuario = usuario,
                                Observacoes = observacoes
                            };
                            historicoNotificacaoSRV.RegistrarHistoricoPendenciaDeLiberação(descriptor);

                        }

                        ServiceFactory.RetornarServico<PropostaItemSRV>().AlterarStatusPropostaItem(propostaItem.PPI_ID, 10);
                        registroLiberacao.DATA_EXCLUSAO = DateTime.Now;
                        registroLiberacao.RLT_DATA_ACAO = DateTime.Now;
                        _registroLiberacaoSRV.Merge(registroLiberacao);
                        ServiceFactory.RetornarServico<RepresentanteSRV>().NotificarRepresentanteRegistroLiberacao(rliId, repId, observacoes, false);
                    }
                    scope.Complete();
                }
            }
            catch(Exception e)
            {
                throw new RegistroLiberacaoException(string.Format("Não é possível rejeitar a proposta {0}. Veja os detalhes para mais informações.", ppiId), e);
            }
        }
    }

}
