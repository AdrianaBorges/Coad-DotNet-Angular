using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.SEGURANCA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.DTO;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class EmpresaDTO : EmpresaModel, IEmpresa
    {
        public EmpresaDTO()
        {

        }

        public EmpresaDTO(EmpresaModel empresa)
        {
            if(empresa != null)
            {
                this.EMP_ID = empresa.EMP_ID;
                this.EMP_RAZAO_SOCIAL = empresa.EMP_RAZAO_SOCIAL;
                this.EMP_NOME_FANTASIA = empresa.EMP_NOME_FANTASIA;
                this.EMP_CNPJ = empresa.EMP_CNPJ;
                this.EMP_IE = empresa.EMP_IE;
                this.EMP_IM = empresa.EMP_IM;
                this.EMP_SUFRAMA = empresa.EMP_SUFRAMA;
                this.EMP_LOGRADOURO = empresa.EMP_LOGRADOURO;
                this.EMP_NUMERO = empresa.EMP_NUMERO;
                this.EMP_COMPLEMENTO = empresa.EMP_COMPLEMENTO;
                this.EMP_BAIRRO = empresa.EMP_BAIRRO;
                this.EMP_CEP = empresa.EMP_CEP;
                this.EMP_TEL1 = empresa.EMP_TEL1;
                this.EMP_TEL2 = empresa.EMP_TEL2;
                this.EMP_TEL3 = empresa.EMP_TEL3;
                this.EMP_EMAIL = empresa.EMP_EMAIL;
                this.EMP_SITE = empresa.EMP_SITE;
                this.EMP_ULTIMA_NFE = empresa.EMP_ULTIMA_NFE;
                this.EMP_CNR_AGCEDENTE = empresa.EMP_CNR_AGCEDENTE;
                this.EMP_AREA = empresa.EMP_AREA;
                this.EMP_TIPO = empresa.EMP_TIPO;
                this.EMP_IE_ST = empresa.EMP_IE_ST;
                this.CNT_ID = empresa.CNT_ID;
                this.IBGE_COD_COMPLETO = empresa.IBGE_COD_COMPLETO;
                this.CID_ID = empresa.CID_ID;
                this.EMP_APARECE_PRE_PEDIDO = empresa.EMP_APARECE_PRE_PEDIDO;
                this.EMP_GRP_COAD = empresa.EMP_GRP_COAD;
                this.EMP_DESP_ADM_BOLETO = empresa.EMP_DESP_ADM_BOLETO;
                this.EMP_MORA_MES_BOLETO = empresa.EMP_MORA_MES_BOLETO;
                this.EMP_ULT_FECHAMENTO_FAT = empresa.EMP_ULT_FECHAMENTO_FAT;
                this.UF = empresa.UF;
                this.COD_UF = empresa.COD_UF;
                this.MUN_DESCRICAO = empresa.MUN_DESCRICAO;
                this.UFDTO = new COAD.FISCAL.Model.DTO.UFDTO()
                {
                    CodigoUF = this.COD_UF,
                    Nome = this.UF
                };
            }
        }

        public EmpresaDTO(FornecedorDTO fornecedor)
        {
            if (fornecedor != null)
            {
                this.EMP_ID = fornecedor.FOR_ID;
                this.EMP_RAZAO_SOCIAL = fornecedor.FOR_RAZAO_SOCIAL;
                this.EMP_NOME_FANTASIA = fornecedor.FOR_NOME_FANTASIA;
                this.EMP_CNPJ = fornecedor.FOR_CNPJ;
                this.EMP_IE = fornecedor.FOR_INSCRICAO;
                this.EMP_IM = fornecedor.FOR_INSCMUNIP;
                this.EMP_SUFRAMA = fornecedor.FOR_INSCSUFRAMA;
                this.EMP_LOGRADOURO = fornecedor.FOR_ENDERECO;
                this.EMP_NUMERO = fornecedor.FOR_END_NUMERO;
                this.EMP_COMPLEMENTO = fornecedor.FOR_END_COMPLEMENTO;
                this.EMP_BAIRRO = fornecedor.FOR_BAIRRO;
                this.EMP_CEP = fornecedor.FOR_CEP;
                this.EMP_TEL1 = fornecedor.FOR_TEL;
                this.EMP_EMAIL = fornecedor.FOR_EMAIL;
                this.IBGE_COD_COMPLETO = fornecedor.IBGE_COD_COMPLETO;
                this.UF = fornecedor.UF;
                this.COD_UF = fornecedor.COD_UF;

                if(fornecedor.MUNICIPIO != null)
                {
                    this.MUN_DESCRICAO = fornecedor.MUNICIPIO.MUN_DESCRICAO;
                }

                this.UFDTO = new COAD.FISCAL.Model.DTO.UFDTO()
                {
                    CodigoUF = this.COD_UF,
                    Nome = this.UF
                };
            }
        }

        public int? EmpresaID { get => EMP_ID; set => EMP_ID = (value != null) ? value.Value : 0; }
        public string RazaoSocial { get => EMP_RAZAO_SOCIAL; set => EMP_RAZAO_SOCIAL = value; }
        public string NomeFantasia { get => EMP_NOME_FANTASIA; set => EMP_NOME_FANTASIA = value; }
        public string CNPJ { get => EMP_CNPJ; set => EMP_CNPJ = value; }
        public string IE { get => EMP_IE; set => EMP_IE = value; }
        public string Logradouro { get => EMP_LOGRADOURO; set => EMP_LOGRADOURO = value; }
        public string Numero { get => EMP_NUMERO; set => EMP_NUMERO = value; }
        public string Complemento { get => EMP_COMPLEMENTO; set => EMP_COMPLEMENTO = value; }
        public string Bairro { get => EMP_BAIRRO; set => EMP_BAIRRO = value; }
        public string CEP { get => EMP_CEP; set => EMP_CEP = value; }
        public string Telefone { get => EMP_TEL1; set => EMP_TEL1 = value; }
        public string Email { get => EMP_EMAIL; set => EMP_EMAIL = value; }
        public int? SeqNFe { get => EMP_ULTIMA_NFE; set => EMP_ULTIMA_NFE = value; }
        public int? SeqNFSe { get => EMP_ULTIMA_NFSE; set => EMP_ULTIMA_NFSE = value; }
        public string CodIBGE { get => IBGE_COD_COMPLETO; set => IBGE_COD_COMPLETO = value; }
        public string Municipio { get => MUN_DESCRICAO; set => MUN_DESCRICAO = value; }
        public string CodPais { get; set; }
        public FISCAL.Model.DTO.UFDTO UFDTO { get; set; }
        public string IM { get => EMP_IM; set => EMP_IM = value; }
    }
}
