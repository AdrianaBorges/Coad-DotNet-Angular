using COAD.CORPORATIVO.Model.Dto.Custons.WebService;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ImportacaoSuspect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace COADSERVICE
{
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da interface "ICoadServiceCursos" no arquivo de código e configuração ao mesmo tempo.
    [ServiceContract]
    public interface ICoadServiceCursos
    {
        [OperationContract]
        ValidatorWebServiceResult CadastrarClienteCurso(ClienteImportacaoWebServiceDTO cliente);
    }
}
