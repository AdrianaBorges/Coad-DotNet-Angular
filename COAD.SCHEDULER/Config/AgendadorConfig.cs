    using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.CORPORATIVO.Config;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.CORPORATIVO.LEGADO.Config;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.PROSPECTADOS.Config;
using COAD.PROXY.Config;
using COAD.SEGURANCA.Config;
using COAD.SEGURANCA.Config.Email;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config;
using GenericCrud.Config.ConfigEnuns;
using GenericCrud.Config.IOCContainer;
using GenericCrud.Exceptions;
using GenericCrud.Service;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.AGENDADOR.Config
{
    public enum TipoAmbienteEnum
    {
        Dev = 0,
        Homologacao = 1,
        Producao = 2
    }

    public class DescricaoServicoDTO
    {
        public string NomeService { get; set; }
        public TipoAmbienteEnum TipoAmbienteEnum { get; set; }
        public string NomeDescricao { get; set; }
        public string Sufixo { get; set; }
        public string SufixoToken { get; set; }
    }

    public class AgendadorConfig
    {
        public static TipoAmbienteEnum Ambiente = TipoAmbienteEnum.Producao;

        public static DescricaoServicoDTO ObterDados()
        {
            switch(Ambiente)
            {
                case TipoAmbienteEnum.Dev : 
                    {
                        return new DescricaoServicoDTO(){
                            NomeService = "JobSchedulerDev",
                            TipoAmbienteEnum = TipoAmbienteEnum.Dev,
                            NomeDescricao = "[Desenvolvimento]",
                            Sufixo = "DEV",
                            SufixoToken = "[DEV]"
                        };                                               
                    }   
                case TipoAmbienteEnum.Homologacao :
                    {   
                        return new DescricaoServicoDTO(){
                            NomeService = "JobSchedulerHomol",
                            TipoAmbienteEnum = TipoAmbienteEnum.Homologacao,
                            NomeDescricao = "[Homologação]",
                            Sufixo = "HOMOL",
                            SufixoToken = "[HOMOL]"
                        };
                    }

               case TipoAmbienteEnum.Producao :
                    {   
                        return new DescricaoServicoDTO(){
                            NomeService = "JobSchedulerProd",
                            TipoAmbienteEnum = TipoAmbienteEnum.Producao,
                            NomeDescricao = "[Produção]",
                            Sufixo = "PROD",
                            SufixoToken = "[PROD]"
                        };
                    }
                default:
                return new DescricaoServicoDTO(){
                            NomeService = "JobSchedulerProd",
                            TipoAmbienteEnum = TipoAmbienteEnum.Producao,
                            NomeDescricao = "[Produção]",
                            Sufixo = "PROD",
                            SufixoToken = "[PROD]"
                        }; 
            }
        }

        public static void Configurar(EventLog log = null)
        {
            try
            {
                var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
                containerConfig.InstallFromAssembly<AgendadorConfig>();
                //
                IOCContainerProxy.DbContextLifestyle = LifeStyleTypeEnum.SCOPE;
                GenericCrudConfig.Configurar();
                CoadProxyConfig.Configurar();

                string defaultPath = Assembly.GetEntryAssembly().Location;
                defaultPath = Path.GetDirectoryName(defaultPath);
                SysUtils.DefaultPath = defaultPath;
                EmailActionContainer.AddActions("emailPropostaBoleto", parId =>
                {
                    return ServiceFactory
                        .RetornarServico<PropostaItemSRV>()
                        .RetornarBytesDoBoleto(parId);
                });
            }
            catch (Exception e)
            {
                if (log != null)
                {
                    var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                    log.WriteEntry(string.Format("Ocorreu um erro ao tentar configurar o serviço {0}", mensagem), EventLogEntryType.Error);
                }
                    throw new Exception("Ocorreu um erro ao tentar configurar o serviço", e);
            }

        }    

    }
}
