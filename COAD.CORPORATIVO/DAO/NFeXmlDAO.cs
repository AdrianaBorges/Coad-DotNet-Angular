using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class NFeXmlDAO : DAOAdapter<NFE_XML, NfeXmlDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public NFeXmlDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<NfeXmlDTO> ListarNFeXmlPorItemPedido(int? ipeId)
        {
            var query = (from nfeX in db.NFE_XML 
                         where nfeX.IPE_ID == ipeId &&
                         (nfeX.NFX_DATA_EXCLUSAO == null 
                         || nfeX.NFX_NUM_EXTORNADO == null 
                         || nfeX.NFX_NUM_EXTORNADO == false)
                             select nfeX);
            return ToDTO(query);
        }

        public string RetornarPath(int? NFX_ID)
        {
            var query = (from nfeX in db.NFE_XML
                         where nfeX.NFX_ID == NFX_ID 
                         select nfeX.NFX_PATH_NOTA).FirstOrDefault();
            return query;
        }

        public IList<NfeXmlDTO> ListarTodasAsNotasDoTipoProduto()
        {
            var query = (from nfeXml in db.NFE_XML
                         where nfeXml.NFX_TIPO == 0 &&
                         (nfeXml.NFX_DATA_EXCLUSAO == null
                         || nfeXml.NFX_NUM_EXTORNADO == null
                         || nfeXml.NFX_NUM_EXTORNADO == false)
                         select nfeXml);
            return ToDTO(query);
        }

        public IList<NfeXmlDTO> ListarNFeXmlPorPedido(int? pedCrmId)
        {
            var query = (from nfeX in db.NFE_XML 
                         where nfeX.ITEM_PEDIDO.PED_CRM_ID == pedCrmId &&
                         (nfeX.NFX_DATA_EXCLUSAO == null
                         || nfeX.NFX_NUM_EXTORNADO == null
                         || nfeX.NFX_NUM_EXTORNADO == false)
                         select nfeX);
            return ToDTO(query);
        }

        public IList<NfeXmlDTO> ListarNFeXmlProdutoPorItemPedido(int? ipeId)
        {
            var query = (from nfeX in db.NFE_XML
                         where 
                            nfeX.IPE_ID == ipeId &&
                            nfeX.NFX_TIPO == 0 &&
                         (nfeX.NFX_DATA_EXCLUSAO == null
                         || nfeX.NFX_NUM_EXTORNADO == null
                         || nfeX.NFX_NUM_EXTORNADO == false)
                         select nfeX);
            return ToDTO(query);
        }


    }
}
