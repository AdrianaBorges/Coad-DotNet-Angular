using AutoMapper;
using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Config.CustomConvert
{
    class PrePedidoToClientConvert : ITypeConverter<PrePedidoDTO, CLIENTES>
    {
        public CLIENTES Convert(ResolutionContext context)
        {
            PrePedidoDTO prepedidoDTO = (PrePedidoDTO) context.SourceValue;
            //CLIENTES _cli = new CLIENTES()
            //{
            //    CLI_NOME = prepedidoDTO.nome,
            //    CLI_A_C = prepedidoDTO.aoscuidados,
            //    CLI_TP_PESSOA = prepedidoDTO.tipodecliente,
            //    CLI_INSCRICAO = prepedidoDTO.inscestadual,
            //    CLI_CPF_CNPJ = prepedidoDTO.cpfcnpj,

            //};



            IList<CLIENTES_ENDERECO> lst_end = new List<CLIENTES_ENDERECO>();

            //CLIENTES_ENDERECO end_entrega = new CLIENTES_ENDERECO()
            //{
            //    END_TIPO = 1,
            //    END_LOGRADOURO = prepedidoDTO.enderecoentrega,
            //    END_NUMERO = prepedidoDTO.numeroentrega,
            //    END_COMPLEMENTO = prepedidoDTO.complementoentrega,
            //    END_CEP = prepedidoDTO.cepentrega,
            //    END_BAIRRO = prepedidoDTO.bairroentrega,
            //    END_MUNICIPIO = prepedidoDTO.municipioentrega,
                
            //    // TODO: colocar combobox para a uf

            //};

            //CLIENTES_ENDERECO end_faturamento = new CLIENTES_ENDERECO()
            //{
            //    END_TIPO = 2,
            //    END_LOGRADOURO = prepedidoDTO.enderecofaturamento,
            //    END_NUMERO = prepedidoDTO.numerofaturamento,
            //    END_COMPLEMENTO = prepedidoDTO.complementofaturamento,
            //    END_CEP = prepedidoDTO.cepfaturamento,
            //    END_BAIRRO = prepedidoDTO.bairroentrega,
            //    END_MUNICIPIO = prepedidoDTO.municipiofaturamento,
            //    // TODO: colocar combobox para a uf

            //};

            //lst_end.Add(end_entrega);
            //lst_end.Add(end_faturamento);
            //_cli.CLIENTES_ENDERECO = lst_end;

            return null; //_cli;            
        }
    }
}
