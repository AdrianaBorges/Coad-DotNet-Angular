using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.PROXY.Model.DTO;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.PROXY.Service
{
    public class CursoProxySRV : ProdutoComposicaoSRV
    {
        [ServiceProperty("CMP_ID", "areaConCursoProxy", PropertyName = "AREA_CONSULTORIA_CURSO_PROXY")]
        private AreaConsultoriaCursoProxySRV _areaConsultoriaCursoProxy = new AreaConsultoriaCursoProxySRV();

        public CursoProxyDTO FindByIdFullLoad(int CMP_ID)
        {
            var produtoComposto = FindByIdFullLoad(CMP_ID, true);
            var curso = GetProxyTools<CursoProxyDTO>().ConvertToProxy(produtoComposto);

            curso.AREA_CONSULTORIA_CURSO = produtoComposto.AREA_CONSULTORIA_CURSO;
            produtoComposto.AREA_CONSULTORIA_CURSO = null;
            curso.PRODUTO_COMPOSICAO_ITEM = produtoComposto.PRODUTO_COMPOSICAO_ITEM;

            //var curso = ConvertWithProfile<ProdutoComposicaoDTO, CursoProxyDTO>(produtoComposto, "proxy");

            _areaConsultoriaCursoProxy.PreencherAreaConsultoriaCurso(curso);

            return curso;
        }

        public void VerificarEAdicionarDadosFixos(CursoProxyDTO curso)
        {
            if (curso != null)
            {
                //if (curso.PRO_ID == null || curso.PRO_ID != 40)
                //{
                //    curso.PRO_ID = 40;
                //}

                if (curso.TIPO_PRO_ID == null || curso.TIPO_PRO_ID != 1)
                {
                    curso.TIPO_PRO_ID = 1;
                }

                if (curso.AREA_ID == null)
                {
                    curso.AREA_ID = 4;
                }
            }
        }

      
        public ProdutoComposicaoDTO SalvarCurso(CursoProxyDTO curso)
        {
            ProdutoComposicaoDTO composicaoSalvaRetorno = null;

            if (curso != null)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    VerificarEAdicionarDadosFixos(curso);

                    var cursoSalvo = SaveOrUpdate(curso);
                    cursoSalvo.PRODUTO_COMPOSICAO_ITEM = curso.PRODUTO_COMPOSICAO_ITEM;

                    var service = new ProdutoComposicaoItemSRV();

                    service.AdicionarProdutoCurso(cursoSalvo);
                    service.DeletarComposicaoItens(cursoSalvo);
                    service.SalvarItensComposicao(cursoSalvo, cursoSalvo.PRODUTO_COMPOSICAO_ITEM, true);

                    if (curso.CMP_ID == null)
                    {
                        composicaoSalvaRetorno = cursoSalvo;
                        curso.CMP_ID = cursoSalvo.CMP_ID;
                    }

                    _areaConsultoriaCursoProxy.SalvarEExcluirAreaConsultoriaRepresentante(curso);

                    scope.Complete();
                }
            }

            return composicaoSalvaRetorno;
        }

    }
}
