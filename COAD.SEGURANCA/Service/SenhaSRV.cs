using COAD.SEGURANCA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Service
{
    public class SenhaSRV
    {
        public SenhaDTO Senha { get; set; }
        private Random rand = new Random();

        private char[] letras = { 
            'a', 'b', 'c', 'd', 'e', 'f', 
            'g', 'h', 'i', 'j', 'k', 'l', 
            'm', 'n', 'o', 'p', 'q', 'r', 
            's', 't', 'u', 'w', 'v', 'x',
            'y','z'                      
        };

        public char[] Letras { get { return letras; } }

        private char[] caracteresEspeciais = { 
            '#', '#', '$', '%', '&', '*', 
            '_', '+', '?', '\\', '/', '}', 
            '{', ')', '('                   
        };

        public char[] CaracteresEspecial { get { return caracteresEspeciais; } }

        private string _gerarSenhaAleatoria(int numero)
        {
            StringBuilder sb = new StringBuilder();
            int firstEspecialCaractere = random(8);
            int secondEspecialCaractere = random(8);

            for (int i = 0; i < numero; i++)
            {
                object digito = string.Empty;
 
                if (i == firstEspecialCaractere)
                {
                    digito = generateEspecialCaractere();                    
                }
                else if (i == secondEspecialCaractere)
                {
                    digito = generateEspecialCaractere();
                }
                else
                {
                    digito = generateDigit();
                }

                sb.Append(digito);
            }

            return sb.ToString();
        }

        public int random(int maxvalue)
        {

            int numeroInt = rand.Next(maxvalue);
            return numeroInt;
        }

        /// <summary>
        /// Gera digito alfanumérico
        /// </summary>
        /// <returns></returns>
        public string generateDigit()
        {
            int numero = random(2); // se for 0 o digito é numérico

            if (numero == 1)
            {
                int digitoNumerico = random(10);
                return digitoNumerico.ToString();
            }
            else // senão digito alfabético
            {
                int charIndex = random(25); // sorteia a letra

                char charValue = Letras[charIndex]; 
                int upperCase = random(2); // maiúsculo ou minúsculo
                string stringValue = charValue.ToString();
                
                if (upperCase == 1)
                {
                    stringValue = stringValue.ToUpper();
                }

                return stringValue;
            }
        }

        public string generateEspecialCaractere()
        {
            int especialCaractereIndex = random(15);
            char especialCaracterer = CaracteresEspecial[especialCaractereIndex];

            return especialCaracterer.ToString();
        }

        public string HashMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public SenhaDTO GerarSenhaAleatoria(int numero = 8)
        {
            SenhaDTO senhaDTO = new SenhaDTO();
            var generateSenha = _gerarSenhaAleatoria(numero);

            senhaDTO.SenhaLiteral = generateSenha;
            senhaDTO.SenhaHash = HashMD5(generateSenha);

            return senhaDTO;
        }
    }

}
