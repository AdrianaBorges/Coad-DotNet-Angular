using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.LEGADO.Model.Dto.CorporativoAntigo;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.DAO.CorporativoAntigo
{
    public class SeqProdDAO : AbstractGenericDao<SEQ_PROD, SeqProdDTO, object>
    {
        public SeqProdDAO()
        {
            SetProfileName("corp_old");
        }

        public SeqProdDTO GetSequenciaAssinaturaPorProduto(string prodId, char letra)
        {

            var letraStr = letra.ToString();
            IQueryable<SEQ_PROD> query = GetDbSet().Where(op => op.COD_PROD == prodId && op.LETRA == letraStr);
            var obj = query.FirstOrDefault();

            return ToDTO(obj);
        }

    }
}
