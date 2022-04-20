using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.CORPSUSPECT.Service;
using COADCORP.Models;
using COAD.CORPSUSPECT.Repositorios;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Service.PedidoContexto;
using COAD.CORPORATIVO.Model.DTO;
using Coad.GenericCrud.ActionResultUtils;
using Coad.GenericCrud.Exceptions;

namespace COADCORP.Controllers.Lancamentos
{
    public class PrePedidoController : Controller
    {
        private FormaDePagamento _listaPagamentos = new FormaDePagamento();
        private TipoDeCliente _tipoDeCliente = new TipoDeCliente();
        private TiposDeNegociosDAO _tipoDeNegocio = new TiposDeNegociosDAO();
        private PrePedidoService _service = new PrePedidoService();
        private Empresas _empresa = new Empresas();

        public ActionResult Index()
        {
			var pedidos = (from x in new PrePedidoSRV().BuscarTodos() where x.PRE_STATUS_ID == 2 select x).OrderByDescending(x => x.PRE_PEDIDO_ID).ToList();
            return View(pedidos);
        }

        public ActionResult Novo()
        {
            ViewBag.tiposdenegocio = _tipoDeNegocio.RetornarTiposDeNegocio();
            ViewBag.tiposDePagamento = _listaPagamentos.RetornarCondicoesReferenteFormasDePagamento();
            ViewBag.tiposDeCliente = _tipoDeCliente.RetornarTiposDeCliente(0);
            ViewBag.empresasCadastradas = _empresa.RetornarEmpresas();
            ViewBag.brindes = new BrindeService().FindAll();
            return View();
        }

