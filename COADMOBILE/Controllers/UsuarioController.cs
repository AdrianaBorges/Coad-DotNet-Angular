using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.PORTAL.Utils;
using COAD.SEGURANCA.Service;

namespace COADMOBILE.Controllers
{
    public class UsuarioController : Controller
    {
        //
        // GET: /Usuario/
        UtilsPortal _uc = new UtilsPortal();
        private AssinaturaSenhaSRV _assinaturasenhasrv = new AssinaturaSenhaSRV();
        private AssinaturaSRV _assinaturasrv = new AssinaturaSRV();
        private AssinaturaEmailSRV _assinaturaemailsrv = new AssinaturaEmailSRV();
        private AssinaturaTelefoneSRV _assinaturatelefonesrv = new AssinaturaTelefoneSRV();
        //ClienteSRV _serviceClientePC = new ClienteSRV();
        //EmailSRV _emailSRV = new EmailSRV();

        public ActionResult CadastroGratuito()
        {
            ViewBag.UFs = _uc.UFs();
            return View();
        }

        //[HttpPost]
        //public ActionResult CadastroGratuito(ClienteDTO cliDTO)
        //{
        //    try
        //    {
        //        if (cliDTO.cpf == null || cliDTO.nome == null || cliDTO.sobrenome == null
        //            || cliDTO.email == null || cliDTO.telefone == null || cliDTO.estado == null || 
        //            cliDTO.cpf == "" || cliDTO.nome == "" || cliDTO.sobrenome == ""
        //            || cliDTO.email == "" || cliDTO.telefone == "" || cliDTO.estado == "Selecione o UF") 
        //            ModelState.AddModelError("Error", "Existem campos em branco");
        //
        //        ViewBag.UFs = _uc.UFs();
        //
        //        if (ModelState.IsValid)
        //        {
        //            var usuario = _serviceClientePC.CadastrarUsuario(cliDTO);
        //            _emailSRV.EnviarEmail(_uc.MontarEmailDTOCadastroGratuito(usuario.nome, usuario.usuario, usuario.senha));
        //            TempData.Add("Resultado", "Cadastro efetuado com sucesso verifique seu email com os dados de cadastro.");
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("Error", ex.Message);
        //        //TempData.Add("Resultado", ex.Message);
        //    }
        //
        //    return View(cliDTO);
        //}

