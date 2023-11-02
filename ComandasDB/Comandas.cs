using System;
using System.Linq;
using System.Collections.Generic;
using ComandasDB.Data;

namespace ComandasDB
{
    public class Comandas
    {
        //internal DataMRPDVContext _dbConnection { get; set; }

        /// <summary>
        /// Localiza no banco local da base de comandas uma pre venda de acordo com um número de pre venda fornecido.
        /// </summary>
        /// <param name="numeroPreVenda"></param>
        /// <returns>
        /// <para>Retorna um objeto PreVendas contendo uma pre venda do banco local da base de comandas.</para>
        /// Obs.: Pode retornar null.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static PreVendas GetPreVendaFromComandasDb(int numeroPreVenda)
        {
            if (numeroPreVenda <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                var selectPreVenda = db.PreVendas.SingleOrDefault(p => p.NUMERO_PRVD == numeroPreVenda);

                return selectPreVenda;
            }
        }

        /// <summary>
        /// Localiza todas as comandas da base de dados de comandas.
        /// </summary>
        /// <returns>
        /// <para>Retorna um objeto List de PreVendas iterável da base de comandas.</para>
        /// Obs.: Pode retornar um objeto vazio.
        /// </returns>
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
        /// Recebe um objeto PreVendas e realiza a inserção da pré venda na base de comandas.
        /// </summary>
        /// <param name="preVenda"></param>
        /// /// <exception cref="ArgumentException"></exception>
        public static void InsertPreVendaOnComandasDb(PreVendas preVenda)
        {
            if (preVenda.COMANDA_PRVD <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                db.PreVendas.Add(preVenda);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Localiza os itens de uma pré venda específica através do número de pré venda da base de comandas.
        /// </summary>
        /// <param name="numeroPreVenda"></param>
        /// <returns>Retorna um objeto List de ItensPreVendas iterável.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<ItensPreVendas> GetItensPreVendaByNumberOfPreVenda(int numeroPreVenda)
        {
            if (numeroPreVenda <= 0)
            {
                throw new ArgumentException();
            }

            List<ItensPreVendas> itensPreVendaList = new List<ItensPreVendas>();

            using (var db = new ComandasMRPDVContext())
            {
                var itens = db.ItensPreVendas.Select(s => s).Where(n => n.NUMERO_PRVD == numeroPreVenda);

                foreach (var item in itens)
                {
                    itensPreVendaList.Add(item);
                }
            }

            return itensPreVendaList;
        }

        /// <summary>
        /// <para>Localiza todos os itens de todas as pré vendas da base de comandas.</para>
        /// </summary>
        /// <returns>
        /// <para>Retorna um objeto List de PreVendas com todas as pré vendas da base de comandas</para>
        /// Obs.: Pode retornar um objeto vazio.
        /// </returns>
        public static List<ItensPreVendas> GetAllItensFromPreVendas()
        {
            List<ItensPreVendas> itensPreVendasList = new List<ItensPreVendas>();

            using (var db = new ComandasMRPDVContext())
            {
                ItensPreVendas itensPreVendas = new ItensPreVendas();

                var selectFromComandas = db.ItensPreVendas.Select(s => s);

                foreach (var item in selectFromComandas)
                {
                    itensPreVendasList.Add(item);
                }
            }

            return itensPreVendasList;
        }

        /// <summary>
        /// Recebe um objeto List de ItensPreVendas para inserir os itens na base de comandas.
        /// </summary>
        /// <param name="itensPreVenda"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void InsertItensPreVendaOnComandasDb(List<ItensPreVendas> itensPreVenda)
        {
            if (itensPreVenda.Count() == 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasMRPDVContext())
            {
                foreach (var item in itensPreVenda)
                {
                    db.ItensPreVendas.Add(item);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gera uma comanda do MR PDV com todas as informações da base de comandas (tabelas PreVendas e ItensPreVendas) 
        /// através de um número de comanda fornecido como parâmetro.
        /// </summary>
        /// <param name="numeroComanda"></param>
        /// <returns>
        ///<para>Retorna um objeto Comanda.</para>
        /// Obs.: Pode retornar null.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static Comanda GetComanda(int numeroComanda)
        {
            if (numeroComanda <= 0)
            {
                throw new ArgumentException();
            }

            Comanda comanda = new Comanda();

            using (var db = new ComandasMRPDVContext())
            {
                var queryPreVenda = (from preVenda in db.PreVendas
                                    where preVenda.COMANDA_PRVD == numeroComanda
                                    select preVenda).FirstOrDefault();

                var queryItens = from preVendas in db.PreVendas
                            join itensPreVendas in db.ItensPreVendas on preVendas.NUMERO_PRVD equals itensPreVendas.NUMERO_PRVD
                            where preVendas.COMANDA_PRVD == numeroComanda
                            select new
                            {
                                PreVenda = preVendas, ItensPreVendas = itensPreVendas
                            };

                if (queryPreVenda != null && queryItens.Count() > 0)
                {
                    List<ItensPreVendas> itensList = new List<ItensPreVendas>();

                    foreach (var item in queryItens)
                    {
                        itensList.Add(item.ItensPreVendas);
                    }

                    comanda.ItensPreVenda = itensList;

                    comanda.PreVenda = queryPreVenda;

                    return comanda;
                }
            }

            return null;
        }
    }
}