        [HttpPost]
        public ActionResult ValidarInformacoesIniciais(PrePedidoDTO prepedido) {
            JSONResponse js = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.ValidarInformacoesIniciais(prepedido);
                }
                else
                {
                    js.success = false;
                    js.SetMessageFromModelState(ModelState);
                }


            }
            catch (ValidacaoException e)
            {
                js.success = false;
                if (e.Validations.Count == 0)
                {
                    js.message = Message.Fail(e);
                }
                else
                {
                    js.SetMessageFromValidacaoException(e);
                }
            }
            catch (Exception e)
            {
                js.success = false;
                js.message = Message.Fail(e);
            }
            return Json(js);
        }
             

        [HttpPost]
        public ActionResult CadastrarPedido(PrePedidoDTO prepedidoDTO)
        {
            JSONResponse js = new JSONResponse();
            try

            {
                if (ModelState.IsValid)
                {
                    _service.SalvarPrepedido(prepedidoDTO);
                }
                else
                {
                    js.success = false;
                    js.SetMessageFromModelState(ModelState);
                }
               
            }          
            catch(Exception e)
            {
                js.success = false;
                js.message = Message.Fail(e);
            }
            return Json(js);
        }

        private bool ValidarFormasDePagamento(IList<FormaDePagamentoDTO> list, int qteDePedidos, JSONResponse response)
        {
            if (list.Count() != qteDePedidos)
                return false;

            foreach (FormaDePagamentoDTO formaDePagamento in list)
            {
               
                // validação inicial : verifica condição de pagamento e forma de pagamento
               if (formaDePagamento.condicaoDePagamento == null || formaDePagamento.condicaoDePagamento == "")
               {
                   response.message = Message.Fail("Selecione a condição de pagamento.");
                   return false;
               }
               else if(formaDePagamento.formapagtoprod == 0)
               {
                    response.message = Message.Fail("Selecione a forma de pagamento.");
                    return false;
               }               
              
                // validação específica para para condições de pagamento parcelado
               if(formaDePagamento.condicaoDePagamento == "1") // se a condição de pagamento for parcelado
               {
                   if (formaDePagamento.entrada == 0)
                   {
                       response.message = Message.Fail("Informe o valor de entrada.");
                       return false;
                   }

                   if (formaDePagamento.qteparcelas == 0)
                   {
                       response.message = Message.Fail("Especifique a uma quantidade de parcelas válida.");
                       return false;
                   }
                   if (formaDePagamento.valorparcelas == 0)
                   {
                       response.message = Message.Fail("Informe o valor das parcelas.");
                       return false;
                   }
                   if (formaDePagamento.vencsegparcela == null)
                   {
                       response.message = Message.Fail("Informe a data de vencimento para a 2ª parcela do cartão");
                       return false;
                   }                  
                   
               }

              // validação específica para forma de pagamento que não seja boleto
               if (formaDePagamento.formapagtoprod != 7 && formaDePagamento.formapagtoprod != 1) // se a forma de pagamento for diferente de boleto
               {
                    if (formaDePagamento.numdocumento == null)
                    {
                        response.message = Message.Fail("Preencha o número do documento");
                        return false;
                    }
                    else
                    if (formaDePagamento.bancoprod == null)
                    {
                        response.message = Message.Fail("Preencha o número do banco");
                        return false;
                    }
               }

               // validação específica para forma de pagamento com cheque
               if ((formaDePagamento.formapagtoprod == 3 || formaDePagamento.formapagtoprod == 8) && formaDePagamento.chequebompara == null)
               {
                   response.message = Message.Fail("Preencha a data do cheque bom para");
                   return false;
               }         
              
                // validações finais
               if (formaDePagamento.total == 0)
               {
                   response.message = Message.Fail("Ocorreu algum erro o total não foi especificado.");
                   return false;
               }                
                              
            }

            return true;
        }


        [HttpPost]
        public JsonResult RetornarFormasDePagamento(int tipoPagamento = 99)
        {
            var lista = _listaPagamentos.RetornarFormasDePagamento(tipoPagamento);
            return Json(lista);
        }

        [HttpPost]
        public JsonResult RecuperarProspect(string idProspect)
        {
            var prospect = new CartCoadSRV().BuscarPorId(idProspect);
            if (prospect != null)
            {
                prospect.TIPO_COMPL = prospect.TIPO_COMPL == "" || prospect.TIPO_COMPL == null ? "" : prospect.TIPO_COMPL;
                prospect.COMPL = prospect.COMPL == "" || prospect.COMPL == null ? "" : prospect.COMPL;
                prospect.TIPO_COMPL2 = prospect.TIPO_COMPL2 == "" || prospect.TIPO_COMPL2 == null ? "" : prospect.TIPO_COMPL2;
                prospect.COMPL2 = prospect.COMPL2 == "" || prospect.COMPL2 == null ? "" : prospect.COMPL2;
                prospect.TIPO_COMPL3 = prospect.TIPO_COMPL3 == "" || prospect.TIPO_COMPL3 == null ? "" : prospect.TIPO_COMPL3;
                prospect.COMPL3 = prospect.COMPL3 == "" || prospect.COMPL3 == null ? "" : prospect.COMPL3;
            }
            return Json(prospect);
        }

        [HttpPost]
        public JsonResult RecuperarProspectInformacoesAdicionais(string idProspect)
        {
            var prospectAdicionais = new ProspectsInformacoesAdicionaisSRV().BuscarPorId(idProspect);
            var informacoesadicionais = new { CPF_CNPJ = prospectAdicionais.CPF_CNPJ, INSCRICAO = prospectAdicionais.INSCRICAO };
            return Json(informacoesadicionais);
        }

        [HttpPost]
        public JsonResult RecuperarEmailProspect(string idProspect)
        {
            var emails = new EmailSRV().BuscarTelPorCodigoProspect(idProspect);
            return Json(emails);
        }

        [HttpPost]
        public JsonResult RecuperarTelefoneProspect(string idProspect)
        {
            var telefone = new TelefoneSRV().BuscarTelPorCodigoProspect(idProspect);
            return Json(telefone);
        }

        [HttpPost]
        public JsonResult RecuperarDadosDoCliente(Int32 idCliente)
        {
            var cliente = new ClienteSRV().BuscarPorId(idCliente);
            TelefoneClienteDTO telefoneCliente = new TelefoneClienteDTO();
            EmailClienteDTO emailCliente = new EmailClienteDTO();

            InformacoesClientePedido informacoesCliente = new InformacoesClientePedido();

            if (cliente != null)
            {
                var enderecoEntrega = new ClienteEnderecoSRV().BuscarEnderecosDeCliente(idCliente, 1);
                var enderecoFaturamento = new ClienteEnderecoSRV().BuscarEnderecosDeCliente(idCliente, 2);

                var assinatura = new AssinaturaSRV().RetornarAssinaturaDeCliente(cliente.CLI_ID);

                if (assinatura != null)
                {
                    var telefones = new TelefonesDeClienteSRV().BuscarTelefonesDeClientePorAssinatura(assinatura.ASN_NUM_ASSINATURA);
                    var emails = new EmailsDeClienteSRV().BuscarEmailsDeCliente(assinatura.ASN_NUM_ASSINATURA);

                    foreach (ASSINATURA_TELEFONE telefone in telefones)
                    {
                        telefoneCliente.assinatura = telefone.ASN_NUM_ASSINATURA;
                        telefoneCliente.idtelefone = telefone.ATE_ID.ToString();
                        telefoneCliente.telefone = telefone.ATE_TELEFONE;
                        telefoneCliente.tipo = telefone.TIPO_TEL_ID.ToString();
                        telefoneCliente.idsetor = telefone.OPC_ID;
                            if(telefone.OPCAO_ATENDIMENTO != null)
                        telefoneCliente.setor = telefone.OPCAO_ATENDIMENTO.OPC_DESCRICAO;
                        informacoesCliente.telefones.Add(telefoneCliente);
                    }

                    foreach (ASSINATURA_EMAIL email in emails)
                    {
                        emailCliente.assinatura = email.ASN_NUM_ASSINATURA;
                        emailCliente.idemail = email.AEM_ID.ToString();
                        emailCliente.email = email.AEM_EMAIL;

                        if (email.OPCAO_ATENDIMENTO != null)
                        {
                            emailCliente.idtipo = email.OPCAO_ATENDIMENTO.OPC_ID;
                            emailCliente.tipo = email.OPCAO_ATENDIMENTO.OPC_DESCRICAO;
                        }
                       
                        informacoesCliente.emails.Add(emailCliente);
                    }
                }

                informacoesCliente.clienteId = cliente.CLI_ID;
                informacoesCliente.nomeRazaoSocial = cliente.CLI_NOME;
                informacoesCliente.aosCuidados = cliente.CLI_A_C;
                informacoesCliente.tipoDeCliente = cliente.TIPO_CLI_ID.ToString();
                informacoesCliente.cpfCnpj = cliente.CLI_CPF_CNPJ;
                informacoesCliente.inscricaoEstadual = cliente.CLI_INSCRICAO;
                informacoesCliente.enderecoEntrega = (enderecoEntrega.TIPO_LOGRADOURO != null) ? enderecoEntrega.TIPO_LOGRADOURO.TIPO_LOG_DESCRICAO : "" + enderecoEntrega.END_LOGRADOURO;
                informacoesCliente.numeroEntrega = enderecoEntrega.END_NUMERO;
                informacoesCliente.complementoEntrega = enderecoEntrega.END_COMPLEMENTO;
                informacoesCliente.cepEntrega = enderecoEntrega.END_CEP;
                informacoesCliente.bairroEntrega = enderecoEntrega.END_BAIRRO;
                informacoesCliente.municipioEntrega = enderecoEntrega.END_MUNICIPIO;

                if(enderecoEntrega.MUNICIPIO != null)
                    informacoesCliente.ufEntrega = enderecoEntrega.MUNICIPIO.UF;

                if (enderecoFaturamento != null)
                {
                    informacoesCliente.enderecoFaturamento = (enderecoEntrega.TIPO_LOGRADOURO != null) ? enderecoEntrega.TIPO_LOGRADOURO.TIPO_LOG_DESCRICAO : "" + enderecoFaturamento.END_LOGRADOURO;
                    informacoesCliente.numeroFaturamento = enderecoFaturamento.END_NUMERO;
                    informacoesCliente.complementoFaturamento = enderecoFaturamento.END_COMPLEMENTO;
                    informacoesCliente.cepFaturamento = enderecoFaturamento.END_CEP;
                    informacoesCliente.bairroFaturamento = enderecoFaturamento.END_BAIRRO;
                    informacoesCliente.municipioFaturamento = enderecoFaturamento.END_MUNICIPIO;

                    if (enderecoEntrega.MUNICIPIO != null)
                        informacoesCliente.ufFaturamento = enderecoFaturamento.MUNICIPIO.UF;
                }
                else
                {
                    informacoesCliente.enderecoFaturamento = (enderecoEntrega.TIPO_LOGRADOURO != null) ? enderecoEntrega.TIPO_LOGRADOURO.TIPO_LOG_DESCRICAO : "" + enderecoEntrega.END_LOGRADOURO;
                    informacoesCliente.numeroFaturamento = enderecoEntrega.END_NUMERO;
                    informacoesCliente.complementoFaturamento = enderecoEntrega.END_COMPLEMENTO;
                    informacoesCliente.cepFaturamento = enderecoEntrega.END_CEP;
                    informacoesCliente.bairroFaturamento = enderecoEntrega.END_BAIRRO;
                    informacoesCliente.municipioFaturamento = enderecoEntrega.END_MUNICIPIO;

                    if (enderecoEntrega.MUNICIPIO != null)
                        informacoesCliente.ufFaturamento = enderecoEntrega.MUNICIPIO.UF;
                }
               
            }
            else
            {
                informacoesCliente = null;
            }

            return Json(informacoesCliente);
        }

        [HttpPost]
        public JsonResult RecuperarSetorDeEmail(int identificador)
        {
            var lista = new SetorAtendimento().RetornarSetorDeEmail(identificador);
            return Json(lista);
        }

        [HttpPost]
        public JsonResult RecuperarSetorDeTelefone(int identificador)
        {
            var lista = new SetorAtendimento().RetornarSetorDeTelefone(identificador);
            return Json(lista);
        }

        [HttpPost]
        public JsonResult RetornarTiposDeClientes(int tipoCliente = 0)
        {
            var lista = _tipoDeCliente.RetornarTiposDeCliente(tipoCliente);
            return Json(lista);
        }

        [HttpPost]
        public JsonResult RetornarTiposDeTelefone(string identificador)
        {
            var lista = new TiposDeTelefone().RetornarTiposDeTelefone(identificador);
            return Json(lista);
        }

        //[HttpPost]
        //public JsonResult RetornarProdutos(string identificador)
        //{
        //    var lista = new ProdutoComposicao().RetornarProdutosComposicao(identificador);
        //    return Json(lista);
        //}

        [HttpPost]
        public ActionResult ProdutosComposicao()
        {
            var lstProduto = new ProdutoComposicaoSRV().FindAllValid();
            JSONResponse js = new JSONResponse();
            js.Add("produtos", lstProduto);

            return Json(js);
        }

        public ActionResult Editar(int idPedido, int idEmp)
        {
            try
            {
                PrePedidoSRV prepedidoSRV = new PrePedidoSRV();
                var prepedido = prepedidoSRV.BuscarPorEmpIdEPrePedidoId(idPedido, idEmp);
                
                return View(prepedido);
            }
            catch
            {
                //js.message = Coad.GenericCrud.ActionResultUtils.Message.Success("Houve um erro ao processar sua requisição. Tente novamente ou chame o suporte.");
                return View();
            }
        }
		
		[HttpPost]
        public JsonResult ExcluirPedido(int idPedido, int idEmp)
        {

            JSONResponse js = new JSONResponse();
            js.success = false;
            try
            {
                PrePedidoSRV prepedidoSRV = new PrePedidoSRV();
                var prepedido = prepedidoSRV.BuscarPorEmpIdEPrePedidoId(idPedido, idEmp);

                if (prepedido != null)
                {
                    prepedidoSRV.InativarRegistro(idPedido, idEmp);
                    js.success = true;
                    js.message = Coad.GenericCrud.ActionResultUtils.Message.Success("Registro excluído com sucesso!!");
                }
                else
                {
                    js.success = false;
                    js.message = Coad.GenericCrud.ActionResultUtils.Message.Success("Registro não encontrado!!");
                }
                return Json(js);
            }
            catch(Exception e)
            {
                js.message = Coad.GenericCrud.ActionResultUtils.Message.Success("Houve um erro ao processar sua requisição. Tente novamente ou chame o suporte. ("+e.Message+")");
                return Json(js);
            }
        }
    }
}