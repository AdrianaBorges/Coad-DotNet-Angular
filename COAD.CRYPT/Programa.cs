using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CRYPT
{
    public class Programa
    {
        public static void Main(string[] args)
        {
            var cryptService = new CryptService();

            Console.WriteLine("Gerando chaves....");
            //cryptService.GerarParDeChaves();
            //string encript = cryptService.Criptografar("i6HsHMkfKJ5/j8V2T5nNx42TX1OLq32aCCdUos8hvHyeqgGo+3gmvrE/MvMx6x2Pmdx/OWQmenatAtX9OS3Ujw==");
            //string decript = cryptService.Desencriptar(encript);

            string decript = cryptService.Desencriptar("i6HsHMkfKJ5/j8V2T5nNx42TX1OLq32aCCdUos8hvHyeqgGo+3gmvrE/MvMx6x2Pmdx/OWQmenatAtX9OS3Ujw==");

            Console.WriteLine("Encriptado");
            Console.WriteLine("Descripitando..");
            Console.WriteLine("Decripitado");
            Console.WriteLine(decript);
            Console.ReadLine();        
        }
    }
}
