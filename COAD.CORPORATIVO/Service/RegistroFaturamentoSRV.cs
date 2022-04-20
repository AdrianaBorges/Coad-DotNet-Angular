using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;
using System.IO;
using GenericCrud.Util;
using System.Web;
using COAD.FISCAL.Model;
using COAD.FISCAL.XmlUtils;
using COAD.CORPORATIVO.Util;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Service;
using COAD.SEGURANCA.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("RFT_ID")]
    public class RegistroFaturamentoSRV : GenericService<REGISTRO_FATURAMENTO, RegistroFaturamentoDTO, int>
    {
        /// <summary>
        /// 
        /// </summary>
        public RegistroFaturamentoDAO _dao { get; set; }

        public RegistroFaturamentoSRV(RegistroFaturamentoDAO _dao)
        {
            this.Dao = _dao;
        }

        public RegistroFaturamentoSRV()
        {
            this.Dao = new RegistroFaturamentoDAO();
        }

        public IList<RegistroFaturamentoDTO> RetornarRegistroDeFaturamentoPorItemDePedido(int? ipeId)
        {
            var lstRegistrosFaturamento =_dao.RetornarRegistroDeFaturamentoPorItemDePedido(ipeId);

            if(lstRegistrosFaturamento != null && lstRegistrosFaturamento.Count() > 0)
            {
                var empSRV = ServiceFactory.RetornarServico<EmpresaSRV>();
                foreach(var registroFa in lstRegistrosFaturamento)
                {
                    registroFa.EMPRESA = empSRV.FindById(registroFa.EMP_ID);
                }
            }

            return lstRegistrosFaturamento;
        }

        public RegistroFaturamentoDTO RetornarPrimerioRegistroDeFaturamentoPorItemPedido(int? ipeId)
        {
            var lstRegFatura = RetornarRegistroDeFaturamentoPorItemDePedido(ipeId);
            if(lstRegFatura != null && lstRegFatura.Count() > 0)
            {
                return lstRegFatura.FirstOrDefault();
            }

            return null;
        }

        public void IncluirRegistroDeFaturamento(int? ipeId, ContextoFaturamentoDTO contexto, RequisicaoFaturamentoDetalheDTO detalhesFat,
            IEnumerable<ContratoDTO> contratos)
        {
            if(contexto != null && contratos != null)
            {
                IList<RegistroFaturamentoDTO> registros = new List<RegistroFaturamentoDTO>();
                
                foreach (var contrato in contratos)
                {
                    RegistroFaturamentoDTO registroFaturamento = new RegistroFaturamentoDTO()
                    {
                        IPE_ID = ipeId,
                        ASN_NUM_ASSINATURA = contexto.assinatura.ASN_NUM_ASSINATURA,
                        CTR_NUM_CONTRATO = contrato.CTR_NUM_CONTRATO,
                        EMP_ID = detalhesFat.EmpId,
                        REP_ID = contexto.REP_ID_QUE_EXECUTOU_ACAO,
                        USU_LOGIN = contexto.USU_LOGIN,
                        RFT_GERAR_NOTA_FISCAL = detalhesFat.GerarNotaFiscal,
                        RFT_DATA_FATURAMENTO = detalhesFat.DataFaturamento,
                        RFT_FATURAMENTO_EFETIVO = contrato.CTR_DATA_FATURAMENTO_EFETIVO
                    };

                    registros.Add(registroFaturamento);
                    
                }

                SaveOrUpdateAll(registros);
            }
        }
    }
}
