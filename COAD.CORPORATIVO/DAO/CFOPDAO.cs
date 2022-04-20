using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Extensions;

namespace COAD.CORPORATIVO.DAO
{
    public class CFOPDAO : AbstractGenericDao<CFOP_TABLE, CFOTableDTO, string> 
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CFOPDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public IList<CFOTableDTO> Buscar(string _cfop_tipo)
        {
            var _lista = db.CFOP_TABLE.Where(x => x.CFOP_TIPO == _cfop_tipo);

            foreach(var item in _lista)
            {
                item.CFOP_DESCRICAO = item.CFOP + " - " + item.CFOP_DESCRICAO;
            }

            return ToDTO(_lista);
        }
        public CFOTableDTO BuscarCFOPEntrada(string _cfopsaida)
        {

            var _cfop = (from c in db.CFOP_REFERENCIA
                         where c.CFOP_SAI == _cfopsaida
                         select c).FirstOrDefault();

            CFOTableDTO _cfoptable = new CFOTableDTO();
                 
             if (_cfop != null)
             {
                 _cfoptable.CFOP = _cfop.CFOP_TABLE.CFOP;
                 _cfoptable.CFOP_DESCRICAO = _cfop.CFOP_TABLE.CFOP + " - " + _cfop.CFOP_TABLE.CFOP_DESCRICAO;
             }

            return _cfoptable;
        }

    }
}
