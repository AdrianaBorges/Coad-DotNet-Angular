using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COAD.CORPORATIVO.Model
{
    public class Email
    {
            public string Id { get; set; }
            public string Assunto { get; set; }
            public string De { get; set; }
            public string Para { get; set; }
            public DateTime Data { get; set; }
            public string ConteudoTexto { get; set; }
            public string ConteudoHtml { get; set; }
    }
}