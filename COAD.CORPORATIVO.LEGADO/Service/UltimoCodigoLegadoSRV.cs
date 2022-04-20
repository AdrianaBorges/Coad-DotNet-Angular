using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.PROSPECTADOS.Dao;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Service
{
    [ServiceConfig("CODIGO2")]
    public class UltimoCodigoLegadoSRV : GenericService<ULTIMO_CODIGO, UltimoCodigoLegadoDTO, string>
    {
        private UltimoCodigoLegadoDAO _dao = new UltimoCodigoLegadoDAO();

        public UltimoCodigoLegadoSRV()
        {
            Dao = _dao;
        }

        /// <summary>
        /// Retorna o objeto ultimo código que representa o id gerado no banco para o prospect
        /// </summary>
        /// <returns></returns>
        public UltimoCodigoLegadoDTO GetUltimoCodigoGerado()
        {
            UltimoCodigoLegadoDTO codigo = new UltimoCodigoLegadoDTO();
            codigo.DV = "0";
            var codigoRetornado = Save(codigo);

            return codigoRetornado;
        }

        /// <summary>
        /// Gera o código do cliente
        /// </summary>
        /// <returns></returns>
        public decimal? GerarCodigo()
        {
            var codigo = GetUltimoCodigoGerado();
            if (codigo != null)
            {
                return codigo.CODIGO2;
            }
            return null;
        }
    }
}
