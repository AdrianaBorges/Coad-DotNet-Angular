using COAD.CORPORATIVO.Model.Dto.Custons.WebService;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ImportacaoSuspect;
using COAD.CORPORATIVO.Service;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace COADSERVICE
{
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da classe "CoadServiceCursos" no arquivo de código, svc e configuração ao mesmo tempo.
    // OBSERVAÇÃO: Para iniciar o cliente de teste do WCF para testar esse serviço, selecione CoadServiceCursos.svc ou CoadServiceCursos.svc.cs no Gerenciador de Soluções e inicie a depuração.
    public class CoadServiceCursos : ICoadServiceCursos
    {
        public ValidatorWebServiceResult CadastrarClienteCurso(ClienteImportacaoWebServiceDTO cliente)
        {
            var result = new ValidatorWebServiceResult();
            try
            {
                ServiceFactory.RetornarServico<ImportacaoSRV>().AgendarImportacaoDiariaWebService(cliente);
                result.Success = true;
                result.Message = Coad.GenericCrud.ActionResultTools.Message.Success("Seu cadastro foi realizado com sucesso!!");

            }
            catch (Exception e)
            {
                var message = Coad.GenericCrud.ActionResultTools.Message.Fail(e, true);
                result.Success = false;
                result.Message = message;
                result.ValidationMessage = message.RetornarValidacoes();

            }
            return result;
        }
    }
}
