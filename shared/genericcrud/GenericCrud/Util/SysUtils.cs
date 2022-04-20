using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Text.RegularExpressions;

namespace GenericCrud.Util
{
    public static class SysUtils
    {
        public static string DefaultPath { get; set; }

        //public static string RetirarCaracterSpecial(string texto)
        //{
        //    [ÓÒÕÔ],O

        //    string pattern = "(Mr\\.? |Mrs\\.? |Miss |Ms\\.? )";
        //    string[] names = { "Mr. Henry Hunt", "Ms. Sara Samuels", 
        //                 "Abraham Adams", "Ms. Nicole Norris" };
        //    foreach (string name in names)
        //        Console.WriteLine(Regex.Replace(name, pattern, String.Empty));

        //    var retorno = "";

        //    return retorno;
        //}
        /// <summary>
        /// Verifica de um campo texto é uma data válida
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDate(string date)
        {
            try
            {
                var dia = int.Parse(date.Substring(0, 2));
                var mes = int.Parse(date.Substring(3, 2));
                var ano = int.Parse(date.Substring(6, 4));

                DateTime dt = new DateTime(ano, mes, dia);

                //DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool InHomologation()
        {
            var configs = ConfigurationManager.AppSettings;
            if (configs != null)
            {
                var ambiente = configs["COADCORP.AMBIENTE"];

                if (!string.IsNullOrWhiteSpace(ambiente))
                {
                    return (ambiente == "DEV" || ambiente == "HOMOL");
                }
            }

            return false;
        }

        public static string RetornarAmbiente()
        {
            var configs = ConfigurationManager.AppSettings;
            if (configs != null)
            {
                var ambiente = configs["COADCORP.AMBIENTE"];

                if (!string.IsNullOrWhiteSpace(ambiente))
                {
                    return ambiente;
                }
            }

            return "DEV";
        }

        public static string RetornarAmbienteName()
        {
            var configs = ConfigurationManager.AppSettings;
            if (configs != null)
            {
                var ambiente = configs["COADCORP.AMBIENTE"];

                if (!string.IsNullOrWhiteSpace(ambiente))
                {
                    if (ambiente == "DEV")
                        return "Desenvolvimento";
                    if (ambiente == "HOMOL")
                        return "Homologação";
                }
            }
            return "@" + DateTime.Now.Year;            
        }

        public static string RetornarAmbienteNome()
        {
            var configs = ConfigurationManager.AppSettings;
            if (configs != null)
            {
                var ambiente = configs["COADCORP.AMBIENTE"];

                if (!string.IsNullOrWhiteSpace(ambiente))
                {
                    if (ambiente == "DEV")
                        return "DEV";
                    if (ambiente == "HOMOL")
                        return "HOMOL";
                }
            }
            return "@" + DateTime.Now.Year;
        }

        public static string AmbienteName()
        {
            var configs = ConfigurationManager.AppSettings;
            if (configs != null)
            {
                var ambiente = configs["COADCORP.AMBIENTE"];

                if (!string.IsNullOrWhiteSpace(ambiente))
                {
                    if (ambiente == "DEV")
                        return "Desenvolvimento";
                    if (ambiente == "HOMOL")
                        return "Homologação";
                }
            }
            return "@" + DateTime.Now.Year;
        }

        public static string RetornarAmbienteNameTotal()
        {
            var configs = ConfigurationManager.AppSettings;
            if (configs != null)
            {
                var ambiente = configs["COADCORP.AMBIENTE"];

                if (!string.IsNullOrWhiteSpace(ambiente))
                {
                    if (ambiente == "DEV")
                        return "Desenvolvimento";
                    if (ambiente == "HOMOL")
                        return "Homologação";
                }
            }
            return "Produção";
        }

        /// <summary>
        /// Pega uma string e substitui o token '{{ambiente}}' pelo nome do ambiente.
        /// </summary>
        /// <returns></returns>
        public static string FormatarPathComNomeAmbiente(string path)
        {
            var ambiente = SysUtils.RetornarAmbienteNameTotal();

            if (!string.IsNullOrWhiteSpace(ambiente) && !string.IsNullOrWhiteSpace(path))
            {
                ambiente = StringUtil.LimparAcentuacao(ambiente.ToLower());
                path = path.Replace("{{ambiente}}", ambiente);
            }

            return path;
        }

        public static string ReadConfig(string chave)
        {
            var configs = ConfigurationManager.AppSettings;
            string valor = null;

            if (configs != null)
            {
                valor = configs[chave];
            }

            return valor;
        }

        public static string RetornarPathNFeXML()
        {
            var path = ReadConfig("COADCORP.NFe.XML.UploadPath");

            if (!string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return "NFe";
        }

        public static string RetornarCoadPagURL()
        {
            var path = ReadConfig("COADPAG.CHECKOUT.PATH");

            if (!string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return null;
        }

        public static string RetornaEmailDeTeste()
        {
            var email = ReadConfig("COADCORP.TEST.MAIL");

            if (!string.IsNullOrWhiteSpace(email))
            {
                return email;
            }

            throw new Exception("Não existe chave 'COADCORP.TEST.MAIL' no appSettings");
        }

        /// <summary>
        /// Verifica se o sistema está em homologação e se a flag de email de teste está ativa. <para></para> 
        /// Em caso positivo devolve o email configurado no appSettings com a chave 'COADCORP.TEST.MAIL' <para></para>
        /// Se nenhuma dessas condições forem satisfeitas então é retornado o email proposto. <para></para>
        /// </summary>
        /// <param name="emailProposto"></param>
        public static string DecidirEnderecoDeEmail(string emailProposto){

            if(SysUtils.InHomologation() && SysUtils.EmailTesteAtivo()){

                return RetornaEmailDeTeste();
            }
            else{
                return emailProposto;
            }
        }

        public static string RetornarHostName()
        {
            var serverName = ReadConfig("COADCORP.SERVER.HOST-NAME");

            if (!string.IsNullOrWhiteSpace(serverName))
            {
                return serverName;
            }

            return null;
        }

        /// <summary>
        /// Verifica se o sistema irá enviar os emails para o endereco de teste
        /// </summary>
        /// <returns></returns>
        public static bool EmailTesteAtivo(){

            var ativo = ReadConfig("COADCORP.TEST.MAIL.ATIVAR");

            if (!string.IsNullOrWhiteSpace(ativo))
            {
                bool resp = false;

                if (bool.TryParse(ativo, out resp))
                {

                    return resp;
                }
                else
                {
                    if (ativo == "1")
                        return true;
                }
                
            }

            return true;
        }

        /// <summary>
        /// Recarrega as configurações
        /// </summary>
        public static void RecarregarConfiguracoes()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static int BuscarCodAdiquirente()
        {
            var _codigo = ReadConfig("COADCORP.ADQUIRENTE");

            if (!string.IsNullOrWhiteSpace(_codigo))
            {
                return  int.Parse(_codigo);
            }

            return 1;
        }

        public static bool useMinResouces = false;
        public static void InitParams()
        {
            if (ResourcesMinOff())
            {
                useMinResouces = false;
                return;
            }

            if (InHomologation())
            {
                useMinResouces = false;
            }
            else
            {
                useMinResouces = true;
            }
        }

        public static bool UseMinResources()
        {
            return useMinResouces;
        }


        /// <summary>
        /// Retorna se o uso de recursos estáticos deve ser sempre não
        /// minimizados (minify).
        /// </summary>
        /// <returns></returns>
        public static bool ResourcesMinOff()
        {
            var forcar = ReadConfig("COAD.RESOURCES.MINIFY.OFF");
            if (!string.IsNullOrWhiteSpace(forcar))
            {
                bool resp = false;

                if (bool.TryParse(forcar, out resp))
                {

                    return resp;
                }
                else
                {
                    if (forcar == "1")
                        return true;
                }

            }

            return true;
        }

        public static AmbienteEnum RetornarAmbienteEnum()
        {
            var configs = ConfigurationManager.AppSettings;
            if (configs != null)
            {
                var ambiente = configs["COADCORP.AMBIENTE"];

                if (!string.IsNullOrWhiteSpace(ambiente))
                {
                    if (ambiente == "DEV")
                        return AmbienteEnum.Dev;
                    if (ambiente == "HOMOL")
                        return AmbienteEnum.Homol;
                }
            }
            return AmbienteEnum.Prod;
        }

        public static string RetornarUrlDetalheNfse()
        {
            var path = ReadConfig("COADCORP.NFse.DETALHE.URL");

            if (!string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return null;
        }

        public static string RetornarUrlDANFENfe()
        {
            var path = ReadConfig("COADCORP.NFe.DANFE.URL");

            if (!string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return null;
        }

    }
}
