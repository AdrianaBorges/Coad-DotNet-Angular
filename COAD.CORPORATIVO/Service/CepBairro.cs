using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class CepBairroSRV : GenericService<CEP_BAIRRO, CepBairroDTO, int>
    {
        public CepBairroDAO _dao = new CepBairroDAO();

        public CepBairroSRV()
        {
            this.Dao = _dao;
        }

        public IList<CepBairroDTO> Buscar(string _bairrodescricao)
        {
            return _dao.Buscar(_bairrodescricao);
        }


    }
}
