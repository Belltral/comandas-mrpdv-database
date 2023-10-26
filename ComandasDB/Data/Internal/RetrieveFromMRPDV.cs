using System.Linq;

namespace ComandasDB.Data.Internal
{
    internal class RetrieveFromMRPDV
    {
        public void RetrievePreVendas()
        {
            using (var db = new DataMRPDVContext())
            {
                var query = from preVendas in db.PreVendas
                            select preVendas;

            }
        }
    }
}
