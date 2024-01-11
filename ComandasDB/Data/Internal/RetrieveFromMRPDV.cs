using System.Collections.Generic;
using System.Linq;

namespace ComandasDB.Data.Internal
{
    internal class RetrieveFromMRPDV
    {
        internal static List<PreVendas> GetPreVendasFromMRPDV()
        {
            List<PreVendas> preVendas = new List<PreVendas>();

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

        internal static List<ItensPreVendas> GetItensPreVendasFromMRPDV()
        {
            List<ItensPreVendas> itensPreVendas = new List<ItensPreVendas>();

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

        internal static void RetriveFromMRToComandas()
        {
            var preVendas = GetPreVendasFromMRPDV();
            var itensPreVendas = GetItensPreVendasFromMRPDV();

            if (preVendas.Count() > 0 && itensPreVendas.Count() > 0)
            {
                foreach (var preVenda in preVendas)
                {
                    Comandas.InsertPreVenda(preVenda);
                }

                Comandas.InsertItensOfPreVenda(itensPreVendas);
            }
        }
    }
}
