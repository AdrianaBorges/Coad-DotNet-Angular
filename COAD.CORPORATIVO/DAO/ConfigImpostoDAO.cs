using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Impostos;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ConfigImpostoDAO : DAOAdapter<CONFIG_IMPOSTO, ConfigImpostoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ConfigImpostoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<ConfigImpostoDTO> ObterConfiguracaoPorRegras(
                RequisicaoConfigImpostoDTO requisicao)
        {

            if (requisicao == null)
                throw new Exception("A requisição de configuração de imposto não foi encontrada");

            var cmpId = requisicao.CmpID;
            var nfcId = requisicao.NfcId;
            var tipoCliId = requisicao.TipoCliId;
            var empresaDoSimples = requisicao.EmpresaDoSimples;
            var clienteRetem = requisicao.ClienteRetem;

            if (empresaDoSimples == null)
                empresaDoSimples = false;

            var query = (from 
                            cfIn in db.CONFIG_IMPOSTO join
                            nfc in db.NOTA_FISCAL_CONFIG on cfIn.NFC_ID equals nfc.NFC_ID
                         where 
                            cfIn.DATA_EXCLUSAO == null &&
                            nfc.DATA_EXCLUSAO == null &&
                            nfc.CMP_ID == cmpId &&
                            (nfcId == null || nfc.NFC_ID == nfcId) &&
                            cfIn.TIPO_CLI_ID == tipoCliId &&
                            ((cfIn.CFI_VLR_DESCONTO_MIM != null && cfIn.CFI_VLR_DESCONTO_MIM > 0) || cfIn.CFI_QUALQUER_VALOR == true) &&
                            (empresaDoSimples == null || (empresaDoSimples == false && cfIn.CFI_EMPRESA_DO_SIMPLES == null) || cfIn.CFI_EMPRESA_DO_SIMPLES == empresaDoSimples) &&
                            (clienteRetem == null || (clienteRetem == false && cfIn.CFI_CLIENTE_RETEM == null) ||  cfIn.CFI_CLIENTE_RETEM == clienteRetem)
                             select cfIn);

            return ToDTO(query);
        }

        public IList<ConfigImpostoDTO> ListarConfigImpostoNotaFiscalConfig(int? nfcId)
        {
            var query = (from 
                            cfIn in db.CONFIG_IMPOSTO
                         where
                            cfIn.NFC_ID == nfcId &&
                            cfIn.DATA_EXCLUSAO == null
                         select cfIn);
            return ToDTO(query);
        }
    }
}
