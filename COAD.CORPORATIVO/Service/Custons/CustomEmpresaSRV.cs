using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using GenericCrud.Service;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Custons
{
    public class CustomEmpresaSRV 
    {
        public EmpresaSRV empresaSRV { get; set; }
        public EmpresaLegadoSRV empresaLegadoSRV { get; set; }

        /// <summary>
        /// Retorna o próximo número da nota fiscal para a empresa informada
        /// e promove a atualização no banco com esse número.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public int? RetornarSequencialNFEEmpresa(int? empId)
        {
            EmpresaModel empresa = empresaSRV.FindById(empId);
            EmpresaLegadoDTO empresaLegado = empresaLegadoSRV.FindById(empId);

            if(empresa != null)
            {
                int? seqNfe = empresa.EMP_ULTIMA_NFE;
                int? seqNfeLegado = 0;

                if (empresaLegado != null)
                    seqNfeLegado = empresaLegado.ultima_nfe;

                int? seqCorreta = (seqNfe >= seqNfeLegado) ? seqNfe : seqNfeLegado;
                seqCorreta++;

                empresa.EMP_ULTIMA_NFE = seqCorreta;

                empresaSRV.Merge(empresa);
                
                if(empresaLegado != null)
                {
                    empresaLegado.ultima_nfe = seqCorreta;
                    empresaLegadoSRV.Merge(empresaLegado);
                }

                return seqCorreta;                
            }

            throw new Exception("Não é possível achar a empresa informado para retornar a seguência de NFE");
        }

        /// <summary>
        /// Retorna a provável sequência do número da nota (pois pode ser que outro processo altere).
        /// Esse método não faz nenhuma alteração no banco.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public int? RetornarProvavelSequencialNFEEmpresa(int? empId)
        {

            var sequencia = RetornarUltimoNumNotaNFEEmpresa(empId);
            ++sequencia;
            return sequencia;
        }

        /// <summary>
        /// Retorna o último número gerado de uma nota.
        /// Esse método não faz nenhuma alteração no banco.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public int? RetornarUltimoNumNotaNFEEmpresa(int? empId)
        {
            EmpresaModel empresa = empresaSRV.FindById(empId);
            EmpresaLegadoDTO empresaLegado = empresaLegadoSRV.FindById(empId);

            if (empresa != null)
            {
                int? seqNfe = empresa.EMP_ULTIMA_NFE;
                int? seqNfeLegado = 0;

                if (empresaLegado != null)
                    seqNfeLegado = empresaLegado.ultima_nfe;

                int? seqCorreta = (seqNfe >= seqNfeLegado) ? seqNfe : seqNfeLegado;

                return seqCorreta;
            }

            throw new Exception("Não é possível achar a empresa informado para retornar a seguência de NFE");
        }

        /// <summary>
        /// Retorna o próximo número da nota fiscal para a empresa informada
        /// e promove a atualização no banco com esse número.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public int? AlterarNumeroNota(int? empId, int? numero)
        {
            EmpresaModel empresa = empresaSRV.FindById(empId);
            EmpresaLegadoDTO empresaLegado = empresaLegadoSRV.FindById(empId);

            if (empresa != null)
            {
                int? seqCorreta = numero;

                empresa.EMP_ULTIMA_NFE = seqCorreta;
                empresaSRV.Merge(empresa);

                if (empresaLegado != null)
                {
                    empresaLegado.ultima_nfe = seqCorreta;
                    empresaLegadoSRV.Merge(empresaLegado);
                }

                return seqCorreta;
            }

            throw new Exception("Não é possível achar a empresa informado para retornar a seguência de NFE");
        }

        public EmpresaDTO RetornarEmpresa(int? empID)
        {
            var empresa = empresaSRV.FindById(empID);
            if(empresa != null)
            {
                var municipio = ServiceFactory.RetornarServico<MunicipioSRV>().BuscarPorIBGE(empresa.IBGE_COD_COMPLETO);
                if (municipio != null)
                {
                    empresa.UF = municipio.UF;
                    empresa.MUN_DESCRICAO = StringUtil.LimparAcentuacao(municipio.MUN_DESCRICAO);
                    var ufDTO = ServiceFactory.RetornarServico<UFSRV>().FindById(empresa.UF);

                    if(ufDTO != null)
                    {
                        int codUF = 0;
                        if (!string.IsNullOrWhiteSpace(ufDTO.UF_COD))
                        {
                            int.TryParse(ufDTO.UF_COD, out codUF);
                        }
                        empresa.COD_UF = codUF;
                    }
                }
                
                var empresaRetorno = new EmpresaDTO(empresa);
                return empresaRetorno;
            }
            return null;
        }

        /// <summary>
        /// Retorna o próximo número da nota fiscal de Serviço para a empresa informada
        /// e promove a atualização no banco com esse número.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public int? RetornarSequencialNFSEEmpresa(int? empId)
        {
            EmpresaModel empresa = empresaSRV.FindById(empId);

            if (empresa != null)
            {
                int? seqNfse = empresa.EMP_ULTIMA_NFSE;
                seqNfse++;
                empresa.EMP_ULTIMA_NFSE = seqNfse;
                empresaSRV.Merge(empresa);
                return seqNfse;
            }

            throw new Exception("Não é possível achar a empresa informado para retornar a seguência de NFE");
        }
    }
}
