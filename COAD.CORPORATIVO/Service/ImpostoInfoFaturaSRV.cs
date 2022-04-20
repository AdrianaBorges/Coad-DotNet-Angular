using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{

    [ServiceConfig("IMP_ID", "IFF_ID")]
    public class ImpostoInfoFaturaSRV : ServiceAdapter<IMPOSTO_INFO_FATURA, ImpostoInfoFaturaDTO, int>
    {
        private ImpostoInfoFaturaDAO _dao = new ImpostoInfoFaturaDAO();

        public ImpostoInfoFaturaSRV()
        {
            SetDao(_dao);
        }

        public void SalvarImpostoFatura(InfoFaturaDTO infoFatura)
        {
            if (infoFatura != null && infoFatura.IMPOSTO_INFO_FATURA != null)
            {
                var lstImpostoInfoFatura = infoFatura.IMPOSTO_INFO_FATURA;

                foreach (var impostoInfo in lstImpostoInfoFatura)
                {
                    if (impostoInfo.IMP_ID == null && impostoInfo.IMPOSTO != null)
                    {
                        impostoInfo.IMP_ID = impostoInfo.IMPOSTO.IMP_ID;
                    }

                    if (impostoInfo.IFF_ID == null && infoFatura != null)
                    {
                        impostoInfo.IFF_ID = infoFatura.IFF_ID;
                    }
                }

                SaveOrUpdateNonIdentityKeyEntity(lstImpostoInfoFatura);
            }
        }

        IList<ImpostoInfoFaturaDTO> ListByIffId(int? iffId)
        {
            return _dao.ListByIffId(iffId);
        }
    }
}
