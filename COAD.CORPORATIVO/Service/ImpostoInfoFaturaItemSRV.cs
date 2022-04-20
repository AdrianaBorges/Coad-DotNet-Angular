

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
	[ServiceConfig("IMP_ID", "IFI_ID")]
	public class ImpostoInfoFaturaItemSRV : GenericService<IMPOSTO_INFO_FATURA_ITEM, ImpostoInfoFaturaItemDTO, Int32>
	{

        public ImpostoInfoFaturaItemDAO _dao { get; set; }

        public ImpostoInfoFaturaItemSRV(ImpostoInfoFaturaItemDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

        public void SalvarImpostoFatura(InfoFaturaItemDTO infoItemFatura)
        {
            if (infoItemFatura != null && infoItemFatura.IMPOSTO_INFO_FATURA_ITEM != null)
            {
                var lstImpostoInfoFaturaItm = infoItemFatura.IMPOSTO_INFO_FATURA_ITEM;

                foreach (var impostoInfo in lstImpostoInfoFaturaItm)
                {
                    if (impostoInfo.IMP_ID == null && impostoInfo.IMPOSTO != null)
                    {
                        impostoInfo.IMP_ID = impostoInfo.IMPOSTO.IMP_ID;
                    }

                    if (impostoInfo.IFI_ID == null && infoItemFatura != null)
                    {
                        impostoInfo.IFI_ID = infoItemFatura.IFI_ID;
                    }
                    
                }

                SaveOrUpdateNonIdentityKeyEntity(lstImpostoInfoFaturaItm);
            }
        }

        public ICollection<ImpostoInfoFaturaItemDTO> ListarImpostoInfoFaturaItm(int? ifiId)
        {
            return _dao.ListarImpostoInfoFatura(ifiId);
        }

        public void PreencherImpostoInfoFaturaItm(InfoFaturaItemDTO infoFaturaItem)
        {
            if(infoFaturaItem != null)
            {
                infoFaturaItem.IMPOSTO_INFO_FATURA_ITEM = ListarImpostoInfoFaturaItm(infoFaturaItem.IFI_ID);
            }
        }
    }
}
