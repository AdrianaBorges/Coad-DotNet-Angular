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
    public class ClassificacaoSRV : ServiceAdapter<CLASSIFICACAO, ClassificacaoDTO, object>
    {
        private  ClassificacaoDAO _dao = new ClassificacaoDAO();

        public ClassificacaoSRV()
        {
            SetDao(_dao);
        }

    }
}
