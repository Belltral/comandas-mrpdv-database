using System.Collections.Generic;
using System.Linq;

namespace ComandasDB.Data.Internal
{
    internal class RetrieveFromMRPDV
    {
        internal static List<PreVendas> GetPreVendasFromMRPDV()
        {
            List<PreVendas> preVendasList = new List<PreVendas>();

            using (var db = new DataMRPDVContext())
            {
                var query = db.PreVendas.Select(s => s);

                foreach (var item in query)
                {
                    preVendasList.Add(item);
                }
            }

            return preVendasList;
        }

        internal static List<ItensPreVendas> GetItensPreVendasFromMRPDV()
        {
            List<ItensPreVendas> itensPreVendasList = new List<ItensPreVendas>();

            using (var db = new DataMRPDVContext())
            {
                var query = db.ItensPreVendas.Select(s => s);

                foreach (var item in query)
                {
                    itensPreVendasList.Add(item);
                }
            }

            return itensPreVendasList;
        }

        internal static void RetriveFromMRToComandas()
        {
            var preVendasList = GetPreVendasFromMRPDV();
            var itensPreVendasList = GetItensPreVendasFromMRPDV();

            if (preVendasList.Count() > 0 && itensPreVendasList.Count() > 0)
            {
                foreach (var preVenda in preVendasList)
                {
                    Comandas.InsertPreVenda(preVenda);
                }

                Comandas.InsertItensOfPreVenda(itensPreVendasList);
            }
        }
    }
}
