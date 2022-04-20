using COAD.CORPORATIVO.Model.Dto;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Filters.Interface
{
    public interface IAccessControlMethod
    {
       bool HasAccess(Autenticado autendicado, string perId, int? repId, int? rgId , int? uenId);
    }
}
