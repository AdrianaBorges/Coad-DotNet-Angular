using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.UTIL.Grafico;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PublicacaoUfSRV : GenericService<PUBLICACAO_UF, PublicacaoUfDTO, object>
    {
        private PublicacaoUfDAO _dao = new PublicacaoUfDAO();

        private AreasSRV _serviceAreas = new AreasSRV();

        public PublicacaoUfSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoUfDTO> PublicacaoUf(int? publicacaoId = null, int? areaId = null, string ufId = null, string ano = null, int? numero = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.PublicacaoUf(publicacaoId, areaId, ufId, ano, numero);
            return resp;
        }

        public JsonGraficoResponse PublicacaoUfGrafico(int? publicacaoId, int? areaId, string ufId = null, string ano = null, int? numero = null, int pagina = 1, int itensPorPagina = 10)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();
            try
            {
                _resultado.Titulo = "Matérias Cadastradas";
                _resultado.Descricao = "Informativo selecionado: " + numero.ToString() + "/" +ano.ToString();

                var resp = _dao.PublicacaoUf(publicacaoId, areaId, ufId, ano, numero).lista;
                
                string area = null;
                int q = 0;
                int t = 0;

                foreach (var dg in resp)
                {
                    JsonGrafico g = new JsonGrafico();

                    AreasDTO objArea = _serviceAreas.FindById(dg.ARE_CONS_ID);

                    if ( ( (area != null) && (area != objArea.ARE_CONS_DESCRICAO) ) || (resp.Count() == t+1) )
                    {
                        g.label = area;
                        g.data = q;
                        _resultado.Dados.Add(g);
                        q = 1;
                    }

                    area = objArea.ARE_CONS_DESCRICAO;
                    q++;
                    t++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }

            return _resultado;
        }

        public void SalvarPublicacaoUf(ICollection<PublicacaoUfDTO> publicacaoUf)
        {
            try
            {
                if (publicacaoUf.Count > 0)
                {
                    // eliminando anteriores...
                    var eliminar = this.PublicacaoUf(publicacaoUf.First().PUB_ID, publicacaoUf.First().ARE_CONS_ID).lista;
                    DeleteAll(eliminar, "PUB_ID", "ARE_CONS_ID", "UF_ID");

                    // salvando atuais...
                    foreach (var s in publicacaoUf)
                    {
                        if (s.PUB_ID != null && s.ARE_CONS_ID != null && s.UF_ID != null && s.INF_ANO != null && s.INF_NUMERO != null)
                        {
                            Save(s);
                        }
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var _erro = new FormattedDbEntityValidationException(dbEx);

                SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                throw _erro;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
    }
}
