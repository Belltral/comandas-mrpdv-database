using System;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using ComandasDB.Data.Internal;
using System.Collections.Generic;

namespace ComandasDB
{
    public class Comandas
    {
        //internal DataMRPDVContext _dbConnection { get; set; }

        public Comandas()
        {
            if (!File.Exists(@".\App_Data\ComandasMRPDV.MDF"))
            {
                CreateDatabase.CreateDatabaseIfNoExists();
            }
        }

#region Testes

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
#endregion


        /// <summary>
        /// Localiza no banco local do MR PDV uma pre venda de acordo com um número de pre venda fornecido.
        /// </summary>
        /// <param name="numeroPrevenda"></param>
        /// <returns>
        /// <para>Retorna um objeto PreVendas contendo uma pre venda do banco local do MR PDV.</para>
        /// Obs.: Pode retornar um objeto PreVendas nulo.
        /// </returns>
        public static PreVendas GetPreVendaByNumber(int numeroPreVenda)
        {
            using (var db = new DataMRPDVContext())
            {
                var selectPreVenda = db.PreVendas.SingleOrDefault(p => p.NUMERO_PRVD == numeroPreVenda);

                return selectPreVenda ?? new PreVendas();
            }
        }

        /// <summary>
        /// Localiza no banco local da base de comandas uma pre venda de acordo com um número de pre venda fornecido.
        /// </summary>
        /// <param name="numeroPreVenda"></param>
        /// <returns>
        /// <para>Retorna um objeto PreVendas contendo uma pre venda do banco local da base de comandas.</para>
        /// Obs.: Pode retornar um objeto PreVendas nulo.
        /// </returns>
        public static PreVendas GetPreVendaFromComandasDb(int numeroPreVenda)
        {
            using (var db = new ComandasMRPDVContext())
            {
                var selectFromComandas = db.PreVendas.SingleOrDefault(p => p.NUMERO_PRVD == numeroPreVenda);

                return selectFromComandas ?? new PreVendas();
            }
        }

        /// <summary>
        /// Localiza todas as comandas da base de dados de comandas.
        /// </summary>
        /// <returns>Retorna um objeto de lista de PreVendas iterável da base de comandas.</returns>
        public static List<PreVendas> GetAllPreVendasFromDb()
        {
            List<PreVendas> preVendasList = new List<PreVendas>();

            using (var db = new ComandasMRPDVContext())
            {
                var selectPreVenda = db.PreVendas.Select(s => s);

                foreach (var preVenda in selectPreVenda)
                {
                    preVendasList.Add(preVenda);
                }
            }

            return preVendasList;
        }

        /// <summary>
        /// Recebe um objeto PreVendas e realiza a inserção da pre venda no banco de comandas.
        /// </summary>
        /// <param name="preVenda"></param>
        public static void InsertPreVendaOnLocalDb(PreVendas preVenda)
        {
                using (var db = new ComandasMRPDVContext())
                {
                    db.PreVendas.Add(preVenda);
                    db.SaveChanges();
                }
        }
    }
}
