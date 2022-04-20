using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.DAO;

namespace COAD.CORPORATIVO.Model.DTO
{
    public class PrePedidoServicos
    {
        //    private CLIENTES CriarNovoCliente(PrePedidoDTO prepedidoDTO)
        //    {
        //        //CLIENTES _cli = new CLIENTES(){
        //        //    CLI_NOME = prepedidoDTO.nome,
        //        //    CLI_A_C = prepedidoDTO.aoscuidados,
        //        //    CLI_TP_PESSOA = prepedidoDTO.tipodecliente, 
        //        //    CLI_INSCRICAO = prepedidoDTO.inscestadual,
        //        //    CLI_CPF_CNPJ = prepedidoDTO.cpfcnpj,                

        //        //};



        //        IList<CLIENTES_ENDERECO> lst_end = new List<CLIENTES_ENDERECO>();

        //        //CLIENTES_ENDERECO end_entrega = new CLIENTES_ENDERECO()
        //        //{
        //        //    END_TIPO = 1,
        //        //    END_LOGRADOURO = prepedidoDTO.enderecoentrega,
        //        //    END_NUMERO = prepedidoDTO.numeroentrega,
        //        //    END_COMPLEMENTO = prepedidoDTO.complementoentrega,
        //        //    END_CEP = prepedidoDTO.cepentrega,
        //        //    END_BAIRRO = prepedidoDTO.bairroentrega,
        //        //    END_MUNICIPIO = prepedidoDTO.municipioentrega,
        //        //    // TODO: colocar combobox para a uf

        //        //};

        //        //CLIENTES_ENDERECO end_faturamento = new CLIENTES_ENDERECO()
        //        //{
        //        //    END_TIPO = 2,
        //        //    END_LOGRADOURO = prepedidoDTO.enderecofaturamento,
        //        //    END_NUMERO = prepedidoDTO.numerofaturamento,
        //        //    END_COMPLEMENTO = prepedidoDTO.complementofaturamento,
        //        //    END_CEP = prepedidoDTO.cepfaturamento,
        //        //    END_BAIRRO = prepedidoDTO.bairroentrega,
        //        //    END_MUNICIPIO = prepedidoDTO.municipiofaturamento,
        //        //    // TODO: colocar combobox para a uf

        //        //};

        //        //lst_end.Add(end_entrega);
        //        //lst_end.Add(end_faturamento);
        //        //_cli.CLIENTES_ENDERECO = lst_end;

        //        return null;
        //    }

        //    //public PRE_PEDIDO PrencherTelefoneEEmail(PrePedidoDTO prepedidoDTO)
        //    //{
        //    //    //var lstTelefones = null; //prepedidoDTO.lstDeTelefones;

        //    //    //if (lstTelefones != null && lstTelefones.Count > 0)
        //    //    //{
        //    //    //    IList<PRE_PEDIDO_TELEFONE> lstTels = new List<PRE_PEDIDO_TELEFONE>();
        //    //    //    foreach (var telefoneDto in lstTelefones)
        //    //    //    {
        //    //    //        PRE_PEDIDO_TELEFONE tel = new PRE_PEDIDO_TELEFONE();
        //    //    //    }
        //    //    //}

        //    //    //return null;
        //    //}

        //    public PRE_PEDIDO PreencherPrePedido(PrePedidoDTO prepedidoDTO)
        //    {
        //        PRE_PEDIDO prepedido = new PRE_PEDIDO();

        //        prepedido.PRE_DATA = DateTime.Now;

        //        if (true)//prepedidoDTO.cliente == "1")
        //        {
        //            CLIENTES cli = CriarNovoCliente(prepedidoDTO);
        //            new ClienteDAO().Incluir(cli);
        //            prepedido.CLI_ID = cli.CLI_ID;
        //        }
        //        else
        //        {
        //            prepedido.CLI_ID = prepedidoDTO.lstDadosPedido.ToList()[0].codigocliente;
        //        }

