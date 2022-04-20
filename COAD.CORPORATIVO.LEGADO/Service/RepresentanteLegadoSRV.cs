using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("CODIGO")]
    public class RepresentanteLegadoSRV : GenericService<representante, RepresentanteLegadoDTO, string>
    {
        private RepresentanteLegadoDAO _dao = new RepresentanteLegadoDAO();

        public RepresentanteLegadoSRV()
        {
            Dao = _dao;
        }

        public string GetCodOperadorRepresentante(string codRep)
        {
            return _dao.GetCodOperadorRepresentante(codRep);
        }

        public ICollection<string> BuscarCodigosCarteiramento(string codRep)
        {
            return _dao.BuscarCodigosCarteiramento(codRep);
        }

        /// <summary>
        /// Busca representante por código de carteiramento
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public RepresentanteLegadoDTO BuscarPorCodigosCarteiramento(string carId)
        {
            return _dao.BuscarPorCodigosCarteiramento(carId);
        }
    }
}
