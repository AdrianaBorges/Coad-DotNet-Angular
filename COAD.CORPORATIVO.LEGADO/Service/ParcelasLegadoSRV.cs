using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.CORPORATIVO.LEGADO.Dao.Reflection;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("CONTRATO", "LETRA", "CD")]
    public class ParcelasLegadoSRV : GenericService<Parcelas, ParcelasLegadoDTO, string>
    {
        private ParcelasLegadoDAO _dao = new ParcelasLegadoDAO();

        public ParcelasLegadoSRV()
        {
            Dao = _dao;
        }

        /// <summary>
        /// ALT: 14/02/2017 - Procedure executando UPDATE em PARCELAS
        /// </summary>
        /// <param name="parcelas"></param>
        public void ExecutarUpdateEmParcelas(List<ParcelasLegadoUpdateDTO> parcelas)
        {
            using (var context = new corporativo2Entities())
            {
                context.ExecutarProcedure<ParcelasLegadoUpdateDTO>(parcelas, "PARCELA_BAIXAS_REGISTRONN", "@parcelas", "tp_Parcela_Baixas_RegistroNN");
            }
        }

        public IList<ParcelasLegadoDTO> LerParcelasLegado(string contrato = null, string letra = null, string cd = null, int pagina = 1, int itensPorPagina = 999999)
        {
            var resp = _dao.LerParcelasLegado(contrato, letra, cd, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarParcelaLegado(IEnumerable<ParcelasLegadoDTO> lstParcelas)
        {
            if (lstParcelas != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(lstParcelas);
            }
        }
    }
}