        [HttpPost]
        public ActionResult CadastroGratuito(ClienteDto cliDTO)
        {
            //var response = this.Request.CreateResponse(HttpStatusCode.OK);

            try
            {
                #region reuperando dados do header da requisição
                string empresa = Request.Headers.GetValues("empresa").ElementAt(0);
                var nomerazaosocial = Request.Headers.GetValues("nomerazaosocial").ElementAt(0);
                var cpfcnpj = Request.Headers.GetValues("cpfcnpj").ElementAt(0);
                var nomeresponsavel = "";
                var email = Request.Headers.GetValues("email").ElementAt(0).Replace(" ", "");
                var telefone = Request.Headers.GetValues("telefone").ElementAt(0).Replace(" ", "");
                var estado = Request.Headers.GetValues("estado").ElementAt(0);
                var endereco = Request.Headers.GetValues("endereco").ElementAt(0);
                var area_de_interesse = Request.Headers.GetValues("area_de_interesse").ElementAt(0);
                #endregion

                #region validação de dados recuparedos do header da requisição
                if (empresa.Equals("") || (empresa != "true" && empresa != "false"))
                {
                    //response.Content = new StringContent("{\"mensagem\":\"Opção empresa inválida.\"}", Encoding.UTF8, "application/json");
                    //return response;
                }

                if (nomerazaosocial.Equals(""))
                {
                   // if (empresa.Equals("true"))
                   //     response.Content = new StringContent("{\"mensagem\":\"Preencha o nome da empresa.\"}", Encoding.UTF8, "application/json");
                   // else
                   //     response.Content = new StringContent("{\"mensagem\":\"Preencha o campo nome.\"}", Encoding.UTF8, "application/json");
                   // return response;
                }


                if (empresa.Equals("true") && !ValidarCNPJ(cpfcnpj))
                {
                    //response.Content = new StringContent("{\"mensagem\":\"CNPJ inválido!\"}", Encoding.UTF8, "application/json");
                    //return response;
                }

                if (empresa.Equals("false") && !ValidarCPF(cpfcnpj))
                {
                    //response.Content = new StringContent("{\"mensagem\":\"CPF inválido!\"}", Encoding.UTF8, "application/json");
                    //return response;
                }


                if (empresa.Equals("true"))
                {
                   // nomeresponsavel = Request.Headers.GetValues("nomeresponsavel").ElementAt(0);
                    //if (nomeresponsavel.Equals(""))
                    //{
                    //    response.Content = new StringContent("{\"mensagem\":\"Informe o nome do respónsavel.\"}", Encoding.UTF8, "application/json");
                    //    return response;
                   // }
                }

                if (email.Equals("") || !Regex.IsMatch(email, (@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")))
                {
                    //response.Content = new StringContent("{\"mensagem\":\"Email inválido!\"}", Encoding.UTF8, "application/json");
                    //return response;
                }

                if (telefone.Equals("") || ValidarTelefone(telefone) == false)
                {
                    //response.Content = new StringContent("{\"mensagem\":\"Telefone inválido\"}", Encoding.UTF8, "application/json");
                    //return response;
                }

                if (estado.Equals(""))
                {
                   // response.Content = new StringContent("{\"mensagem\":\"Estado inválido!\"}", Encoding.UTF8, "application/json");
                   // return response;
                }

                //if (endereco.Equals(""))
                //{
                //    response.Content = new StringContent("{\"mensagem\":\"Preencha o campo endereço!\"}", Encoding.UTF8, "application/json");
                //    return response;
                //}

                if (area_de_interesse.Equals(""))
                {
                   // response.Content = new StringContent("{\"mensagem\":\"Escolha uma área de interesse!\"}", Encoding.UTF8, "application/json");
                   // return response;
                }
                #endregion

                ClienteSRV clientesrv = new ClienteSRV();
                AssinaturaSenhaDTO assinaturaSenha = new AssinaturaSenhaDTO();
                AssinaturaDTO assinaturaCliente = new AssinaturaDTO();
                BuscarClienteDTO cliente = null;
                AssinaturaEmailDTO aedto = new AssinaturaEmailDTO();
                AssinaturaTelefoneDTO atdto = new AssinaturaTelefoneDTO();
                ClienteDto cliente_salvo = new ClienteDto();
                AssinaturaDTO assinaturarecupareda = new AssinaturaDTO();
                ClienteDto cdto = null;
                //bool adicionartelefone = false;

                #region checa duplicação de email ou cpf por tipo de usuário
                var clientesPorEmail = clientesrv.BuscarClientesGeral(email: email);
                var clientesPorCPFCNPJ = clientesrv.BuscarClientesGeral(cpf_cnpj: cpfcnpj);

                if (clientesPorEmail != null && clientesPorEmail.lista != null && clientesPorEmail.lista.Count() > 0)
                {
                    //response.Content = new StringContent("{\"mensagem\":\"Este email já foi cadastrado!\"}", Encoding.UTF8, "application/json");
                    //return response;
                }

                if (clientesPorCPFCNPJ != null && clientesPorCPFCNPJ.lista != null && clientesPorCPFCNPJ.lista.Count() > 0)
                {
                    cliente = clientesPorCPFCNPJ.lista.FirstOrDefault();

                    assinaturaCliente = _assinaturasrv.FindPrimeiraAssinaturaPorCliente((int)cliente.CLI_ID, 51);
                    if (assinaturaCliente != null
                        && ((cliente.CLI_CPF_CNPJ.Length == 14 && empresa.Equals("true")) || (cliente.CLI_CPF_CNPJ.Length == 11 && empresa.Equals("false"))))
                    {
                        string docIdentificacao = empresa.Equals("true") ? "CNPJ" : "CPF";
                        //response.Content = new StringContent("{\"mensagem\":\"Este " + docIdentificacao + " já foi cadastrado!\"}", Encoding.UTF8, "application/json");
                        //return response;
                    }
                }
                #endregion


                bool clientenovo = false;
                if (cliente == null)
                {
                    cdto = new ClienteDto();
                    cdto.CLI_NOME = nomerazaosocial;
                    cdto.CLI_A_C = nomeresponsavel + telefone;
                    cdto.CLI_TP_PESSOA = empresa.Equals("true") ? "J" : "F";
                    cdto.CLI_CPF_CNPJ = cpfcnpj;
                    cdto.DATA_CADASTRO = DateTime.Now;
                    cdto.USU_LOGIN = "SISTEMA";
                    cdto.TIPO_CLI_ID = 1;
                    cdto.CLI_COD_PAIS = "1058";
                    cdto.CLA_CLI_ID = 3;
                    cdto.CLI_EMAIL = email;

                    assinaturarecupareda = new AssinaturaDTO();
                    cdto.ASSINATURA.Add(assinaturarecupareda);
                    clientenovo = true;
                }
                else
                {
                    cdto = clientesrv.FindByIdFullLoadedGeneral((int)cliente.CLI_ID, 51, false, true, true);
                    assinaturarecupareda = _assinaturasrv.ExtrairAssinaturaCliente(cdto, 51, true, true, true);

                    if (assinaturarecupareda == null)
                    {

                        assinaturarecupareda = new AssinaturaDTO();
                        cdto.ASSINATURA.Add(assinaturarecupareda);
                        assinaturarecupareda = _assinaturasrv.GerarAssinatura(cdto, 51);
                    }

                }

                assinaturarecupareda.ASSINATURA_EMAIL.Add(aedto);
                assinaturarecupareda.ASSINATURA_TELEFONE.Add(atdto);

                cliente_salvo = clientesrv.SalvarClienteEInformacoesDeMarketingSemRodizio(cdto, 51);

                assinaturaSenha.ASN_ATIVO = true;
                assinaturaSenha.ASN_DATA_CADASTRO = DateTime.Now;
                assinaturaSenha.ASN_DATA_ALTERA = DateTime.Now;

                var assinaturanova = _assinaturasrv.BuscarAssinaturaPorCLIID((int)cliente_salvo.CLI_ID);

                assinaturaSenha.ASN_NUM_ASSINATURA = clientenovo ? assinaturanova.ASN_NUM_ASSINATURA : assinaturarecupareda.ASN_NUM_ASSINATURA;

                int inteiro = 0;
                if (int.TryParse(_assinaturasenhasrv.pegarUltimasenha(), out inteiro))
                    assinaturaSenha.ASN_SENHA = (inteiro + 1).ToString();

                atdto.ASN_NUM_ASSINATURA = assinaturaSenha.ASN_NUM_ASSINATURA;
                atdto.ATE_DDD = telefone.Substring(0, 2);
                atdto.ATE_TELEFONE = telefone.Length == 10 ? telefone.Substring(2, 8) : telefone.Substring(2, 9);
                atdto.TIPO_TEL_ID = 0;

                aedto.ASN_NUM_ASSINATURA = assinaturaSenha.ASN_NUM_ASSINATURA;
                aedto.AEM_EMAIL = email;

                _assinaturaemailsrv.Save(aedto);
                //_assinaturatelefonesrv.Save(atdto);
                _assinaturasenhasrv.Save(assinaturaSenha);

                string clienteJSON = "";
                clienteJSON += "\"nome\":\"" + nomerazaosocial + "\",";
                using (MD5 md5Hash = MD5.Create())
                {
                    clienteJSON += "\"senha\":\"" + COAD.SEGURANCA.Repositorios.Base.SessionContext.HashMD5(assinaturaSenha.ASN_SENHA) + "\",";
                }
                clienteJSON += "\"email\":\"" + email + "\",";
                clienteJSON += "\"cpf\":\"" + cpfcnpj + "\",";
                clienteJSON += "\"chave\":\"sdfdsgrt346trgxcbi9inuh95uth\",";
                clienteJSON += "\"telefone\":\"" + telefone + "\",";
                clienteJSON += "\"validade\":\"17/06/2016\",";
                clienteJSON += "\"login\":\"" + assinaturaSenha.ASN_NUM_ASSINATURA + "\",";
                clienteJSON += "\"login2\":\"" + assinaturaSenha.ASN_NUM_ASSINATURA + "\"";

                //response.Content = new StringContent("{\"mensagem\":\"Sucesso.\"," + clienteJSON + "}", Encoding.UTF8, "application/json");
                //return response;
            }
            catch (InvalidOperationException e)
            {
                //response.Content = new StringContent("{\"mensagem\":\"Por favor preencha todos os campos.\"}", Encoding.UTF8, "application/json");
                //return response;
            }
            catch (Exception e)
            {
                //response.Content = new StringContent("{\"mensagem\":\"Ocorreu um erro na sua requisição.\"}", Encoding.UTF8, "application/json");
                //return response;
            }
            return View(cliDTO);
        }

        private bool ValidarCNPJ(string vrCNPJ)
        {
            string CNPJ = vrCNPJ.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");
            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;
            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));

                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));

                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);

                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                    else
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                } return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }

        private bool ValidarCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        private bool ValidarTelefone(string numero)
        {
            long check = 0;
            if (numero == null || numero.Length < 10 || numero.Length > 11)
                return false;

            string ddd = numero.Substring(0, 2);
            List<string> ddds = new List<string>();
            ddds.Add("11"); ddds.Add("12"); ddds.Add("13"); ddds.Add("14"); ddds.Add("15"); ddds.Add("16"); ddds.Add("17"); ddds.Add("18"); ddds.Add("19");
            ddds.Add("21"); ddds.Add("22"); ddds.Add("24"); ddds.Add("27"); ddds.Add("28"); ddds.Add("31"); ddds.Add("32"); ddds.Add("33"); ddds.Add("34");
            ddds.Add("35"); ddds.Add("37"); ddds.Add("38"); ddds.Add("41"); ddds.Add("42"); ddds.Add("43"); ddds.Add("44"); ddds.Add("45"); ddds.Add("46");
            ddds.Add("47"); ddds.Add("48"); ddds.Add("49"); ddds.Add("51"); ddds.Add("53"); ddds.Add("54"); ddds.Add("55"); ddds.Add("61"); ddds.Add("62");
            ddds.Add("63"); ddds.Add("64"); ddds.Add("65"); ddds.Add("66"); ddds.Add("67"); ddds.Add("68"); ddds.Add("69"); ddds.Add("71"); ddds.Add("73");
            ddds.Add("74"); ddds.Add("75"); ddds.Add("77"); ddds.Add("79"); ddds.Add("81"); ddds.Add("82"); ddds.Add("83"); ddds.Add("84"); ddds.Add("85");
            ddds.Add("86"); ddds.Add("87"); ddds.Add("88"); ddds.Add("89"); ddds.Add("91"); ddds.Add("92"); ddds.Add("93"); ddds.Add("94"); ddds.Add("95");
            ddds.Add("96"); ddds.Add("97"); ddds.Add("98"); ddds.Add("99");

            if (!ddds.Contains(ddd))
                return false;

            if (!long.TryParse(numero, out check))
                return false;

            return true;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
