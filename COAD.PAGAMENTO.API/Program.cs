using COAD.PAGAMENTO.API.Model;
using COAD.PAGAMENTO.API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;

namespace COAD.PAGAMENTO.API
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                 Console.WriteLine("Iniciando...");

                            //bool ehSandBox = true;
                            //EnvironmentConfiguration.ChangeEnvironment(ehSandBox);

                            //// HardCoded
                            ////AccountCredentials conta = new AccountCredentials("dasilva@coad.com.br", "37E7315D9FBF47F48F431AACE72ECEEB"); 

                            //AccountCredentials conta = PagSeguroConfiguration.Credentials(ehSandBox);
                            //PaymentRequest pagamento = new PaymentRequest();

                            //pagamento.Items.Add(new Item("001", "Notebook Prata", 1, 2430.00m));
                            //pagamento.Items.Add(new Item("002", "Mochila", 1, 150.99m));

                            //pagamento.Sender = new Sender(
                
                            //    "Sr. Comprador", 
                            //    "c95625768024818435946@sandbox.pagseguro.com.br", 

                            //    new Phone(                    
                            //        "21",
                            //        "96273440"
                            //   )
                            //);

                            //pagamento.Shipping = new Shipping();
                            //pagamento.Shipping.ShippingType = ShippingType.Sedex;

                            //pagamento.Shipping.Address = new Address(
                
                            //    "BRA",
                            //    "RJ",
                            //    "São João de Meriti",
                            //    "Jardim Metropoles",
                            //    "244343-54",
                            //    "Av portugal",
                            //    "s/n",
                            //    "Lt. 29 Q.2"
                            //);

                            //pagamento.Currency = Currency.Brl;
                            //pagamento.Reference = "00332";

                            //Uri path = pagamento.Register(conta);

                            //Console.WriteLine("Path de pagamento :" + path);
                            Console.ReadLine();

                            EnvioService service = new EnvioService();
                            service.SetEnvironment(true);

                            Credentials cred = new Credentials("PWSHUSXNRIJSSA9NM93LQVH9MYH2FOOH", "VZEXVKC0CJQ51CNFVA994OOY0ZR5MJWE4HCCS6AS");
                            service.Enviar(cred);
            }
            catch (PagSeguroServiceException exception)
            {
                Console.WriteLine(exception.Message + "\n");

                foreach (Uol.PagSeguro.Domain.ServiceError element in exception.Errors)
                {
                    Console.WriteLine(element + "\n");
                }
                Console.ReadKey();
            }
           

        }
    }
}
