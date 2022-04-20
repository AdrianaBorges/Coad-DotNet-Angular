using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PPR_ID")]
    public class PedidoParticipanteSRV : ServiceAdapter<PEDIDO_PARTICIPANTE, PedidoParticipanteDTO, int>
    {
        private PedidoParticipanteDAO _dao = new PedidoParticipanteDAO();

        public PedidoParticipanteSRV()
        {
            SetDao(_dao);
        }

        public IList<PedidoParticipanteDTO> ListPedidoParticipanteByItemPedido(int? IPE_ID)
        {
            return _dao.ListPedidoParticipanteByItemPedido(IPE_ID);
        }

        public IList<PedidoParticipanteDTO> PesquisarPorPropostaItem(int? PPI_ID)
        {
            return _dao.ListPedidoParticipanteByPropostaItem(PPI_ID);
        }

        public void PreencherPedidoParticipanteNaPropostaItem(PropostaItemDTO propostaItem)
        {
            if(propostaItem != null)
            {
                propostaItem.PEDIDO_PARTICIPANTE = PesquisarPorPropostaItem(propostaItem.PPI_ID);
            }
        }

        public void ChecarExcluirPedidoParticipantesAusentes(PropostaItemDTO propostaItem)
        {
            var propostaItemSRV = ServiceFactory.RetornarServico<PropostaItemSRV>();

            if (propostaItem != null)
            {
                var objetoDoBanco = propostaItemSRV.FindByIdFullLoaded(propostaItem, true);
                ExcluirList<PropostaItemDTO>(propostaItem, propostaItem, "PEDIDO_PARTICIPANTE");
            }
        }

        public void SalvarPedidoParticipantes(ItemPedidoDTO itemPedido)
        {
            if (itemPedido != null)
            {
                var lstPedidoParticipante = itemPedido.PEDIDO_PARTICIPANTE;

                if (lstPedidoParticipante != null)
                {
                    CheckAndAssignKeyFromParentToChildsList(itemPedido, lstPedidoParticipante, "IPE_ID", "PED_CRM_ID");

                    if (itemPedido.PEDIDO_CRM != null)
                    {
                        CheckAndAssignKeyFromParentToChildsList(itemPedido.PEDIDO_CRM, lstPedidoParticipante, "CLI_ID");   
                    }

                    foreach (var pedidoParticipante in lstPedidoParticipante)
                    {
                        _processarSalvamentoPedidoParticipante(pedidoParticipante);
                    }
                }
            }
        }


        public void SalvarPedidoParticipante(PropostaItemDTO propostaItem)
        {
            if (propostaItem != null)
            {
                var lstPedidoParticipante = propostaItem.PEDIDO_PARTICIPANTE;

                if (lstPedidoParticipante != null)
                {
                    CheckAndAssignKeyFromParentToChildsList(propostaItem, lstPedidoParticipante, "PRT_ID", "PPI_ID");

                    if (propostaItem.PRT_ID != null)
                    {
                        var proposta = ServiceFactory.RetornarServico<PropostaSRV>().FindById(propostaItem.PRT_ID);
                        CheckAndAssignKeyFromParentToChildsList(proposta, lstPedidoParticipante, "CLI_ID");
                    }

                    SaveOrUpdateAll(lstPedidoParticipante);
                }
            }
        }


        private void _processarSalvamentoPedidoParticipante(PedidoParticipanteDTO pedidoParticipante)
        {
            if (pedidoParticipante != null)
            {
                SaveOrUpdate(pedidoParticipante);
            }
        }
    }

}
