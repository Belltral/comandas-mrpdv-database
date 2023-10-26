using System;
using System.Data.SqlClient;
using System.Linq;


namespace ComandasDB
{
    public class Comandas
    {
        //internal DataMRPDVContext _dbConnection { get; set; }

        /// <summary>
        /// Testes <para/>
        /// Retorna no console uma lista com algumas informações das pré vendas a fim de testes.
        /// </summary>
        public static void GetPreVendasInConsole()
        {
            try
            {
                using (var db = new DataMRPDVContext())
                {
                    var query = from preVendas in db.PreVendas
                                select preVendas;

                    foreach (var item in query)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Testes <para/>
        /// Retorna no console uma lista com algumas informações sobre os produtos das pré vendas a fim de testes.
        /// </summary>
        public static void GetItensPreVendasInConsole()
        {
            try
            {
                using (var db = new DataMRPDVContext())
                {
                    var query = from itensPreVendas in db.ItensPreVendas
                                select itensPreVendas;

                    foreach (var itemPreVenda in query)
                    {
                        Console.WriteLine(itemPreVenda);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
