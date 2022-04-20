using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class TipoTelefoneDAO : DAOAdapter<TIPO_TELEFONE, TipoTelefoneDTO, string>
    {
    }
}
