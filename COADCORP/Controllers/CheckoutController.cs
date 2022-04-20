using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace COADCORP.Controllers
{
    public class CheckoutController : Controller
    {
        public ActionResult mundipaggreturn(string xmlStatusNotification)
        {

            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            Autenticado _autenticado = new Autenticado();
            System.Web.HttpContext.Current.Session.Timeout = 240;

            _autenticado.IP_ACESSO = SessionContext.GetIp();
            _autenticado.PATH = url;
            _autenticado.SESSION_ID = System.Web.HttpContext.Current.Session.SessionID;
            _autenticado.SESSION_TIMEOUT = System.Web.HttpContext.Current.Session.Timeout;
            _autenticado.SESSION_TIMEOUT_RESTANTE = System.Web.HttpContext.Current.Session.Timeout;
            _autenticado.SIS_ID = "COADCORP";
            _autenticado.EMP_ID = 1;
            _autenticado.ADMIN = false;
            _autenticado.EMAIL = "TI@Coad.com.br";
            _autenticado.USU_LOGIN = "MOBILEUSER";
            _autenticado.DATA_LOGIN = DateTime.Now;
            _autenticado.MEIO_ACESSO = "POST/NOTIF";

            SessionContext.autenticado = _autenticado;

            //--Boleto
//            xmlStatusNotification = @"&lt;StatusNotification&gt;
//                                      &lt;AmountInCents&gt;41461&lt;/AmountInCents&gt;
//                                      &lt;AmountPaidInCents&gt;0&lt;/AmountPaidInCents&gt;
//                                      &lt;BoletoTransaction&gt;
//                                        &lt;AmountInCents&gt;41461&lt;/AmountInCents&gt;
//                                        &lt;AmountPaidInCents&gt;41461&lt;/AmountPaidInCents&gt;
//                                        &lt;Bank&gt;341&lt;/Bank&gt;
//                                        &lt;BoletoExpirationDate&gt;2017-03-19T10:28:33.893&lt;/BoletoExpirationDate&gt;
//                                        &lt;NossoNumero&gt;01810026&lt;/NossoNumero&gt;
//                                        &lt;StatusChangedDate&gt;2017-03-21T06:56:17.5&lt;/StatusChangedDate&gt;
//                                        &lt;TransactionKey&gt;9c43fe4f-43b9-4041-abd1-b047040552f6&lt;/TransactionKey&gt;
//                                        &lt;TransactionReference&gt;263&lt;/TransactionReference&gt;
//                                        &lt;PreviousBoletoTransactionStatus&gt;Generated&lt;/PreviousBoletoTransactionStatus&gt;
//                                        &lt;BoletoTransactionStatus&gt;Paid&lt;/BoletoTransactionStatus&gt;
//                                      &lt;/BoletoTransaction&gt;
//                                      &lt;CreditCardTransaction /&gt;
//                                      &lt;MerchantKey&gt;a950ebd1-49f2-4f7b-9a6d-5e221eb2c1f1&lt;/MerchantKey&gt;
//                                      &lt;OrderKey&gt;e95b5a0b-e141-4e52-952b-991982913ba1&lt;/OrderKey&gt;
//                                      &lt;OrderReference&gt;263&lt;/OrderReference&gt;
//                                      &lt;OrderStatus&gt;Paid&lt;/OrderStatus&gt;
//                                    &lt;/StatusNotification&gt;";

            //--Cartão
            //xmlStatusNotification = @"&lt;StatusNotification&gt; " +
            //                         "    &lt;AmountInCents&gt;100&lt;/AmountInCents&gt; " +
            //                         "    &lt;AmountPaidInCents&gt;0&lt;/AmountPaidInCents&gt; " +
            //                         "    &lt;BoletoTransaction /&gt; " +
            //                         "    &lt;CreditCardTransaction&gt; " +
            //                         "      &lt;Acquirer&gt;Simulator&lt;/Acquirer&gt; " +
            //                         "      &lt;AmountInCents&gt;100&lt;/AmountInCents&gt; " +
            //                         "      &lt;AuthorizationCode /&gt; " +
            //                         "      &lt;AuthorizedAmountInCents /&gt; " +
            //                         "      &lt;CapturedAmountInCents /&gt; " +
            //                         "      &lt;CreditCardBrand&gt;Visa&lt;/CreditCardBrand&gt; " +
            //                         "      &lt;CustomStatus /&gt; " +
            //                         "      &lt;RefundedAmountInCents /&gt; " +
            //                         "      &lt;StatusChangedDate&gt;2016-12-27T14:38:13.903&lt;/StatusChangedDate&gt; " +
            //                         "      &lt;TransactionIdentifier /&gt; " +
            //                         "      &lt;TransactionKey&gt;c5349d31-e2f5-4e9a-8313-32dac27a9b20&lt;/TransactionKey&gt; " +
            //                         "      &lt;TransactionReference&gt;37f6f651-647a-4146-99a3-02dd64d62aa6&lt;/TransactionReference&gt; " +
            //                         "      &lt;UniqueSequentialNumber /&gt; " +
            //                         "      &lt;VoidedAmountInCents /&gt; " +
            //                         "      &lt;PreviousCreditCardTransactionStatus&gt;PendingAuthorize&lt;/PreviousCreditCardTransactionStatus&gt; " +
            //                         "      &lt;CreditCardTransactionStatus&gt;Voided&lt;/CreditCardTransactionStatus&gt; " +
            //                         "    &lt;/CreditCardTransaction&gt; " +
            //                         "    &lt;MerchantKey&gt;a950ebd1-49f2-4f7b-9a6d-5e221eb2c1f1&lt;/MerchantKey&gt; " +
            //                         "    &lt;OrderKey&gt;3abaea75-44f5-4803-b9da-651ee59b0de2&lt;/OrderKey&gt; " +
            //                         "    &lt;OrderReference&gt;dd5bcc5a&lt;/OrderReference&gt; " +
            //                         "    &lt;OrderStatus&gt;Opened&lt;/OrderStatus&gt; " +
            //                         "  &lt;/StatusNotification&gt;";


            //            xmlStatusNotification = @"<StatusNotification xmlns='http://schemas.datacontract.org/2004/07/gateway.NotificationService.DataContract'
            //                                        xmlns:i='http://www.w3.org/2001/XMLSchema-instance'
            //                                        i:schemaLocation='http://schemas.datacontract.org/2004/07/gateway.NotificationService.DataContract StatusNotificationXmlSchema.xsd'>
            //                                        <AmountInCents>500</AmountInCents>
            //                                        <AmountPaidInCents>0</AmountPaidInCents>
            //                                        <BoletoTransaction>
            //                                            <AmountInCents>500</AmountInCents>
            //                                            <AmountPaidInCents>0</AmountPaidInCents>
            //                                            <BoletoExpirationDate>2013-02-08T00:00:00</BoletoExpirationDate>
            //                                            <NossoNumero>0123456789</NossoNumero>
            //                                            <StatusChangedDate>2012-11-06T08:55:49.753</StatusChangedDate>
            //                                            <TransactionKey>4111D523-9A83-4BE3-94D2-160F1BC9C4BD</TransactionKey>
            //                                            <TransactionReference>B2E32108</TransactionReference>
            //                                            <PreviousBoletoTransactionStatus>Generated</PreviousBoletoTransactionStatus>
            //                                            <BoletoTransactionStatus>Paid</BoletoTransactionStatus>
            //                                        </BoletoTransaction>
            //                                        <CreditCardTransaction>
            //                                            <Acquirer>Simulator</Acquirer>
            //                                            <AmountInCents>2000</AmountInCents>
            //                                            <AuthorizedAmountInCents>2000</AuthorizedAmountInCents>
            //                                            <CapturedAmountInCents>2000</CapturedAmountInCents>
            //                                            <CreditCardBrand>Visa</CreditCardBrand>
            //                                            <RefundedAmountInCents i:nil='true'/>
            //                                            <StatusChangedDate>2012-11-06T10:52:55.93</StatusChangedDate>
            //                                            <TransactionIdentifier>123456</TransactionIdentifier>
            //                                            <TransactionKey>351FC96A-7F42-4269-AF3C-1E3C179C1CD0</TransactionKey>
            //                                            <TransactionReference>24de0432</TransactionReference>
            //                                            <UniqueSequentialNumber>123456</UniqueSequentialNumber>
            //                                            <VoidedAmountInCents i:nil='true'/>
            //                                            <PreviousCreditCardTransactionStatus>AuthorizedPendingCapture</PreviousCreditCardTransactionStatus>
            //                                            <CreditCardTransactionStatus>Captured</CreditCardTransactionStatus>
            //                                        </CreditCardTransaction>
            //                                        <!--O nó OnlineDebitTransaction só é enviado caso uma transação de débito esteja sendo notificada-->
            //                                        <OnlineDebitTransaction>
            //                                            <AmountInCents>100</AmountInCents>
            //                                            <AmountPaidInCents>0</AmountPaidInCents>
            //                                            <StatusChangedDate>2013-06-27T19:46:46.87</StatusChangedDate>
            //                                            <TransactionKey>fb3f158a-0309-4ae3-b8ef-3c5ac2d603d2</TransactionKey>
            //                                            <TransactionReference>30bfee13-c908-4e3b-9f70-1f84dbe79fbf</TransactionReference>
            //                                            <PreviousOnlineDebitTransactionStatus>OpenedPendingPayment</PreviousOnlineDebitTransactionStatus>
            //                                            <OnlineDebitTransactionStatus>NotPaid</OnlineDebitTransactionStatus>
            //                                        </OnlineDebitTransaction>
            //                                        <!--O nó OnlineDebitTransaction só é enviado caso uma transação de débito esteja sendo notificada-->
            //                                        <MerchantKey>B1B1092C-8681-40C2-A734-500F22683D9B</MerchantKey>
            //                                        <OrderKey>18471F05-9F6D-4497-9C24-D60D5BBB6BBE</OrderKey>
            //                                        <OrderReference>64a85875</OrderReference>
            //                                        <OrderStatus>Paid</OrderStatus>
            //                                    </StatusNotification>";


            var dataok = DateTime.Now;

            string _filePath = HttpContext.Server.MapPath("~/temp/") + "post"+dataok.Minute.ToString()+dataok.Millisecond.ToString()+".txt";

            System.IO.File.WriteAllText(_filePath, xmlStatusNotification);

            xmlStatusNotification = HttpUtility.HtmlDecode(xmlStatusNotification);

            RetornoTransacaoSRV _service = new RetornoTransacaoSRV();

            try
            {
                ItemPedidoSRV _pedidoSRV = new ItemPedidoSRV();
                TransactionOptions txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {

                    XmlDocument _doc = new XmlDocument();

                    _doc.LoadXml(xmlStatusNotification);

                    XmlNodeList elemList = _doc.GetElementsByTagName("BoletoTransaction");

                    var _ret = new RetornoTransacaoDTO();

                    if (elemList[0].HasChildNodes)
                    {
                        _ret.RTT_VLR_TOTAL = Convert.ToDecimal(SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "AmountInCents", "0"));
                        _ret.RTT_VLR_PAGO = Convert.ToDecimal(SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "AmountPaidInCents", "0"));
                        _ret.RTT_NOSSO_NUMERO = SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "NossoNumero");
                        _ret.RTT_STATUS = SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "BoletoTransactionStatus");
                        _ret.RTT_STATUS_ANT = SessionContext.RetornaStringDoCampo(_doc, "BoletoTransaction", "PreviousBoletoTransactionStatus");
                        _ret.RTT_STATUS_CHANGE = SessionContext.RetornarDateTime(_doc, "BoletoTransaction", "StatusChangedDate");
                        _ret.AUTHORIZATION_CODE = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AuthorizationCode");
                        _ret.TPG_ID = 1;

                        if (_ret.RTT_VLR_TOTAL > 0)
                            _ret.RTT_VLR_TOTAL = (_ret.RTT_VLR_TOTAL / 100);

                        if (_ret.RTT_VLR_PAGO > 0)
                            _ret.RTT_VLR_PAGO = (_ret.RTT_VLR_PAGO / 100);

                    }

                    elemList = _doc.GetElementsByTagName("CreditCardTransaction");

                    if (elemList[0].HasChildNodes)
                    {
                        _ret.RTT_ADIQUIRENTE = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "Acquirer");
                        _ret.RTT_VLR_TOTAL = Convert.ToDecimal(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AmountInCents", "0"));
                        _ret.RTT_VLR_AUROTIZADO = Convert.ToDecimal(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AuthorizedAmountInCents", "0"));
                        _ret.RTT_VLR_PAGO = Convert.ToDecimal(SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "CapturedAmountInCents", "0"));
                        _ret.RTT_BANDEIRA = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "CreditCardBrand");
                        _ret.RTT_STATUS = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "CreditCardTransactionStatus");
                        _ret.RTT_STATUS_ANT = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "PreviousCreditCardTransactionStatus");
                        _ret.RTT_STATUS_CHANGE = SessionContext.RetornarDateTime(_doc, "CreditCardTransaction", "StatusChangedDate");
                        _ret.AUTHORIZATION_CODE = SessionContext.RetornaStringDoCampo(_doc, "CreditCardTransaction", "AuthorizationCode");
                        _ret.TPG_ID = 9;

                        if (_ret.RTT_VLR_TOTAL > 0)
                            _ret.RTT_VLR_TOTAL = (_ret.RTT_VLR_TOTAL / 100);

                        if (_ret.RTT_VLR_PAGO > 0)
                            _ret.RTT_VLR_PAGO = (_ret.RTT_VLR_PAGO / 100);

                        if (_ret.RTT_VLR_AUROTIZADO > 0)
                            _ret.RTT_VLR_AUROTIZADO = (_ret.RTT_VLR_AUROTIZADO / 100);

                    }

                    _ret.ORDER_KEY = _doc.GetElementsByTagName("OrderKey")[0].InnerText;
                    _ret.ORDER_KEY_REF = _doc.GetElementsByTagName("OrderReference")[0].InnerText;
                    _ret.RTT_DATA_INCLUSAO = DateTime.Now;
                    _ret.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;

                    var _pedido = _pedidoSRV.BuscarPedidoPorTransacao(_ret.ORDER_KEY);

                    try
                    {

                        if (_pedido != null)
                        {
                            _ret.IPE_ID = _pedido.IPE_ID;

                            var _alteracao = _pedidoSRV.RetornarDadosDePagamento(_pedido.IPE_ID);
                            
                            if (_alteracao != null)
                            {
                                _alteracao.DataLiquidacao = SessionContext.RetornarDateTime(_doc, "BoletoTransaction", "StatusChangedDate");

                                _pedidoSRV.PagarPedido(_alteracao);
                                _ret.RTT_MENSAGEM = "Operação realizada com sucesso!";
                            }
                            else
                                _ret.RTT_MENSAGEM = "Pedido não encontrado. Operação Não realizada!";
                        }
                        else
                            _ret.RTT_MENSAGEM = "Pedido não encontrado. Operação Não realizada!";

                    }
                    catch (Exception ex)
                    {
                        _ret.RTT_MENSAGEM = ex.Message;

                        SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);

                    }

                    _service.Save(_ret);

                    scope.Complete();

                }

                SysException.RegistrarLog("Operação realizada (mundipaggreturn)", "", SessionContext.autenticado);

            }
            catch (Exception ex)
            {

                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);

            }

            return View();
        }
        public ActionResult Pagamento(string id, string interno)
        {
            var _path = SysUtils.RetornarCoadPagURL();

            if (!string.IsNullOrWhiteSpace(_path))
                _path = _path + id + "/" + interno;
            else
                _path = "/home/Erro";

            return Redirect(_path);
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
