using COAD.CORPORATIVO.Model;
using System;
using System.Collections.Generic;


namespace COAD.CORPORATIVO.Config
{
    public class EmailConfig
    {

        public List<Email> _emails = new List<Email>();
        public string _hostname = "pop.gmail.com"; // Host do seu servidor POP3. Por exemplo, pop.gmail.com para o servidor do Gmail.
        public int _port = 995; // Porta utilizada pelo host. Por exemplo, 995 para o servidor do Gmail.
        public bool _useSsl = true; // Indicação se o servidor POP3 utiliza SSL para autenticação. Por exemplo, o servidor do Gmail utiliza SSL, então, "true".
        public string _username = "recent:afrodrigues@coad.com.br"; // Usuário do servidor POP3. Por exemplo, seuemail@gmail.com.
        public string _password = "Q1w2e3r4!";

    }

}
