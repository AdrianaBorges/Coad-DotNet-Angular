using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model
{
    class DownloadRequest
    {
        public string tpAmb { get; set; }
        public string CNPJ { get; set; }
        public string chaveNfe { get; set; }
        public string downloadPath { get; set; }
    }

    /**
     * <downloadNFe  xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.00">
	        <tpAmb>1</tpAmb>
	        <CNPJ>27922913000111</CNPJ>
	        <chNFe>35150314314050000662550640000026721496332811</chNFe>
        </downloadNFe>

     */
}
