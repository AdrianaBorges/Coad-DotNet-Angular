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

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class VeiculoSRV : GenericService<VEICULO, VeiculoDTO, int>
    {
        private VeiculoDAO _dao = new VeiculoDAO();

        public VeiculoSRV()
        {
            Dao = _dao;
        }

        public Pagina<VeiculoDTO> Veiculos(int? veiculoId, string descricao = null, int? periodoId = null, int? ativoId = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Veiculos(veiculoId, descricao, periodoId, ativoId, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarVeiculo(VeiculoDTO veiculo)
        {
            try
            {
                if (veiculo.TVI_ID == null && _dao.Veiculos(null, veiculo.TVI_DESCRICAO).lista.Count() > 0)
                {
                    throw new Exception("Já existe um veículo cadastrado com este nome!");
                }
                else
                {
                    veiculo.TVI_ATIVO = veiculo.TVI_ATIVO == null ? 1 : veiculo.TVI_ATIVO;

                    if (veiculo.TVI_ID != null)
                    {
                        veiculo.DATA_ALTERA = DateTime.Now;
                        Merge(veiculo, "TVI_ID");
                    }
                    else
                    {
                        //veiculo.DATA_CADASTRO = DateTime.Now; // campo não foi criado na estrutura de dados...
                        Save(veiculo);
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
