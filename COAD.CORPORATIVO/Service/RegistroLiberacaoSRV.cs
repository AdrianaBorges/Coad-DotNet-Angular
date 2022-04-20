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
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("RLI_ID")]
    public class RegistroLiberacaoSRV : GenericService<REGISTRO_LIBERACAO, RegistroLiberacaoDTO, int>
    {
        public HistoricoNotificacaoSRV historicoNotificacaoSRV { get; set; }
        private RegistroLiberacaoDAO _dao;

        public RegistroLiberacaoSRV()
        {
            this._dao = new RegistroLiberacaoDAO();
            this.Dao = _dao;
        }

        public RegistroLiberacaoSRV(RegistroLiberacaoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public Pagina<RegistroLiberacaoDTO> PesquisarRegistrosLiberacao(PesquisaRegistroLiberacaoDTO pesquisaDTO)
        {
            var paginaRegistroLiberacao = _dao.PesquisarRegistrosLiberacao(pesquisaDTO);
            if(paginaRegistroLiberacao != null)
            {
                PreencherPropostaItem(paginaRegistroLiberacao.lista);
            }
            return paginaRegistroLiberacao;
        }

        public void PreencherPropostaItem(IEnumerable<RegistroLiberacaoDTO> lstRegistroLiberacao)
        {
            if(lstRegistroLiberacao != null)
            {
                var propostaItemSRV = ServiceFactory.RetornarServico<PropostaItemSRV>();
                foreach(var regLib in lstRegistroLiberacao)
                {
                    regLib.PropostaItem = propostaItemSRV.FindByRegistroLiberacao(regLib.RLI_ID);
                }
            }
        }

        public RegistroLiberacaoDTO RetornarRegistroLiberacaoPropostaItem(PropostaItemDTO propostaItem, int? repId, string usuario)
        {
            RegistroLiberacaoDTO registroLiberacao = null;
            if (propostaItem != null)
            {

                if (propostaItem.RLI_ID != null)
                {
                    registroLiberacao = FindById(propostaItem.RLI_ID);
                    if (registroLiberacao.DATA_EXCLUSAO != null)
                    {
                        registroLiberacao.DATA_EXCLUSAO = null;
                        registroLiberacao.RLT_DATA_ACAO = DateTime.Now;
                        Merge(registroLiberacao);
                    }
                }
                else
                {
                    registroLiberacao = new RegistroLiberacaoDTO()
                    {
                        REP_ID = repId,
                        RLI_DATA_CADASTRO = DateTime.Now,
                        RLT_DATA_ACAO = DateTime.Now,
                        RLI_DESCRICAO = "Proposta Aguardando Liberação.",
                        USU_LOGIN = usuario,
                        RLT_ID = 1,
                        CLI_ID = propostaItem.PROPOSTA.CLI_ID
                    };

                    registroLiberacao = Save(registroLiberacao);
                    propostaItem.RLI_ID = registroLiberacao.RLI_ID;
                    ServiceFactory.RetornarServico<PropostaItemSRV>().Merge(propostaItem);
                }
            }
            return registroLiberacao;
        }


        /// <summary>
        /// Checa se todos os detalhes de uma liberação ocorreu. Caso possitivo. Dá baixa na liberação
        /// </summary>
        /// <param name="rliId"></param>
        public void ChecarLiberacaoTotal(int? rliId)
        {
            var registrosItem = ServiceFactory.RetornarServico<RegistroLiberacaoItemSRV>().ListarRegistroItemPorRegistroAtivo(rliId);
            var count = registrosItem.Count();
            var countBaixados = registrosItem.Where(x => x.RIT_DATA_ACAO != null).Count();

            if(count == countBaixados)
            {
                var registro = FindById(rliId);
                registro.RLT_DATA_ACAO = DateTime.Now;
                Merge(registro);
            }

        }
    }
}
