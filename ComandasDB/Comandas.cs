using DatabaseScaffold;
using System;
using System.Linq;


namespace ComandasDB
{
    public class Comandas
    {
        internal DataMRPDV _dbConnection { get; set; }

        public Comandas()
        {
            _dbConnection = new DataMRPDV();
        }

        /// <summary>
        /// Testes <para/>
        /// Retorna no console uma lista com algumas informações das pré vendas a fim de testes.
        /// </summary>
        public static void GetPreVendasInConsole()
        {
            using (var db = new DataMRPDV())
            {
                var query = from preVendas in db.PreVendas
                            select preVendas;

                foreach (var preVenda in query)
                {
                    Console.WriteLine(preVenda);
                }
            }
        }
    }
}
