using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.DAO.CorporativoAntigo;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.CORPORATIVO.LEGADO.Model.Dto.CorporativoAntigo;
using COAD.CORPORATIVO.LEGADO.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.LEGADO.Service.CorporativoAntigo
{
    [ServiceConfig("COD_PROD", "LETRA")]
    public class SeqProdSRV : GenericService<SEQ_PROD, SeqProdDTO, object>
    {
        private SeqProdDAO _dao = new SeqProdDAO();

        public SeqProdSRV()
        {
            Dao = _dao;
        }

        public SeqProdDTO GetSeqProduto(int prodId, int mes)
        {
            var letra = AssinaturaUtil.GetLetraFromMes(mes);
            var prodIdStr = (prodId > 9) ? prodId.ToString() : "0" + prodId.ToString();

            return GetSeqProduto(prodIdStr, letra);
        }

        public SeqProdDTO GetSeqProduto(int prodId, char letra)
        {
            var prodIdStr = (prodId > 9) ? prodId.ToString() : "0" + prodId.ToString();
            return GetSeqProduto(prodIdStr, letra);
        }

        public SeqProdDTO GetSeqProduto(string prodId, char letra)
        {
            var seqProd  = _dao.GetSequenciaAssinaturaPorProduto(prodId, letra);

            if (seqProd == null) // e a sequencia ainda não existe crio e retorno
            {
                var novaSequencia = new SeqProdDTO()
                {
                    COD_PROD = prodId,
                    LETRA = letra.ToString(),
                    SEQUENCIA = "0"
                };

                //seqProd.SEQUENCIA = "0";
                seqProd = Save(novaSequencia);
            }

            return seqProd;
        }

        /// <summary>
        /// Salva a sequência da assinatura
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="letra"></param>
        /// <param name="sequencia"></param>
        /// <returns></returns>
        public SeqProdDTO SalvarSequencia(string prodId, char letra, string sequencia)
        {
            var letraStr = letra.ToString();
            SeqProdDTO seqProd = FindById(prodId, letraStr);
            seqProd.SEQUENCIA = sequencia;

            SaveOrUpdateNonIdentityKeyEntity(seqProd); // Merge(seqProd, "COD_PROD", "LETRA");
            return seqProd;
        }

       

    }
}
