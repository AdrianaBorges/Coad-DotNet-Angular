﻿using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(cart_coad))]
    public partial class cart_coadDTO
    {
        public string CODIGO { get; set; }
        public string NOME { get; set; }
        public string A_C { get; set; }
        public string TIPO { get; set; }
        public string LOGRAD { get; set; }
        public string NUMERO { get; set; }
        public string TIPO_COMPL { get; set; }
        public string COMPL { get; set; }
        public string TIPO_COMPL2 { get; set; }
        public string COMPL2 { get; set; }
        public string TIPO_COMPL3 { get; set; }
        public string COMPL3 { get; set; }
        public string BAIRRO { get; set; }
        public string MUNIC { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string DDD_TEL { get; set; }
        public string TELEFONE { get; set; }
        public string DDD_FAX { get; set; }
        public string FAX { get; set; }
        public string E_MAIL { get; set; }
        public string CARGO { get; set; }
        public string PROF { get; set; }
        public string IDENTIFICACAO { get; set; }
        public string DATA_CADASTRO { get; set; }
        public string MXM_CODIGO { get; set; }
        public string cep_status { get; set; }
        public string TP_PESSOA { get; set; }
        public Nullable<int> id_ceps { get; set; }
        public string DH_SUBIR { get; set; }
        public string DH_SUBIU { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<System.DateTime> DATA_INSERT { get; set; }
        public Nullable<int> CLI_ID { get; set; }

        public virtual clienteLegDTO CLIENTES { get; set; }
    }
}
