using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class ClassificacaoConsumoSRV : ServiceCorp<CLASSICACAO_CONSUMO>
    {
        public List<CLASSICACAO_CONSUMO> BuscarPorTipo(int id)
        {
            return new ClassificacaoConsumoDAO().BuscarPorTipo(id);
        }
    }

}
