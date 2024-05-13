using ComandasDB.Context;
using System.Collections.Generic;
using System.Linq;

namespace ComandasDB.Data.Internal
{
    /// <summary>
    /// Lida com a troca de informações internas de controle entre o PDV e a base de comandas.
    /// </summary>
    internal class RetrieveFromMRPDV
    {
        private static List<PreVenda> GetPreVendasFromMRPDV()
        {
            List<PreVenda> preVendas = new List<PreVenda>();

            using (var db = new DataMRPDVContext())
            {
                var query = db.PreVendas.Select(s => s);

                foreach (var item in query)
                {
                    preVendas.Add(item);
                }
            }

            return preVendas;
        }

        private static List<ItensPreVenda> GetItensPreVendasFromMRPDV()
        {
            List<ItensPreVenda> itensPreVendas = new List<ItensPreVenda>();

            using (var db = new DataMRPDVContext())
            {
                var query = db.ItensPreVendas.Select(s => s);

                foreach (var item in query)
                {
                    itensPreVendas.Add(item);
                }
            }

            return itensPreVendas;
        }

        // TODO: validação de números de comandas repetidos quando é implementado o servidor
        // em PDVs que não tinham o servidor padrão anteriormente
        internal static void RetriveFromMRToComandas()
        {
            var preVendas = GetPreVendasFromMRPDV();
            var itensPreVendas = GetItensPreVendasFromMRPDV();

            if (preVendas.Any() && itensPreVendas.Any())
            {
                using (var db = new ComandasDbContext())
                {
                    foreach (var preVenda in preVendas)
                    {
                        int comandaNumber = preVenda.COMANDA_PRVD;

                        int oldPreVendaNumber = preVenda.NUMERO_PRVD;

                        var itensCurrentPreVenda = itensPreVendas.Select(i => i).Where(n => n.NUMERO_PRVD == oldPreVendaNumber);

                        db.PreVendas.Add(preVenda);
                        db.SaveChanges();

                        var newPreVendaNumber = db.PreVendas.SingleOrDefault(pv => pv.COMANDA_PRVD == comandaNumber).NUMERO_PRVD;

                        List<ItensPreVenda> updatedItens = new List<ItensPreVenda>();

                        foreach (var item in itensCurrentPreVenda)
                        {
                            item.NUMERO_PRVD = newPreVendaNumber;
                            updatedItens.Add(item);
                        }

                        db.ItensPreVendas.AddRange(updatedItens);
                        db.SaveChanges();
                    }
                }
            }
        }

        internal static Parametros GetServerInfo()
        {
            using (var db = new DataMRPDVContext())
            {
                var query = db.Parametros.SingleOrDefault(p => p.ID_TPPR == 228);

                return query;
            }
        }

        internal static Pdvs PdvInfo()
        {
            using (var db = new DataMRPDVContext())
            {
                var query = db.Pdvs.FirstOrDefault();

                return query;
            }
        }
    }
}