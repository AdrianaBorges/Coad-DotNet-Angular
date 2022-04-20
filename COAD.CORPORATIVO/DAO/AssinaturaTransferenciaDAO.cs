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
    public class AssinaturaTransferenciaDAO : DAOAdapter<ASSINATURA_TRANSFERENCIA, AssinaturaTransferenciaDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AssinaturaTransferenciaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public IList<AssinaturaTransferenciaDTO> BuscarTrasferenciaPorPeriodo(int _mes, int _ano, string _assinatura)
        {
            var query = (from T in db.ASSINATURA_TRANSFERENCIA
                         where (T.ASN_DATA_TRANSF.Value.Month == _mes) &&
                               (T.ASN_DATA_TRANSF.Value.Year == _ano) &&
                               (_assinatura == null || (_assinatura != null && T.ASN_NUM_ASSINATURA_ATU != _assinatura))
                         select T);


            return ToDTO(query);

        }

    }
}
