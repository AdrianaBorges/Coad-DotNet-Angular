using COAD.COBRANCA.Exceptions;
using COAD.COBRANCA.Bancos.Model.DTO;
using COAD.COBRANCA.Bancos.Model.DTO.Interfaces;
using System;
using System.Collections.Generic;

namespace COAD.COBRANCA.Boleto.Service
{
    public static class ConfigBoletoSRV
    {
        public static IDictionary<ChaveConfigBoletoDTO, Type> Regras { get; set; } = new Dictionary<ChaveConfigBoletoDTO, Type>();

        public static void RegistrarRegra<TRegrasBoleto>(IBanco banco, string codigoCarteira) where TRegrasBoleto : IRegrasBoleto
        {
            try
            {
                if (banco == null)
                    throw new ConfigException("O banco não foi informado.");


                if (string.IsNullOrWhiteSpace(codigoCarteira))
                    throw new ConfigException("O código da carteira não foi informado.");

                var type = typeof(TRegrasBoleto);
                if (type == null)
                    throw new ConfigException("A regra não foi informada.");

                var key = new ChaveConfigBoletoDTO()
                {
                    Banco = banco,
                    CodigoCarteira = codigoCarteira
                };
                if (!Regras.Keys.Contains(key))
                {
                    Regras.Add(key, type);
                }
            }
            catch (Exception e)
            {
                throw new ConfigException("Não é possível configurar as regras.", e);
            }

        }

        public static IRegrasBoleto RetornarRegra(IBanco banco, string codigoCarteira)
        {

            if (banco == null)
                throw new ConfigException("O banco não foi informado.");

            if (string.IsNullOrWhiteSpace(codigoCarteira))
                throw new ConfigException("O código da carteira não foi informado.");

            ChaveConfigBoletoDTO chave = new ChaveConfigBoletoDTO()
            {
                Banco = banco,
                CodigoCarteira = codigoCarteira
            };

            if (Regras.Keys.Contains(chave))
            {
                var typeRegra = Regras[chave];
                var obj = Activator.CreateInstance(typeRegra);

                return obj as IRegrasBoleto;
            }

            return null;
        }
    }
}
