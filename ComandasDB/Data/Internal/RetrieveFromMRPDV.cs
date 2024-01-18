using System.Collections.Generic;
using System.Linq;

namespace ComandasDB.Data.Internal
{
    /// <summary>
    /// Faz a cópia automática de comandas existentes no PDV para a base
    /// separada de comandas ao invocar a classe de contexto.
    /// </summary>
    internal class RetrieveFromMRPDV
    {
        internal static List<PreVenda> GetPreVendasFromMRPDV()
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

        internal static List<ItensPreVenda> GetItensPreVendasFromMRPDV()
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
