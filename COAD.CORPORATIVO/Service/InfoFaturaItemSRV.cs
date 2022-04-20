

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("IFI_ID")]
	public class InfoFaturaItemSRV : GenericService<INFO_FATURA_ITEM, InfoFaturaItemDTO, Int32>
	{
        public InfoFaturaItemDAO _dao { get; set; }
        public ImpostoInfoFaturaItemSRV _impostoInfoFaturaItemSRV { get; set; }

        public InfoFaturaItemSRV(InfoFaturaItemDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }
        
        public void SalvarInfoFaturaItem(InfoFaturaDTO infoFatura)
        {
            if(infoFatura != null && infoFatura.INFO_FATURA_ITEM != null)
            {
                foreach(var infoItm in infoFatura.INFO_FATURA_ITEM)
                {
                    _salvarFaturaItm(infoItm, infoFatura);
                }
            }
        }

        private void _salvarFaturaItm(InfoFaturaItemDTO infoFaturaItem, InfoFaturaDTO infoFatura)
        {
            if(infoFaturaItem != null)
            {
                var lstImpostos = infoFaturaItem.IMPOSTO_INFO_FATURA_ITEM;

                infoFaturaItem.IMPOSTO_INFO_FATURA_ITEM = null;
                infoFaturaItem.INFO_FATURA = null;

                if (infoFaturaItem.IFF_ID == null && infoFatura != null)
                    infoFaturaItem.IFF_ID = infoFatura.IFF_ID;

                var infoFa = SaveOrUpdate(infoFaturaItem);

                if (infoFaturaItem.IFI_ID == null && infoFa != null)
                    infoFaturaItem.IFI_ID = infoFa.IFI_ID;

                infoFaturaItem.IMPOSTO_INFO_FATURA_ITEM = lstImpostos;
                _impostoInfoFaturaItemSRV.SalvarImpostoFatura(infoFaturaItem);
            }
        }

        public IList<InfoFaturaItemDTO> ListarInfoFaturaItemDaInfoFatura(int? iffId, bool trazImpostoInfoFatItm = false)
        {
            var lstItmFatura = _dao.ListarInfoFaturaItemDaInfoFatura(iffId);

            if(lstItmFatura != null && 
                lstItmFatura.Count > 0 &&
                trazImpostoInfoFatItm)
            {
                foreach(var infFat in lstItmFatura)
                {
                    _impostoInfoFaturaItemSRV.PreencherImpostoInfoFaturaItm(infFat);
                }
            }

            return lstItmFatura;
        }

        public void PreencherInfoFaturaItem(InfoFaturaDTO infoFatura, bool trazImpostoInfoFatItm = false)
        {
            if(infoFatura != null && infoFatura.IFF_ID != null)
            {
                infoFatura.INFO_FATURA_ITEM = ListarInfoFaturaItemDaInfoFatura(infoFatura.IFF_ID, trazImpostoInfoFatItm);
            }
        }
        
        public void DeletarItens(InfoFaturaDTO infoFatura)
        {
            if(infoFatura != null 
                && infoFatura.INFO_FATURA_ITEM != null 
                && infoFatura.INFO_FATURA_ITEM.Count > 0)
            {
                DeleteAll(infoFatura.INFO_FATURA_ITEM);
            }
        }

        public InfoFaturaItemDTO ListarInfoFaturaItemPorConfig(int? iffId, int? nfcId)
        {
            var infoFaturaItm = _dao.ListarInfoFaturaItemPorConfig(iffId, nfcId);

            if(infoFaturaItm != null)
            {
                _impostoInfoFaturaItemSRV.PreencherImpostoInfoFaturaItm(infoFaturaItm);
            }
            return infoFaturaItm;
        }

        public InfoFaturaItemDTO ListarInfoFaturaPorItem(int? iffId)
        {
            var infoFaturaItm = _dao.ListarInfoFaturaPorItem(iffId);

            if (infoFaturaItm != null)
            {
                _impostoInfoFaturaItemSRV.PreencherImpostoInfoFaturaItm(infoFaturaItm);
            }
            return infoFaturaItm;
        }
    }
}
