using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Model;


namespace COAD.CORPORATIVO.DAO
{
    public class LinhaProdutoInformativoDAO : AbstractGenericDao<LINHA_PRODUTO_INFORMATIVO, LinhaProdutoInformativoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public LinhaProdutoInformativoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<LinhaProdutoInformativoDTO> ListarInformativo(int _pro_id)
        {
            PRODUTOS _produto = this.db.PRODUTOS.Where(x => x.PRO_ID == _pro_id).FirstOrDefault();

            IList<LINHA_PRODUTO_INFORMATIVO> query =  this.db.LINHA_PRODUTO_INFORMATIVO.Where(x => x.LIN_PRO_ID == _produto.LIN_PRO_ID).ToList();

            return ToDTO(query);
        }


    }
}
