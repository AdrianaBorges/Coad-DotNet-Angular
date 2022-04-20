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
    [ServiceConfig("ASN_NUM_ASSINATURA_ANT", "ASN_NUM_ASSINATURA_ATU")]
    public class AssinaturaTransferenciaSRV : ServiceAdapter<ASSINATURA_TRANSFERENCIA, AssinaturaTransferenciaDTO, object>
    {
        private AssinaturaTransferenciaDAO _dao;

        public AssinaturaTransferenciaSRV()
        {
            this._dao = new AssinaturaTransferenciaDAO();
            SetDao(_dao);
        }

        public AssinaturaTransferenciaSRV(AssinaturaTransferenciaDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public IList<AssinaturaTransferenciaDTO> BuscarTrasferenciaPorPeriodo(int _mes, int _ano, string _assinatura)
        {
            return _dao.BuscarTrasferenciaPorPeriodo(_mes, _ano, _assinatura);
        }

        public void RegistrarHistoricoTransferencia(
            int cliId, 
            string assinaturaOrigem, 
            string assinaturaAtual, 
            string usuLogin, 
            string contratoOrigem, 
            string contratoAtual, 
            string motivo)
        {
            AssinaturaTransferenciaDTO assTransf = new AssinaturaTransferenciaDTO()
            {
                ASN_NUM_ASSINATURA_ANT = assinaturaOrigem,
                ASN_NUM_ASSINATURA_ATU = assinaturaAtual,
                USU_LOGIN = usuLogin,
                ASN_DATA_TRANSF = DateTime.Now,
                ASN_TRANSF_MOTIVO = motivo,
                ASN_TRANSF_SOLICITANTE = usuLogin,
                CLI_ID = cliId,
                ASN_TRANSF_CONTRATO_ORIGEM = contratoOrigem,
                ASN_TRANSF_CONTRATO_GERADO = contratoAtual                
            };

            SaveOrUpdateNonIdentityKeyEntity(assTransf);
        }
    }
}
