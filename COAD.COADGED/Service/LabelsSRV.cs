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

    public class LabelsSRV : GenericService<LABELS, LabelsDTO, int>
    {
        private LabelsDAO _dao = new LabelsDAO();

        public LabelsSRV()
        {
            Dao = _dao;
        }

        public Pagina<LabelsDTO> Labels(int? labelId, string label = null, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Labels(labelId, label, descricao, ativoId, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarLabel(LabelsDTO label)
        {
            try
            {
                if (label.LBL_ID == null && _dao.Labels(null, label.LBL_NOME).lista.Count() > 0)
                {
                    throw new Exception("Já existe uma label cadastrada com este nome!");
                }
                else
                {
                    label.LBL_ATIVO = label.LBL_ATIVO == null ? 1 : label.LBL_ATIVO;

                    if (label.LBL_ID != null)
                    {
                        Merge(label, "LBL_ID");
                    }
                    else
                    {
                        Save(label);
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
