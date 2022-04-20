using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("IFF_ID")]
    public class InfoFaturaSRV : ServiceAdapter<INFO_FATURA, InfoFaturaDTO, int>
    {
        private InfoFaturaDAO _dao;

        [ServiceProperty("IFF_ID", Name= "impostoInfoFatura", PropertyName = "IMPOSTO_INFO_FATURA")]
        public ImpostoInfoFaturaSRV _impostoInfoFaturaSRV { get; set; }
        public InfoFaturaItemSRV _infoFaturaItemSRV { get; set; }

        public InfoFaturaSRV()
        {
            _dao = new InfoFaturaDAO();
            _impostoInfoFaturaSRV = new ImpostoInfoFaturaSRV();
            SetDao(_dao);
        }

        public InfoFaturaSRV(InfoFaturaDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public InfoFaturaDTO SalvarInfoFatura(ItemPedidoDTO itemPedido)
        {
            if (itemPedido != null && itemPedido.INFO_FATURA != null)
            {
                var infoFatura = itemPedido.INFO_FATURA;
                var lstImpostoInfoFatura = infoFatura.IMPOSTO_INFO_FATURA;
                infoFatura.IMPOSTO_INFO_FATURA = null;

                var faturaSalva = SaveOrUpdate(infoFatura);
                faturaSalva.IMPOSTO_INFO_FATURA = lstImpostoInfoFatura;

                _impostoInfoFaturaSRV.SalvarImpostoFatura(faturaSalva);
                itemPedido.IFF_ID = faturaSalva.IFF_ID;

                return faturaSalva;                
            }

            return null;
        }

        private InfoFaturaDTO _salvarInfoFatura(InfoFaturaDTO infoFatura)
        {
            if (infoFatura != null)
            {
                var lstImpostoInfoFatura = infoFatura.IMPOSTO_INFO_FATURA;
                var lstInfoFaturaItem = infoFatura.INFO_FATURA_ITEM;

                infoFatura.IMPOSTO_INFO_FATURA = null;
                infoFatura.INFO_FATURA_ITEM = null;

                var faturaSalva = SaveOrUpdate(infoFatura);
                faturaSalva.IMPOSTO_INFO_FATURA = lstImpostoInfoFatura;
                faturaSalva.INFO_FATURA_ITEM = lstInfoFaturaItem;

                _impostoInfoFaturaSRV.SalvarImpostoFatura(faturaSalva);
                _infoFaturaItemSRV.SalvarInfoFaturaItem(faturaSalva);

                return faturaSalva;
            }
            return null;
        }

        public void SalvarInfoFatura(PropostaItemDTO propostaItem)
        {
            if (propostaItem != null)
            {            
                if(propostaItem.IFF_ID != null && propostaItem.INFO_FATURA == null)
                {
                    var infoFat = FindById(propostaItem.IFF_ID);
                    _infoFaturaItemSRV.DeletarItens(infoFat);
                    Delete(infoFat);
                }

                if (propostaItem.IFF_ID_ENTRADA != null && propostaItem.INFO_FATURA1 == null)
                {
                    var infoFat = FindById(propostaItem.IFF_ID_ENTRADA);
                    _infoFaturaItemSRV.DeletarItens(infoFat);
                    Delete(infoFat);
                }

                if (propostaItem.INFO_FATURA != null || propostaItem.INFO_FATURA1 != null)
                {
                    var infoFaturaEntrada = propostaItem.INFO_FATURA1;
                    var infoFaturaParcela = propostaItem.INFO_FATURA;

                    var infoFaturaEntradaSalva = _salvarInfoFatura(infoFaturaEntrada);
                    var infoFaturaParcelaSalva = _salvarInfoFatura(infoFaturaParcela);

                    if (infoFaturaEntrada != null)
                    {
                        propostaItem.IFF_ID_ENTRADA = infoFaturaEntradaSalva.IFF_ID;
                    }

                    if (infoFaturaParcelaSalva != null)
                    {
                        propostaItem.IFF_ID = infoFaturaParcelaSalva.IFF_ID;
                    }
                }

                ServiceFactory.RetornarServico<PropostaItemSRV>().Merge(propostaItem);
            }
        }

        public InfoFaturaDTO FindByIdFullLoaded(int? iffId, bool trazImpostoFatura = false, bool trazInfoFaturaItem = false)
        {
            var infoFatura = FindById(iffId);

            if(infoFatura != null)
            {
                if (trazImpostoFatura)
                {
                    GetAssociations(infoFatura, "impostoInfoFatura");
                }

                if (trazInfoFaturaItem)
                {
                    _infoFaturaItemSRV.PreencherInfoFaturaItem(infoFatura);
                }
            }
            return infoFatura;
        }

        public void PreencherInfoFaturaImposto(IEnumerable<ItemPedidoDTO> lstItemPedido)
        {
            if (lstItemPedido != null)
            {               
                foreach (var itemPedido in lstItemPedido)
                {
                    if (itemPedido.INFO_FATURA != null)
                    {
                        GetAssociations(itemPedido.INFO_FATURA, "impostoInfoFatura");

                        _infoFaturaItemSRV.PreencherInfoFaturaItem(itemPedido.INFO_FATURA, true);
                    }

                    if (itemPedido.INFO_FATURA1 != null)
                    {
                        GetAssociations(itemPedido.INFO_FATURA1, "impostoInfoFatura");

                        _infoFaturaItemSRV.PreencherInfoFaturaItem(itemPedido.INFO_FATURA1, true);
                    }

                }
            }
        }

        public void PreencherInfoFaturaImposto(IEnumerable<PropostaItemDTO> lstPropostaItem)
        {
            if (lstPropostaItem != null)
            {
                foreach (var propostaItem in lstPropostaItem)
                {
                    if (propostaItem.INFO_FATURA != null)
                    {
                        GetAssociations(propostaItem.INFO_FATURA, "impostoInfoFatura");
                        _infoFaturaItemSRV.PreencherInfoFaturaItem(propostaItem.INFO_FATURA, true);
                    }

                    if (propostaItem.INFO_FATURA1 != null)
                    {
                        GetAssociations(propostaItem.INFO_FATURA1, "impostoInfoFatura");
                        _infoFaturaItemSRV.PreencherInfoFaturaItem(propostaItem.INFO_FATURA1, true);
                    }                    
                }
            }
        }
    }
}
