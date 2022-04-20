using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace COAD.CORPORATIVO.Service
{
    public class ContaRefSRV : GenericService<CONTA_REF, ContaRefDTO, int>
    {
        public ContaRefDAO _dao = new ContaRefDAO();

        public ContaRefSRV()
        {
            this.Dao = _dao;
        }

        public Pagina<ContaRefDTO> Conta(int? empresa = null, string banco = null, string agencia = null, string conta = null, string tipo = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Conta(empresa, banco, agencia, conta, tipo, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarContaRef(ContaRefDTO conta, bool incluir)
        {
            try
            {
                if (!incluir)
                {
                    Merge(conta, "CTA_ID");
                }
                else
                {
                    Save(conta);
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
