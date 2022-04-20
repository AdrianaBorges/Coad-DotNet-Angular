using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Service.Mundipagg;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Xml;

namespace COADRESTSERVICE.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MundipaggCheckoutController : ControllerBase
    {

        public MundipaggCheckoutController()
        {
           
        }

        /*
        [HttpPost("mundipagg-return")]
        public string MundipaggReturn(string xmlStatusNotification)
        {


            //JSONResponse response = new JSONResponse();
            

            string dataok = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") 
                + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");

            if ( !Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "temp") )
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "temp");

            string _filePath = AppDomain.CurrentDomain.BaseDirectory + "temp" + Path.DirectorySeparatorChar + "post_" + dataok + ".txt";

            System.IO.File.WriteAllText(_filePath, xmlStatusNotification);

            xmlStatusNotification = HttpUtility.HtmlDecode(xmlStatusNotification);

            RetTransacaoSRV _service = new RetTransacaoSRV();

            try
            {

                PostSRV _pedidoSRV = new PostSRV();
                PostDTO pedido;
                TransactionOptions txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {

                    XmlDocument _doc = new XmlDocument();

                    _doc.LoadXml(xmlStatusNotification);

                    if ( _doc.GetElementsByTagName("OrderKey")[0] != null )
                    {

                        if (_pedidoSRV.BuscarPedidoPorTransacao(_doc.GetElementsByTagName("OrderKey")[0].InnerText) != null)

                            pedido = _pedidoSRV.BuscarPedidoPorTransacao(_doc.GetElementsByTagName("OrderKey")[0].InnerText);

                        else

                            pedido = new PostDTO();

                        XmlNodeList elemList;

                        pedido.MUP_ORDER_KEY = _doc.GetElementsByTagName("OrderKey")[0].InnerText;
                        pedido.MUP_ORDER_REFERENCE = _doc.GetElementsByTagName("OrderReference")[0].InnerText;
                        pedido.MUP_MERCHANT_KEY = _doc.GetElementsByTagName("MerchantKey")[0].InnerText;
                        pedido.MUP_ORDER_REFERENCE = _doc.GetElementsByTagName("OrderReference")[0].InnerText;
                        pedido.MUP_ORDER_KEY = _doc.GetElementsByTagName("OrderKey")[0].InnerText;
                        pedido.MUP_AMOUNT_IN_CENTS = int.Parse(_doc.GetElementsByTagName("AmountInCents")[0].InnerText);
                        pedido.MUP_AMOUNT_PAID_IN_CENTS = int.Parse(_doc.GetElementsByTagName("AmountPaidInCents")[0].InnerText);
                        pedido.MUP_ORDER_STATUS = _doc.GetElementsByTagName("OrderStatus")[0].InnerText;

                        elemList = _doc.GetElementsByTagName("BoletoTransaction");

                        if (( elemList != null ) && (elemList[0] != null) && ( elemList[0].HasChildNodes ) )
                        {

                            pedido.MUP_BOLETO_TRANSACTION_STATUS = SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "BoletoTransactionStatus");
                            pedido.MUP_STATUS_CHANGE_DATE = DateTime.Parse(SessionContext.RetornarDateTime(_doc, "BoletoTransaction", "StatusChangedDate").ToString());
                            pedido.MUP_TRANSACTION_KEY = SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "TransactionKey");
                            pedido.MUP_TRANSACTION_REFERENCE = SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "TransactionReference");
                            pedido.MUP_PREVIOUS_TRANSACTION_STATUS = SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "PreviousBoletoTransactionStatus");
                            pedido.MUP_STATUS_CHANGE_DATE = DateTime.Parse(SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "StatusChangedDate"));
                            pedido.MUP_BANK = int.Parse(SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "Bank"));
                            pedido.MUP_NOSSO_NUMERO = SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "NossoNumero");

                        }

                        elemList = _doc.GetElementsByTagName("CreditCardTransaction");

                        if ((elemList != null) && (elemList[0] != null) && (elemList[0].HasChildNodes))
                        {

                            pedido.MUP_AMOUNT_IN_CENTS = int.Parse((Convert.ToDecimal(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AmountInCents", "0"))).ToString());
                            pedido.MUP_AMOUNT_PAID_IN_CENTS = int.Parse(Convert.ToDecimal(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AuthorizedAmountInCents", "0")).ToString());
                            pedido.MUP_CREDIT_CARD_BRAND = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "CreditCardBrand");
                            pedido.MUP_STATUS_CHANGE_DATE = DateTime.Parse(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "StatusChangedDate").ToString());
                            pedido.MUP_AUTHORIZATION_CODE = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AuthorizationCode");
                            pedido.MUP_TRANSACTION_KEY = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "TransactionKey");
                            pedido.MUP_TRANSACTION_REFERENCE = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "TransactionReference");
                            pedido.MUP_TRANSACTION_IDENTIFIER = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "TransactionIdentifier");
                            pedido.MUP_UNIQUE_SEQUENTIAL_NUMBER = long.Parse(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "UniqueSequentialNumber"));
                            pedido.MUP_AMOUNT_IN_CENTS = int.Parse(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AmountInCents"));
                            pedido.MUP_ACQUIRER = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "Acquirer");
                            pedido.MUP_AUTHORIZED_AMOUNT_IN_CENTS = int.Parse(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AuthorizedAmountInCents"));
                            pedido.MUP_CAPTURED_AMOUNT_IN_CENTS = int.Parse(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "CapturedAmountInCents"));
                            pedido.MUP_AUTHORIZATION_CODE = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AuthorizationCode");
                            pedido.MUP_PREVIOUS_TRANSACTION_STATUS = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "PreviousCreditCardTransactionStatus").Substring(0, 12);
                            pedido.MUP_CREDIT_CARD_TRANSACTION_STATUS = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "CreditCardTransactionStatus");

                        }

                        try
                        {

                            _service.Save(pedido);

                        }
                        catch (Exception ex)
                        {

                            //response.message = Message.Fail("Falha ao atualizar ou inserir! - " + ex.ToString() + " - " + ex.StackTrace.ToString());

                            return ex.ToString() + " - " + ex.StackTrace.ToString();

                        }

                    }
                    
                    scope.Complete();

                }

                //response.message = Message.Success("Ok");

            }
            catch (Exception ex)
            {

                //response.message = Message.Fail("Fail" + " - " + ex.ToString() + " - " + ex.StackTrace.ToString());
                
                return ex.ToString() + " - " + ex.StackTrace.ToString();

            }

            return "Ok";

        }
        */
    }

}