        //        prepedido.EMP_ID = (int) prepedidoDTO.empresa;
        //        prepedido.PRE_ANO = prepedidoDTO.anobase;
        //        prepedido.PRE_PERIODO = prepedidoDTO.periodo;
        //        prepedido.PER_SEMANA = prepedidoDTO.semana;
        //        prepedido.BRI_ID = prepedidoDTO.brindeid;
        //        prepedido.USU_LOGIN = COAD.SEGURANCA.Repositorios.Base.SessionContext.autenticado.USU_NOME;
        //        prepedido.REP_ID = COAD.SEGURANCA.Repositorios.Base.SessionContext.autenticado.REP_ID;
        //        prepedido.PRE_ENTREGA_PASTA = prepedidoDTO.pastaprod;
        //        prepedido.PRE_ENTREGA_LIVRO = prepedidoDTO.livroprod;
        //        prepedido.PRE_ENTREGA_BRINDE = prepedidoDTO.cdromprod;
        //        prepedido.PRE_STATUS_ID = 2;

        //        PEDIDO pedido = new PEDIDO();
        //        foreach(PedidoDTO pedidoDTO in prepedidoDTO.lstDadosPedido)
        //        {
        //            pedido.EMP_ID = (int) prepedidoDTO.empresa;

        //            pedido.PED_DATA_PEDIDO = DateTime.Now;
        //            pedido.PED_VLR_PEDIDO = pedidoDTO.valor;
        //            pedido.PED_STATUS = 2;
        //            //pedido.PED_OBS = prepedidoDTO.
        //            pedido.TIPO_PED_ID = pedidoDTO.tiponegocioprod;
        //            pedido.PED_PERC_DESCONTO = pedidoDTO.desconto;
        //            pedido.PED_VLR_TOTAL_PEDIDO = pedidoDTO.total;
        //            pedido.USU_LOGIN = prepedidoDTO.operadora;
        //            pedido.PED_VIGENCIA_INI = pedidoDTO.inicioVigenciaprod;
        //            pedido.PED_VIGENCIA_FIM = pedidoDTO.fimVigenciaprod;
        //            pedido.CMP_ID = pedidoDTO.produtoProd;

        //            prepedido.PEDIDO.Add(pedido);
        //        }

        //        PEDIDO_PAGAMENTO pedidoPagamento = new PEDIDO_PAGAMENTO();
        //        foreach (FormaDePagamentoDTO formaPagamentoDTO in prepedidoDTO.lstFormaPagamento)
        //        {
        //            pedidoPagamento.EMP_ID = (int) prepedidoDTO.empresa;
        //            pedidoPagamento.TPG_ID = (int) formaPagamentoDTO.formapagtoprod;
        //            pedidoPagamento.PGT_VLR_TOTAL = formaPagamentoDTO.total;
        //            pedidoPagamento.PGT_QTDE_PARCELAS = formaPagamentoDTO.qteparcelas;
        //            pedidoPagamento.PGT_VLR_ENTRADA = formaPagamentoDTO.entrada;
        //            pedidoPagamento.PGT_VLR_PARCELA = formaPagamentoDTO.valorparcelas;
        //            pedidoPagamento.PGT_SEGUNDO_VENCTO = formaPagamentoDTO.vencsegparcela;
        //            pedidoPagamento.PGT_TIPO_DOCUMENTO = formaPagamentoDTO.tipodocumento;
        //            pedidoPagamento.PGT_NUMERO_DOCUMENTO = formaPagamentoDTO.numdocumento;
        //            pedidoPagamento.PGT_BANCO = formaPagamentoDTO.bancoprod;
        //            pedidoPagamento.PGT_CHEQUE_BOM_PARA = formaPagamentoDTO.chequebompara;
        //            pedidoPagamento.PGT_OBS = formaPagamentoDTO.observacaoprod;
        //            prepedido.PEDIDO_PAGAMENTO.Add(pedidoPagamento);
        //        }

        //        return prepedido;
        //    }

        //}
    }
}