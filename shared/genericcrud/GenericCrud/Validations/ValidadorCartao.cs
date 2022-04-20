using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericCrud.Validations
{
    public static class ValidadorCartao
    {
        // Bandeiras
        //Visa = 1,
        //Mastercard = 2,
        //Hipercard = 3,
        //Amex = 4,
        //Diners = 5,
        //Elo = 6,
        //Aura = 7,
        //Discover = 8,
        //CasaShow = 9,
        //Havan = 10,
        //HugCard = 11,
        //AndarAki = 12,
        //LeaderCard = 13,
         
        public static int Validar(string numero)
        {
            int _retorno = 0;

            if (ValidarVisa(numero)) _retorno = 1; 
            if (_retorno==0 && ValidarMaster(numero)) _retorno = 2; 
            if (_retorno==0 && ValidarHipercard(numero)) _retorno = 3;  
            if (_retorno==0 && ValidarAmex(numero)) _retorno = 4; 
            if (_retorno==0 && ValidarDinersClub(numero)) _retorno = 5; 
            if (_retorno==0 && ValidarElo(numero)) _retorno = 6;  
            if (_retorno==0 && ValidarDiscover(numero)) _retorno = 8; 

            return _retorno;

        }
        private static bool ValidarVisa(string numero)
        {
            Regex _regEx = new Regex(@"^4[0-9]{12}(?:[0-9]{3})");
            
            _regEx.IsMatch(numero);

            return false;

        }
        private static bool ValidarMaster(string numero)
        {
            Regex _regEx = new Regex(@"^5[1-5][0-9]{14}");
            
            _regEx.IsMatch(numero);

            return false;

        }
        private static bool ValidarAmex(string numero)
        {
            Regex _regEx = new Regex(@"^3[47][0-9]{13}");
            
            _regEx.IsMatch(numero);

            return false;

        }
        private static bool ValidarDinersClub(string numero)
        {
            Regex _regEx = new Regex(@"^3(?:0[0-5]|[68][0-9])[0-9]{11}");
            
            _regEx.IsMatch(numero);

            return false;

        }
        private static bool ValidarDiscover(string numero)
        {
            Regex _regEx = new Regex(@"^6(?:011|5[0-9]{2})[0-9]{12}");
            
            _regEx.IsMatch(numero);

            return false;

        }
        private static bool ValidarJCB(string numero)
        {
            Regex _regEx = new Regex(@"^(?:2131|1800|35\d{3})\d{11}");
            
            _regEx.IsMatch(numero);

            return false;

        }
        private static bool ValidarHipercard(string numero)
        {
            Regex _regEx = new Regex(@"/^(606282\d{10}(\d{3})?)|(3841\d{15})$/");
            
            _regEx.IsMatch(numero);

            return false;

        }
        private static bool ValidarElo(string numero)
        {
            Regex _regEx = new Regex(@"/^((((636368)|(438935)|(504175)|(451416)|(636297))\d{0,10})|((5067)|(4576)|(4011))\d{0,12})$/");
            
            _regEx.IsMatch(numero);

            return false;

        }

        
        

    }
}
