using System;
using System.Linq;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;

namespace COAD.SEGURANCA.Service
{
    public class CFOPSRV : GenericService<CFOP_TABLE, CFOTableDTO, string>
    {
        public IList<CFOTableDTO> Buscar(string _cfop_tipo)
        {
            return  new CFOPDAO().Buscar(_cfop_tipo);
        }
        public CFOTableDTO BuscarCFOPEntrada(string _cfopsaida)
        {
            return new CFOPDAO().BuscarCFOPEntrada(_cfopsaida);
        
        }
    }
}
