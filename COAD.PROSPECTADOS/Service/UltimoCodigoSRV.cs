using Coad.GenericCrud.Service.Base;
using COAD.PROSPECTADOS.Dao;
using COAD.PROSPECTADOS.Model.Dto;
using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Service
{
    [ServiceConfig("CODIGO")]
    public class UltimoCodigoSRV : GenericService<ULTIMO_CODIGO, UltimoCodigoDTO, string>
    {
        private UltimoCodigoDAO _dao = new UltimoCodigoDAO();

        public UltimoCodigoSRV()
        {
            Dao = _dao;
        }

        /// <summary>
        /// Retorna o objeto ultimo código que representa o id gerado no banco para o prospect
        /// </summary>
        /// <returns></returns>
        public UltimoCodigoDTO GetUltimoCodigoGerado()
        {
            UltimoCodigoDTO codigo = new UltimoCodigoDTO();
            codigo.dv = "0";
            var codigoRetornado = Save(codigo);

            return codigoRetornado;
        }

        /// <summary>
        /// Gera o código do cliente
        /// </summary>
        /// <returns></returns>
        public int? GerarCodigo()
        {
            var codigo = GetUltimoCodigoGerado();
            if (codigo != null)
            {
                return codigo.codigo;
            }
            return null;
        }
    }
}
