using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoNet;

namespace COAD.CORPORATIVO.Model.Dto.Boleto
{
    /// <summary>
    /// ALT: 11/10/2016 - Espécie do Documento.
    /// </summary>
    public class EspecieDocumentoDTO :  AbstractEspecieDocumento, IEspecieDocumento
    {
        public EspecieDocumentoDTO(string banco, string codigo)
        {
            switch (banco)
            {
                case "001":
                    EspecieDocumento_BancoBrasil bb = new EspecieDocumento_BancoBrasil(codigo);
                    base.Banco = bb.Banco;
                    base.Codigo = bb.Codigo;
                    base.Especie = bb.Especie;
                    base.Sigla = bb.Sigla;
                    break;
                case "033":
                    EspecieDocumento_Santander st = new EspecieDocumento_Santander(codigo);
                    base.Banco = st.Banco;
                    base.Codigo = st.Codigo;
                    base.Especie = st.Especie;
                    base.Sigla = st.Sigla;
                    break;
                case "041":
                    EspecieDocumento_Banrisul ba = new EspecieDocumento_Banrisul(codigo);
                    base.Banco = ba.Banco;
                    base.Codigo = ba.Codigo;
                    base.Especie = ba.Especie;
                    base.Sigla = ba.Sigla;
                    break;
                case "104":
                    EspecieDocumento_Caixa cx = new EspecieDocumento_Caixa(codigo);
                    base.Banco = cx.Banco;
                    base.Codigo = cx.Codigo;
                    base.Especie = cx.Especie;
                    base.Sigla = cx.Sigla;
                    break;
                case "237":
                    EspecieDocumento_Bradesco br = new EspecieDocumento_Bradesco(codigo);
                    base.Banco = br.Banco;
                    base.Codigo = br.Codigo;
                    base.Especie = br.Especie;
                    base.Sigla = br.Sigla;
                    break;
                case "341":
                    EspecieDocumento_Itau it = new EspecieDocumento_Itau(codigo);
                    base.Banco = it.Banco;
                    base.Codigo = it.Codigo;
                    base.Especie = it.Especie;
                    base.Sigla = it.Sigla;
                    break;
                case "347":
                    EspecieDocumento_Sudameris sd = new EspecieDocumento_Sudameris(codigo);
                    base.Banco = sd.Banco;
                    base.Codigo = sd.Codigo;
                    base.Especie = sd.Especie;
                    base.Sigla = sd.Sigla;
                    break;
            }
        }
    }
}
